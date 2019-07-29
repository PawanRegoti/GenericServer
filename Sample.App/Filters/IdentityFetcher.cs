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
			var userId = GetUserIdFromHttpContext(httpContext);

			var identityProvider = (IIdentityProvider) httpContext.RequestServices.GetService(typeof(IIdentityProvider));
      identityProvider.SetUserId(userId);
		}

		private int GetUserIdFromHttpContext(HttpContext httpContext)
		{
			if (Int32.TryParse(httpContext.Request.Headers["UserId"], out int userId))
			{
				return userId;
			}

			throw new NullReferenceException("Could not retrieve user id");
		}
	}
}
