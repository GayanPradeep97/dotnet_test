using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GameStore.API.Middleware
{
    public class ValidationErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await _next(context);

            if (context.Response.StatusCode == 400 && context.Items["errors"] is ModelStateDictionary modelState)
            {
                // Flatten validation errors
                var errors = modelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                // Build response object
                var response = new
                {
                    type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    title = "One or more validation errors occurred.",
                    status = 400,
                    errors
                };

                // Write JSON response
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
