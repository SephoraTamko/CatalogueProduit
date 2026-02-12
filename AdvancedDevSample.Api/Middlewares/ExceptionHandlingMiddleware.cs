using AdvancedDevSample.Application.Exceptions;
using AdvancedDevSample.Domain.Exceptions;
using AdvanceDevSample.Infrastructure.Exceptions;
using System.Net;

namespace AdvancedDevSample.Api.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        // Si tu utilises AllowCredentials côté CORS, mets ici l’origine exacte
        private const string AllowedOrigin = "http://localhost:5083"; // adapte si besoin
        private const bool UseWildcardOrigin = true; // passe à false si AllowCredentials = true

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainException ex)
            {
                _logger.LogError(ex, "Erreur métier");
                await WriteProblemJsonAsync(context, StatusCodes.Status400BadRequest,
                    title: "Erreur métier", detail: ex.Message);
            }
            catch (ApplicationServiceException ex)
            {
                _logger.LogWarning(ex, "Erreur applicative");
                await WriteProblemJsonAsync(context, (int)ex.StatusCode,
                    title: "Erreur applicative", detail: ex.Message);
            }
            catch (InfrastructureException ex)
            {
                _logger.LogError(ex, "Erreur technique");
                await WriteProblemJsonAsync(context, StatusCodes.Status500InternalServerError,
                    title: "Erreur technique", detail: "Une erreur technique est survenue.");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Erreur inattendue");
                await WriteProblemJsonAsync(context, StatusCodes.Status500InternalServerError,
                    title: "Erreur interne", detail: "Une erreur inattendue est survenue.");
            }
        }

        private static void EnsureCorsHeaders(HttpContext context)
        {
            // Ajoute systématiquement CORS sur les erreurs
            if (UseWildcardOrigin)
            {
                // OK si tu n'utilises PAS AllowCredentials
                context.Response.Headers["Access-Control-Allow-Origin"] = "*";
            }
            else
            {
                // Obligatoire si AllowCredentials = true
                context.Response.Headers["Access-Control-Allow-Origin"] = AllowedOrigin;
                context.Response.Headers["Vary"] = "Origin"; // bonne pratique
            }

            context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization";
            context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";
        }

        private static async Task WriteProblemJsonAsync(
            HttpContext context,
            int statusCode,
            string title,
            string detail)
        {
            if (!context.Response.HasStarted)
            {
                context.Response.Clear();
                context.Response.StatusCode = statusCode;
                context.Response.ContentType = "application/json";
                EnsureCorsHeaders(context);
            }

            var payload = new
            {
                title,
                detail,
                status = statusCode,
            };
        }
    }
}