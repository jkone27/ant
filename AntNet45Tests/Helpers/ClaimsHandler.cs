using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace AntNet45Tests.Helpers
{
    public sealed class ClaimsHandler : DelegatingHandler
    {
        //assign roles via delegating handler (before login/authentication filters)

        private readonly string[] roles;

        private ClaimsHandler(params string[] roles)
        {
            this.roles = roles;
        }

        public static IEnumerable<DelegatingHandler> Parse(params string[] roles)
        {
            return new[] {new ClaimsHandler(roles)};
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (roles.Any())
            {
                var claims = roles.Select(p => new Claim(ClaimTypes.Role, p)).ToList();
                var principal = new ClaimsPrincipal(new List<ClaimsIdentity>
                {
                    new ClaimsIdentity(claims)
                });
                var reqContext = request.GetRequestContext();
                reqContext.Principal = principal;
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}