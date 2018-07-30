using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace AntNet45Tests.Helpers
{
    public sealed class AssignClaimsAuthenticationFilterAttribute : Attribute, IAuthenticationFilter
    {

        // aspnet webapi pipeline
        // HTTP req --> DelegatingHandler(X) --> AuthenticationFilter(X) --> AuthorizationFilter(DUT) --> CustomFilter --> Controller(DUT) -->

        //assign roles via authentication filter (before authorization filters)

        private readonly string[] roles;

        private AssignClaimsAuthenticationFilterAttribute(params string[] roles)
        {
            this.roles = roles;
        }

        public static IEnumerable<IFilter> Parse(params string[] roles)
        {
            return new[] {new AssignClaimsAuthenticationFilterAttribute(roles)};
        }

        public bool AllowMultiple => false;

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (roles.Any())
            {
                var claims = roles.Select(p => new Claim(ClaimTypes.Role, p)).ToList();
                var principal = new ClaimsPrincipal(new List<ClaimsIdentity>
                {
                    new ClaimsIdentity(claims)
                });
                context.ActionContext.RequestContext.Principal = principal;
                context.Principal = principal;
            }

            return Task.FromResult(0);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult(0);
        }
    }
}