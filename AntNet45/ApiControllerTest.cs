using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;
using Microsoft.Practices.Unity;
using Unity;
using Unity.WebApi;

namespace AntNet45
{
    public class ApiControllerTest<T> where T : ApiController
    {
        private readonly T controller;
        private readonly IEnumerable<IFilter> filters;
        private readonly Action<HttpConfiguration> httpConfigurationAction;
        private readonly IEnumerable<DelegatingHandler> handlers;
        private static Action<HttpConfiguration> Noop => _ => { };

        private ApiControllerTest(T controller,
            IEnumerable<DelegatingHandler> handlers,
            IEnumerable<IFilter> filters,
            Action<HttpConfiguration> httpConfigurationAction)
        {
            this.controller = controller;
            this.handlers = handlers;
            this.filters = filters;
            this.httpConfigurationAction = httpConfigurationAction;
        }

        public static ApiControllerTest<T> Get(
            T controller, 
            IEnumerable<DelegatingHandler> handlers = null,
            IEnumerable<IFilter> filters = null,
            Action<HttpConfiguration> httpConfigurationAction = null)
        {
            var filterCollection = filters ?? new IFilter[] { };
            var customConfiguration = httpConfigurationAction ?? Noop;
            var httpMessageHandlers = handlers ?? new DelegatingHandler[] { };
            return new ApiControllerTest<T>(controller, httpMessageHandlers, filterCollection, customConfiguration);
        }

        private HttpServer GetInMemoryHttpServer()
        {
            var container = new UnityContainer();
            container.RegisterInstance(controller);
            var resolver = new UnityDependencyResolver(container);
            var httpConfiguration = new HttpConfiguration
            {
                DependencyResolver = resolver
            };
            httpConfiguration.Routes.MapHttpRoute("Default", "{controller}");
            httpConfiguration.MapHttpAttributeRoutes();
            DelegatingHandlers(httpConfiguration);
            httpConfiguration.Filters.AddRange(filters);
            httpConfigurationAction(httpConfiguration);
            return new HttpServer(httpConfiguration);
        }

        private void DelegatingHandlers(HttpConfiguration httpConfiguration)
        {
            if (!handlers.Any())
            {
                return;
            }

            foreach (var h in handlers)
            {
                httpConfiguration.MessageHandlers.Add(h);
            }
        }

        public async Task<TResult> HttpRequest<TResult>(
           HttpMethod method,
           string requestUri,
           Expression<Func<HttpResponseMessage, TResult>> responseFunction,
           Action<HttpRequestMessage> customRequestAction = null
           )
        {
            using (var server = GetInMemoryHttpServer())
            using (var client = new HttpMessageInvoker(server))
            using (var request = new HttpRequestMessage(method, requestUri))
            {
                var action = customRequestAction ?? (r => { });
                action(request);
                using (var response = await client.SendAsync(request, CancellationToken.None))
                {
                    return responseFunction.Compile()(response);
                }
            }
        }

        public async Task<TResult> BuildHttpRequest<TResult>(
            Expression<Func<IHttpActionResult>> invokedAction,
            Expression<Func<HttpResponseMessage, TResult>> responseFunction,
            Action<HttpRequestMessage> customRequestAction = null,
            String baseUri = "http://localhost"
        )
        {
            var derivedRequest = invokedAction.DeriveRequestUriAndHttpMethod<T>(baseUri);
            using (var server = GetInMemoryHttpServer())
            using (var client = new HttpMessageInvoker(server))
            {
                using (var request = derivedRequest)
                {
                    var action = customRequestAction ?? (r => { });
                    action(request);
                    using (var response = await client.SendAsync(request, CancellationToken.None))
                    {
                        return responseFunction.Compile()(response);
                    }
                }
            }
        }

    }
}