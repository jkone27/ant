using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace AntNet45
{
    public static class ApiControllerTestExtensions
    {
        public static ApiControllerTest<T> Test<T>(
            this T controller, 
            IEnumerable<DelegatingHandler> messageHandlers = null, 
            IEnumerable<IFilter> filters = null, 
            Action<HttpConfiguration> customSetup = null)
        where T : ApiController
        {
            return ApiControllerTest<T>.Get(controller, messageHandlers, filters, customSetup);
        }

        public static Task<T> GetAsync<TController,T>(
            this ApiControllerTest<TController> test, 
            string url, 
            Expression<Func<HttpResponseMessage, T>> expression)
            where TController : ApiController
        {
            return test.HttpRequest(HttpMethod.Get, url, expression);
        }

        public static Task<T> PostAsync<TController,T>(
            this ApiControllerTest<TController> test, 
            string url, 
            Expression<Func<HttpResponseMessage, T>> expression,
            string body = null)
            where TController : ApiController
        {

            var action = body != null ? r => SetBody(r,body) : new Action<HttpRequestMessage>(r => { });
            return test.HttpRequest(HttpMethod.Post, url, expression, action);
        }

        private static void SetBody(HttpRequestMessage message, string body)
        {
            message.Content = new StringContent(body, Encoding.UTF8, "application/json");
        }

    }
    
}