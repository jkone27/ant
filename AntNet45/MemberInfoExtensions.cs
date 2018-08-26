using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace AntNet45
{
    public static class MemberInfoExtensions
    {

        public static string RequestUri<T>(this MemberInfo actionMethod, string baseUri)
        where T : ApiController
        {
            var controllerPrefix = typeof(T).GetCustomAttributes()
                .OfType<RoutePrefixAttribute>().LastOrDefault()?.Prefix;

            var routeAttribute = actionMethod
                .GetCustomAttributes().OfType<RouteAttribute>().LastOrDefault()?.Template;

            if (routeAttribute != null)
            {
                var indexOfTemplate = routeAttribute.IndexOf('{');
                if (indexOfTemplate >= 0)
                {
                    routeAttribute = routeAttribute.Remove(indexOfTemplate);
                }
            }

            var requestUri = new Uri(baseUri).Append(controllerPrefix, routeAttribute).ToString();
            return requestUri;
        }

        public static HttpMethod GetHttpMethod(this MemberInfo actionMethod)
        {
            var httpVerbs = actionMethod.GetCustomAttributes()
                .Select(t => t.GetType().Name)
                .Where(n => n.Contains("Http"))
                .ToArray();

            var method = httpVerbs.Any() ? DeriveFromHttpAttribute(httpVerbs) 
                : DeriveFromSignatureName(actionMethod.Name);

            if (method == null)
            {
                throw new ArgumentException($"unable to derive {nameof(HttpMethod)}.");
            }

            return method;
        }

        private static HttpMethod DeriveFromHttpAttribute(string[] httpVerbs)
        {
            string verb = httpVerbs.LastOrDefault()
                .Replace("Http", String.Empty)
                .Replace("Attribute", String.Empty)
                .ToUpperInvariant();

            var method = new HttpMethod(verb);
            return method;
        }

        private static HttpMethod DeriveFromSignatureName(string methodName)
        {
            var httpMethods = new Regex("Get|Put|Post|Delete|Head|Options|Trace");
            var match = httpMethods.Match(methodName);
            string verb = match.Value.ToUpperInvariant();
            var method = new HttpMethod(verb);
            return method;
        }
    }
}