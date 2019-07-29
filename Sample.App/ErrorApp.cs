using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sample.App.Models.ApiResponse;

namespace Sample.App
{
	//	Catches and logs unhandled errors/exceptions
	public class ErrorApp
	{
		private IApplicationBuilder _errorApp;
		private ILogger _logger;
		public ErrorApp(IApplicationBuilder errorApp, ILogger logger)
		{
			_errorApp = errorApp ?? throw new ArgumentNullException(nameof(errorApp));
			_logger = logger;
		}

		public void Run()
		{
			_errorApp.Run(async context =>
			{
				var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
				var exception = errorFeature.Error;

				//	TODO: Assign Instance property
				var problemDetails = new ProblemDetails { };

				//	Kestrel thrown error, that should not be returned as 500
				if (exception is BadHttpRequestException badHttpRequestException)
				{
					problemDetails.Title = "Invalid request";

					var type = typeof(BadHttpRequestException);
					var propertyInfo = type.GetProperty("StatusCode",
						BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
					if (propertyInfo != null)
					{
						problemDetails.Status = (int) propertyInfo.GetValue(badHttpRequestException);
					}
					problemDetails.Detail = badHttpRequestException.Message;
				}
				else if (exception is ValidationException validationException)
				{
					problemDetails.Title = "Bad request!";
					problemDetails.Status = (int) HttpStatusCode.BadRequest;
					problemDetails.Detail = exception.Message;
				}
				else if (exception is IntegrityException integrityException)
				{
					problemDetails.Title = "Not found!";
					problemDetails.Status = (int) HttpStatusCode.NotFound;
					problemDetails.Detail = exception.Message;
				}
				else
				{
					problemDetails.Title = "An unexpected error occurred!";
					problemDetails.Status = (int) HttpStatusCode.InternalServerError;
					problemDetails.Detail = exception.Message;
				}

				var problemDetailsJson = FormatResponse(problemDetails, context);
				await context.Response.WriteAsync(problemDetailsJson);
			});
		}

		private string FormatResponse(ProblemDetails problemDetails, HttpContext context)
		{
			if (problemDetails.Status != null)
			{
				context.Response.StatusCode = problemDetails.Status.Value;
			}
			context.Response.Headers.Add("Content-Type", "application/json");
			return SerializeProblemDetails(problemDetails);
		}

		private string SerializeProblemDetails(ProblemDetails problemDetails)
		{
			return JsonConvert.SerializeObject(problemDetails,
				Newtonsoft.Json.Formatting.None,
				new JsonSerializerSettings
				{
					NullValueHandling = NullValueHandling.Ignore
				});
		}
	}
}
