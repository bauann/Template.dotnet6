using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template.dotnet6.webapi;

/// <summary>
/// Represents the Swagger/Swashbuckle operation filter used to document the implicit API version parameter.
/// </summary>
/// <remarks>This <see cref="IOperationFilter"/> is only required due to bugs in the <see cref="SwaggerGenerator"/>.
/// Once they are fixed and published, this class can be removed.</remarks>
public class SwaggerDefaultValues : IOperationFilter
{
    /// <summary>
    /// Applies the filter to the specified operation using the given context.
    /// </summary>
    /// <param name="operation">The operation to apply the filter to.</param>
    /// <param name="context">The current operation filter context.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        operation.Deprecated |= apiDescription.IsDeprecated();

        if (operation.Parameters == null)
        {
            return;
        }

        // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
        // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

            if (parameter.Description == null)
            {
                parameter.Description = description.ModelMetadata?.Description;
            }

            if (parameter.Schema.Default == null && description.DefaultValue != null)
            {
                parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
            }

            parameter.Required |= description.IsRequired;
        }

        var descriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
        if (descriptor != null)
        {
            //ServiceFilterAttribute
            var actionAttributes = descriptor.MethodInfo.GetCustomAttributes(inherit: true);

            var d = actionAttributes.Where(a => a is TypeFilterAttribute);
            foreach (var item in d)
            {
                if ((item as TypeFilterAttribute).ImplementationType.Name == "SvcTkRequired" ||
                    (item as TypeFilterAttribute).ImplementationType.Name == "UserTkRequired")
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = "X-Token",
                        In = ParameterLocation.Header,
                        Description = "access token",
                        Required = true
                    });
                }
                else if ((item as TypeFilterAttribute).ImplementationType.Name == "ApiTkRequired")
                {
                    operation.Parameters.Add(new OpenApiParameter
                    {
                        Name = "x-apitoken",
                        In = ParameterLocation.Header,
                        Description = "存取驗證碼 Access Token，client_id + client_secret 透過 SHA256 編碼再轉 Base64",
                        Required = true
                    });
                }
            }
        }
    }
}