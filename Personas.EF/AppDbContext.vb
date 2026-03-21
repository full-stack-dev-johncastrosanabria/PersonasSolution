Imports Microsoft.EntityFrameworkCore

Namespace Personas.EF
    Public Class AppDbContext
        Inherits DbContext

        Public Sub New(options As DbContextOptions(Of AppDbContext))
            MyBase.New(options)
        End Sub

        Public Property Personas As DbSet(Of Persona)
        Public Property Direcciones As DbSet(Of Direccion)

        Protected Overrides Sub OnModelCreating(modelBuilder As ModelBuilder)
            modelBuilder.Entity(Of Persona)().
                HasIndex(Function(p) p.Identificacion).
                IsUnique()

            modelBuilder.Entity(Of Direccion)().
                HasOne(Function(d) d.Persona).
                WithMany(Function(p) p.Direcciones).
                HasForeignKey(Function(d) d.PersonaId)

            modelBuilder.Entity(Of Direccion)().
                HasIndex(Function(d) d.PersonaId).
                HasDatabaseName("IX_direccion_persona_id")

            modelBuilder.Entity(Of Direccion)().
                HasIndex(Function(d) d.PersonaId).
                HasFilter("[es_principal] = 1").
                IsUnique().
                HasDatabaseName("UX_direccion_principal_por_persona")
        End Sub
    End Class
End Namespace
