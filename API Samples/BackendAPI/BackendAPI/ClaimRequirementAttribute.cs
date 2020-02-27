using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;

public sealed class ClaimRequirementAttribute : TypeFilterAttribute
{
    public ClaimRequirementAttribute() : base(typeof(ClaimRequirementFilter))
    {
        Arguments = new object[] { new Claim("CustomHeader", "True") };
    }
}

public class ClaimRequirementFilter : IAuthorizationFilter
{
    readonly Claim _claim;

    public ClaimRequirementFilter(Claim claim)
    {
        _claim = claim;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var hasClaim = context.HttpContext.Request.Headers.ContainsKey("CustomHeader");
        if (!hasClaim)
        {
            context.Result = new ForbidResult();
        }
    }
}
