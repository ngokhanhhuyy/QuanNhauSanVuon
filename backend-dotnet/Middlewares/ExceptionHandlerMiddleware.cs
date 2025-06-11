using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.Options;
using QuanNhauSanVuon.Extensions;
using QuanNhauSanVuon.Services.Exceptions;
using QuanNhauSanVuon.Services.Structs;

namespace QuanNhauSanVuon.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IOptions<JsonOptions> jsonOptions)
    {
        JsonSerializerOptions serializerOptions = jsonOptions.Value.SerializerOptions;
        JsonNamingPolicy namingPolicy = serializerOptions.PropertyNamingPolicy; 
        bool isCamelCasePolicy = namingPolicy == JsonNamingPolicy.CamelCase;

        try
        {
            await _next(context);
        }
        catch (ValidationException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            PropertyErrorDetail[] errorDetails = exception.Errors
                .Select(failure =>
                {
                    object[] propertyPathElements = failure.PropertyName
                        .Replace(@"(\[|\].)", ".")
                        .Replace("]", ".")
                        .Split(".")
                        .Select<string, object>(element =>
                        {
                            if (int.TryParse(element, out int intElement))
                            {
                                return intElement;
                            }

                            if (isCamelCasePolicy)
                            {
                                return element.LowerCaseFirstLetter();
                            }

                            return element;
                        }).ToArray();
                    return new PropertyErrorDetail
                    {
                        PropertyPathElements = propertyPathElements,
                        ErrorMessage = failure.ErrorMessage
                    };
                }).ToArray();

            await context.Response.WriteAsJsonAsync(errorDetails);
        }
        catch (NotFoundException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            if (exception.PropertyPathElements != null)
            {
                await context.Response.WriteAsJsonAsync(new[]
                {
                    new PropertyErrorDetail
                    {
                        PropertyPathElements = LowerCasePropertyPathElementsIfConfigured(
                            exception.PropertyPathElements,
                            isCamelCasePolicy),
                        ErrorMessage = exception.Message
                    }
                });
            }
        }
        catch (OperationException exception)
        {
            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            await context.Response.WriteAsJsonAsync(new[]
            {
                new PropertyErrorDetail
                {
                    PropertyPathElements = LowerCasePropertyPathElementsIfConfigured(
                        exception.PropertyPathElements,
                        isCamelCasePolicy),
                    ErrorMessage = exception.Message
                }
            });
        }
        catch (ConcurrencyException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
        }
    }

    private static object[] LowerCasePropertyPathElementsIfConfigured(
        object[] propertyPathElements,
        bool isCamelCasePolicy)
    {
        if (isCamelCasePolicy)
        {
            for (int index = 0; index < propertyPathElements.Length; index++)
            {
                if (propertyPathElements[index] is string stringElement)
                {
                    propertyPathElements[index] = stringElement.LowerCaseFirstLetter();
                }
            }
        }

        return propertyPathElements;
    }
}