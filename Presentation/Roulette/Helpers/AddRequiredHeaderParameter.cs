using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

public class AddRequiredHeaderParameter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Verifica si el endpoint actual contiene "bet" en la ruta
        var isBetEndpoint = context.ApiDescription.RelativePath
            .Contains("/bet", StringComparison.OrdinalIgnoreCase);

        if (!isBetEndpoint)
            return; // Si no es /bet, no agrega el header

        operation.Parameters ??= new List<OpenApiParameter>();

        operation.Parameters.Add(new OpenApiParameter
        {
            Name = "User-Id",
            In = ParameterLocation.Header,
            Required = true,
            Schema = new OpenApiSchema
            {
                Type = "string"
            },
            Description = "User-Id"
        });
    }
}