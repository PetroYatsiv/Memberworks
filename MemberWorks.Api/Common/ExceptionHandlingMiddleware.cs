using System.Net;
using FluentValidation;
using MemberWorks.Application.Common.Exceptions;

namespace MemberWorks.Api.Common;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());
            await WriteProblem(context, HttpStatusCode.BadRequest, "Validation failed", errors);
        }
        catch (NotFoundException ex)
        {
            await WriteProblem(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (ForbiddenAccessException ex)
        {
            await WriteProblem(context, HttpStatusCode.Forbidden, ex.Message);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception");
            await WriteProblem(context, HttpStatusCode.InternalServerError, "An unexpected error occurred.");
        }
    }

    private static async Task WriteProblem(
        HttpContext context, HttpStatusCode status, string title, object? errors = null)
    {
        context.Response.StatusCode = (int)status;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            status = (int)status,
            title,
            errors
        });
    }
}