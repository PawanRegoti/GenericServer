using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Sample.App.SwaggerScripts
{
  public class UserIdHeaderFilter : IOperationFilter
  {
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
      if (operation.Parameters == null)
        operation.Parameters = new List<OpenApiParameter>();

      operation.Parameters.Add(new OpenApiParameter
      {
        Name = "UserId",
        In = ParameterLocation.Header,
        AllowEmptyValue = false,        
        Required = true
      });
    }
  }
}
