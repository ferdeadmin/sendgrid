using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Linq;

namespace Utility.API.Filters
{
    public class FileUploadOperation : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var parameters = operation.Parameters;
            if (parameters == null || parameters.Count == 0)
            {
                return;
            }
            var isUploadFile = context.ApiDescription.ActionDescriptor.
                                Parameters.Any(x => x.ParameterType == typeof(IFormFile));

            if (isUploadFile)
            {
                operation.RequestBody = new OpenApiRequestBody()
                {
                    Content =
                        {
                            ["multipart/form-data"] = new OpenApiMediaType()
                            {
                                Schema = new OpenApiSchema()
                                {
                                    Type = "object",
                                    Properties =
                                    {
                                        ["file"] = new OpenApiSchema()
                                        {
                                            Description = "Select file",
                                            Type = "string",
                                            Format = "binary"
                                        }
                                    }
                                }
                            }
                        }
                };
            }
        }
    }
}
