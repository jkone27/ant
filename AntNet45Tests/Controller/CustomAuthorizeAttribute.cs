using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace AntNet45Tests.Controller
{
    public sealed class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly string[] roles;

        public CustomAuthorizeAttribute(params string[] roles)
        {
            this.roles = roles;
        }
        
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var principal = actionContext.RequestContext.Principal;
            return roles.All(principal.IsInRole);
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden);
        }

    }
}