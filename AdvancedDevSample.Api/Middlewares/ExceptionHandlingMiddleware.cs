using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Domain.Exceptions;
using AdvanceDevSample.Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AdvancedDevSample.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                _logger.LogWarning(ex, "Erreur de domaine: {Message}", ex.Message);

                var problem = new ProblemDetails
                {
                    Type = "https://httpstatuses.com/400",
                    Title = "Erreur de validation",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/problem+json; charset=utf-8";
                await context.Response.WriteAsJsonAsync(problem);
            }
            catch (ApplicationServiceException ex)
            {
                _logger.LogWarning(ex, "Erreur applicative: {Message}", ex.Message);

                var problem = new ProblemDetails
                {
                    Type = $"https://httpstatuses.com/{(int)ex.StatusCode}",
                    Title = "Erreur de traitement",
                    Status = (int)ex.StatusCode,
                    Detail = ex.Message,
                    Instance = context.Request.Path
                };
                context.Response.StatusCode = (int)ex.StatusCode;
                context.Response.ContentType = "application/problem+json; charset=utf-8";
                await context.Response.WriteAsJsonAsync(problem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur interne non gérée");
                var problem = new ProblemDetails
                {
                    Type = "https://httpstatuses.com/500",
                    Title = "Erreur interne du serveur",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "Une erreur inattendue est survenue.",
                    Instance = context.Request.Path
                };
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json; charset=utf-8";
                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}