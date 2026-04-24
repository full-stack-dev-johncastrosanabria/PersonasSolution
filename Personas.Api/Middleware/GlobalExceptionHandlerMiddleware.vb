Imports System.Net
Imports System.Text.Json
Imports Microsoft.AspNetCore.Http
Imports Microsoft.Extensions.Logging

Namespace Personas.Api.Middleware
    Public Class GlobalExceptionHandlerMiddleware
        Private ReadOnly _next As RequestDelegate
        Private ReadOnly _logger As ILogger(Of GlobalExceptionHandlerMiddleware)

        Public Sub New([next] As RequestDelegate, logger As ILogger(Of GlobalExceptionHandlerMiddleware))
            _next = [next]
            _logger = logger
        End Sub

        Public Async Function InvokeAsync(context As HttpContext) As Task
            Try
                Await _next(context)
            Catch ex As Exception
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message)
                Call HandleExceptionAsync(context, ex)
            End Try
        End Function

        Private Shared Sub HandleExceptionAsync(context As HttpContext, exception As Exception)
            context.Response.ContentType = "application/json"

            Dim statusCode As HttpStatusCode
            Dim message As String

            Select Case exception.GetType()
                Case GetType(KeyNotFoundException)
                    statusCode = HttpStatusCode.NotFound
                    message = exception.Message
                Case GetType(InvalidOperationException)
                    statusCode = HttpStatusCode.BadRequest
                    message = exception.Message
                Case GetType(UnauthorizedAccessException)
                    statusCode = HttpStatusCode.Unauthorized
                    message = "No autorizado."
                Case GetType(ArgumentException), GetType(ArgumentNullException)
                    statusCode = HttpStatusCode.BadRequest
                    message = exception.Message
                Case Else
                    statusCode = HttpStatusCode.InternalServerError
                    message = "Ha ocurrido un error interno en el servidor."
            End Select

            context.Response.StatusCode = CInt(statusCode)

            Dim response = New With {
                .statusCode = CInt(statusCode),
                .message = message,
                .timestamp = DateTime.UtcNow
            }

            Dim options = New JsonSerializerOptions With {
                .PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }

            Dim jsonResponse = JsonSerializer.Serialize(response, options)
            Dim bytes = System.Text.Encoding.UTF8.GetBytes(jsonResponse)
            context.Response.Body.Write(bytes, 0, bytes.Length)
        End Sub
    End Class
End Namespace
