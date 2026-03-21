Imports Microsoft.AspNetCore.Builder
Imports Microsoft.Extensions.DependencyInjection
Imports Microsoft.Extensions.Hosting
Imports Microsoft.Extensions.Configuration
Imports Microsoft.EntityFrameworkCore
Imports Personas.Business
Imports Personas.EF
Imports Personas.Repository

Module Program
    Sub Main(args As String())
        Dim builder = WebApplication.CreateBuilder(args)

        builder.Services.AddControllers()
        builder.Services.AddEndpointsApiExplorer()
        builder.Services.AddSwaggerGen()

        builder.Services.AddCors(Sub(options)
                                     options.AddPolicy("frontend", Sub(policy)
                                                                       policy.WithOrigins("http://localhost:5173", "http://localhost:4173").
                                                                              AllowAnyHeader().
                                                                              AllowAnyMethod()
                                                                   End Sub)
                                 End Sub)

        builder.Services.AddDbContext(Of AppDbContext)(
            Sub(options)
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            End Sub)

        builder.Services.AddScoped(Of IPersonaRepository, PersonaRepository)()
        builder.Services.AddScoped(Of IPersonaService, PersonaService)()

        Dim app = builder.Build()

        If app.Environment.IsDevelopment() Then
            app.UseSwagger()
            app.UseSwaggerUI()
        End If

        app.UseCors("frontend")
        'app.UseHttpsRedirection()
        app.UseAuthorization()
        app.MapControllers()

        app.Run()
    End Sub
End Module