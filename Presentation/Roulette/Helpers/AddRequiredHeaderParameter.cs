using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Roulette.Helpers
{
    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.RelativePath == null)
                return;

            // Agrega el header solo a endpoints que contengan "/bet"
            var isBetEndpoint = context.ApiDescription.RelativePath
                .Contains("/bet", StringComparison.OrdinalIgnoreCase);

            if (!isBetEndpoint)
                return;

            operation.Parameters ??= new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "User-Id",
                In = ParameterLocation.Header,
                Required = true,
                Schema = new OpenApiSchema { Type = "string" },
                Description = "User ID requerido para realizar apuestas"
            });
        }
    }
}
