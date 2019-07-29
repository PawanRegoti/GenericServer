using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace Sample.App.SwaggerScripts
{
  public class UserIdHeaderFilter : IOperationFilter
  {
    public void Apply(Operation operation, OperationFilterContext context)
    {
      if (operation.Parameters == null)
        operation.Parameters = new List<IParameter>();

      operation.Parameters.Add(new NonBodyParameter
      {
        Name = "UserId",
        In = "header",
        Type = "string",
        Required = true
      });
    }
  }
}
