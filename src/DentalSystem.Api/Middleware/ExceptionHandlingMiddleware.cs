using Azure;
using DentalSystem.Application.Exceptions;
using DentalSystem.Domain.Exceptions;
using DentalSystem.Domain.Exceptions.Rules;
using DentalSystem.Domain.Exceptions.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.ConstrainedExecution;

namespace DentalSystem.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        // TODO: If we use loggers, we need to create the property here
        //private readonly ILogger<> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next
            //ILogger<> logger
            )
        {
            _next = next;
            //_logger = logger;
        }


        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                // Add this In case something already
                // wrote a body or header before throwing the exception
                if (context.Response.HasStarted)
                {
                    throw;
                }

                string type;
                string detail;
                string title;
                int status;

                // Check if exception is from Application
                if (exception is NotFoundException)
                {
                    type = "resource-not-found";
                    status = StatusCodes.Status404NotFound;
                    detail = exception.Message;
                    title = "Resource not found";
                }
                // Check if exception is from domain
                else if (exception is DomainException)
                {
                    detail = exception.Message;

                    switch (exception)
                    {
                        case ValueObjectException:
                            type = "value-object-invalid";
                            status = StatusCodes.Status400BadRequest;
                            title = "Invalid input";
                            break;

                        case BusinessRuleViolationException:
                            type = "business-rule-violation";
                            status = StatusCodes.Status409Conflict;
                            title = "Business rule violation";
                            break;

                        default:
                            type = "domain-error";
                            status = StatusCodes.Status409Conflict; ;
                            title = "Domain conflict";
                            break;
                    }

                }
                else
                {
                    // Unexpected errors: 500
                    status = StatusCodes.Status500InternalServerError;
                    detail = "An unexpected error occurred.";
                    type = "internal-error";
                    title = "Internal server error";

                    // TODO: Log errors: Only log unexpected (5xx) errors
                }

                // Complete this problem detail with the correct information
                var problemDetails = new ProblemDetails
                {
                    Status = status,
                    Title = title,
                    Detail = detail,
                    Type = type,
                    Instance = context.Request.Path
                };

                // Prepare the response props

                // Clear the response, because some previous middleware
                // may have added headers.
                //Clear() ensures that the error response is consistent.
                context.Response.Clear();
                context.Response.ContentType = "application/problem+json";
                context.Response.StatusCode = status;
                await context.Response.WriteAsJsonAsync(problemDetails);
            }

        }
    }
}
