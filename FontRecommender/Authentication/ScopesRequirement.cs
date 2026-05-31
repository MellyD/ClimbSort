using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;

namespace FontRecommender.Authentication
{
    public class ScopesRequirement: AuthorizationHandler<ScopesRequirement>, IAuthorizationRequirement
    {
        private readonly string[] _acceptedScopes;

        public ScopesRequirement(params string[] acceptedScopes)
        {
            _acceptedScopes = acceptedScopes;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ScopesRequirement requirement)
        {
            if(!context.User.Claims.Any(x => x.Type == ClaimConstants.Scope) && !context.User.Claims.Any(y => y.Type == ClaimConstants.Scp))
            {
                return Task.CompletedTask;
            }

            var scopeClaim = context?.User?.FindFirst(x => x.Type == ClaimConstants.Scp) ?? context?.User?.FindFirst(y => y.Type == ClaimConstants.Scope);

            if(scopeClaim != null && scopeClaim.Value.Split(' ').Intersect(requirement._acceptedScopes).Any())
            {
                context?.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
