using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sample.App.Filters
{
	public class IdentityFetcher : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext actionContext)
		{
			var httpContext = actionContext.HttpContext;
			var setupId = GetUserIdFromHttpContext(httpContext);

			var identityProvider = (IIdentityProvider) httpContext.RequestServices.GetService(typeof(IIdentityProvider));
      identityProvider.SetUserId(setupId);
		}

		private int GetUserIdFromHttpContext(HttpContext httpContext)
		{
			var claims = httpContext.User.Claims.Where(claim => claim?.Type == CustomClaimTypes.UserId);
			if (claims.Any())
			{
				if (Int32.TryParse(claims.First().Value, out int setupId))
				{
					return setupId;
				}
			}
			throw new NullReferenceException("Could not retrieve user id");
		}
	}
}
