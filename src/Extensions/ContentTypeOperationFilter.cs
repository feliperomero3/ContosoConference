using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ContosoConference.Api.Extensions;

internal class ContentTypeOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Responses.ContainsKey("200"))
        {
            operation.Responses["200"].Content.Remove("text/plain");
        }
    }
}