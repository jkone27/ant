using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;

namespace AntNet45
{
    public static class ExpressionExtensions
    {

        public static (HttpMethod Method, string RequestUri) DeriveRequestUriAndHttpMethod<T>(
            this Expression<Func<IHttpActionResult>> invokedAction, string baseUri)
        where T : ApiController
        {
            var actionMethod = invokedAction.GetActionMethodInfo<T>();
            var method = actionMethod.GetHttpMethod();
            var requestUri = actionMethod.RequestUri<T>(baseUri);
            return (method, requestUri);
        }

        public static MemberInfo GetActionMethodInfo<T>(
            this Expression<Func<IHttpActionResult>> invokedAction)
        {
            var memberInfo = invokedAction.GetMemberInfo();
            var controllerName = typeof(T).Name;
            if (controllerName != memberInfo?.DeclaringType?.Name)
            {
                throw new ArgumentException(
                    $"class {memberInfo.DeclaringType.Name} does not match controller: {controllerName}");
            }

            var methodName = memberInfo.Name;
            return typeof(T).GetMethod(methodName);
        }

        private static MemberInfo GetMemberInfo(this LambdaExpression expression)
        {
            return ((MethodCallExpression)expression.Body).Method;
        }
    }
}