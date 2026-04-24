Imports Microsoft.AspNetCore.Builder
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports Microsoft.Extensions.Configuration
Imports Microsoft.EntityFrameworkCore
Imports Personas.Business
Imports Personas.EF
Imports Personas.Repository
Imports Personas.Api.Extensions
Imports Personas.Api.Middleware
Imports Microsoft.AspNetCore.Diagnostics.HealthChecks
Imports System.Text.Json

Module Program
    Sub Main(args As String())
        Dim builder = WebApplication.CreateBuilder(args)

        ' Configuration
        Dim connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        Dim allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get(Of String())()

        ' Add services to the container
        builder.Services.AddControllers()
        builder.Services.AddEndpointsApiExplorer()

        ' Swagger configuration
        builder.Services.AddSwaggerDocumentation()

        ' FluentValidation
        builder.Services.AddFluentValidationConfig()

        ' CORS configuration
        builder.Services.AddCors(Sub(options)
                                     options.AddPolicy("CorsPolicy", Sub(policy)
                                                                         policy.WithOrigins(allowedOrigins).
                                                                                AllowAnyHeader().
                                                                                AllowAnyMethod().
                                                                                AllowCredentials()
                                                                     End Sub)
                                 End Sub)

        ' Database context
        builder.Services.AddDbContext(Of AppDbContext)(
            Sub(options)
                options.UseSqlServer(connectionString)
                If builder.Environment.IsDevelopment() Then
                    options.EnableSensitiveDataLogging()
                    options.EnableDetailedErrors()
                End If
            End Sub)

        ' Health checks
        builder.Services.AddHealthChecksConfig(connectionString)

        ' Dependency injection
        builder.Services.AddScoped(Of IPersonaRepository, PersonaRepository)()
        builder.Services.AddScoped(Of IPersonaService, PersonaService)()

        ' Build the application
        Dim app = builder.Build()

        ' Configure the HTTP request pipeline
        ' Global exception handler
        app.UseMiddleware(Of GlobalExceptionHandlerMiddleware)()

        If app.Environment.IsDevelopment() Then
            app.UseSwagger()
            app.UseSwaggerUI(Sub(c)
                                 c.SwaggerEndpoint("/swagger/v1/swagger.json", "Personas API v1")
                                 c.RoutePrefix = String.Empty ' Swagger at root
                             End Sub)
        End If

        ' Health check endpoint
        app.MapHealthChecks("/health", New HealthCheckOptions With {
            .ResponseWriter = Function(context, report) Task.Run(Async Function()
                                  context.Response.ContentType = "application/json"
                                  Dim result = JsonSerializer.Serialize(New With {
                                      .status = report.Status.ToString(),
                                      .checks = report.Entries.Select(Function(e) New With {
                                          .name = e.Key,
                                          .status = e.Value.Status.ToString(),
                                          .description = e.Value.Description,
                                          .duration = e.Value.Duration.TotalMilliseconds
                                      }),
                                      .totalDuration = report.TotalDuration.TotalMilliseconds
                                  })
                                  Dim bytes = System.Text.Encoding.UTF8.GetBytes(result)
                                  Await context.Response.Body.WriteAsync(bytes, 0, bytes.Length)
                              End Function)
        })

        app.UseCors("CorsPolicy")
        app.UseHttpsRedirection()
        app.UseAuthorization()
        app.MapControllers()

        app.Run()
    End Sub
End Module