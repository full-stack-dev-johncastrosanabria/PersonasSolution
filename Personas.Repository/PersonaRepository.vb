Imports Microsoft.EntityFrameworkCore
Imports Personas.EF

Namespace Personas.Repository
    Public Class PersonaRepository
        Implements IPersonaRepository

        Private ReadOnly _context As AppDbContext

        Public Sub New(context As AppDbContext)
            _context = context
        End Sub

        Public Async Function GetPagedAsync(search As String, page As Integer, pageSize As Integer) As Task(Of PagedData(Of Persona)) Implements IPersonaRepository.GetPagedAsync
            Dim query = _context.Personas.
                Include(Function(p) p.Direcciones).
                AsQueryable()

            If Not String.IsNullOrWhiteSpace(search) Then
                search = search.Trim()
                query = query.Where(Function(p) p.Identificacion.Contains(search) OrElse
                                                p.Nombre.Contains(search) OrElse
                                                p.Apellidos.Contains(search))
            End If

            Dim total = Await query.CountAsync()

            Dim items = Await query.
                OrderByDescending(Function(p) p.FechaRegistro).
                Skip((page - 1) * pageSize).
                Take(pageSize).
                ToListAsync()

            Return New PagedData(Of Persona) With {
                .Items = items,
                .Total = total,
                .Page = page,
                .PageSize = pageSize
            }
        End Function

        Public Async Function GetByIdAsync(id As Integer) As Task(Of Persona) Implements IPersonaRepository.GetByIdAsync
            Return Await _context.Personas.
                Include(Function(p) p.Direcciones).
                FirstOrDefaultAsync(Function(p) p.PersonaId = id)
        End Function

        Public Async Function ExistsIdentificacionAsync(identificacion As String, Optional personaId As Integer? = Nothing) As Task(Of Boolean) Implements IPersonaRepository.ExistsIdentificacionAsync
            Dim query = _context.Personas.Where(Function(p) p.Identificacion = identificacion)

            If personaId.HasValue Then
                query = query.Where(Function(p) p.PersonaId <> personaId.Value)
            End If

            Return Await query.AnyAsync()
        End Function

        Public Async Function AddAsync(persona As Persona) As Task Implements IPersonaRepository.AddAsync
            Await _context.Personas.AddAsync(persona)
        End Function

        Public Function UpdateAsync(persona As Persona) As Task Implements IPersonaRepository.UpdateAsync
            _context.Personas.Update(persona)
            Return Task.CompletedTask
        End Function

        Public Async Function SaveChangesAsync() As Task Implements IPersonaRepository.SaveChangesAsync
            Await _context.SaveChangesAsync()
        End Function

        Public Async Function GetDireccionByIdAsync(id As Integer) As Task(Of Direccion) Implements IPersonaRepository.GetDireccionByIdAsync
            Return Await _context.Direcciones.FirstOrDefaultAsync(Function(d) d.DireccionId = id)
        End Function

        Public Async Function AddDireccionAsync(direccion As Direccion) As Task Implements IPersonaRepository.AddDireccionAsync
            Await _context.Direcciones.AddAsync(direccion)
        End Function

        Public Function DeleteDireccionAsync(direccion As Direccion) As Task Implements IPersonaRepository.DeleteDireccionAsync
            _context.Direcciones.Remove(direccion)
            Return Task.CompletedTask
        End Function

        Public Async Function ClearPrincipalAsync(personaId As Integer, Optional direccionIdExcluir As Integer? = Nothing) As Task Implements IPersonaRepository.ClearPrincipalAsync
            Dim query = _context.Direcciones.Where(Function(d) d.PersonaId = personaId AndAlso d.EsPrincipal)

            If direccionIdExcluir.HasValue Then
                query = query.Where(Function(d) d.DireccionId <> direccionIdExcluir.Value)
            End If

            Dim direcciones = Await query.ToListAsync()

            For Each d In direcciones
                d.EsPrincipal = False
            Next
        End Function
    End Class
End Namespace
