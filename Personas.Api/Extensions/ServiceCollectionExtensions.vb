Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.OpenApi.Models
Imports FluentValidation
Imports FluentValidation.AspNetCore
Imports Personas.Business
Imports Personas.Business.Validators

Namespace Personas.Api.Extensions
    Public Module ServiceCollectionExtensions
        <Runtime.CompilerServices.Extension>
        Public Sub AddSwaggerDocumentation(services As IServiceCollection)
            services.AddSwaggerGen(Sub(options)
                                       options.SwaggerDoc("v1", New OpenApiInfo With {
                                           .Title = "Personas API",
                                           .Version = "v1",
                                           .Description = "API REST para gestión de personas y direcciones",
                                           .Contact = New OpenApiContact With {
                                               .Name = "John Castro Sanabria",
                                               .Email = "castrosanabriajohn2@gmail.com",
                                               .Url = New Uri("https://github.com/full-stack-dev-johncastrosanabria")
                                           }
                                       })

                                       ' Enable XML comments if available
                                       options.EnableAnnotations()
                                   End Sub)
        End Sub

        <Runtime.CompilerServices.Extension>
        Public Sub AddFluentValidationConfig(services As IServiceCollection)
            services.AddFluentValidationAutoValidation()
            services.AddFluentValidationClientsideAdapters()
            services.AddValidatorsFromAssemblyContaining(Of PersonaCreateUpdateDtoValidator)()
        End Sub

        <Runtime.CompilerServices.Extension>
        Public Sub AddHealthChecksConfig(services As IServiceCollection, connectionString As String)
            services.AddHealthChecks().
                AddSqlServer(connectionString, name:="database", tags:={"db", "sql", "sqlserver"})
        End Sub
    End Module
End Namespace
