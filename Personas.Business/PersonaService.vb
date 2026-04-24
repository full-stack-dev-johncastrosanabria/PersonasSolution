Imports System.Linq
Imports Personas.EF
Imports Personas.Repository

Namespace Personas.Business

    Public Class PersonaService
        Implements IPersonaService

        Private ReadOnly _repository As IPersonaRepository

        Public Sub New(repository As IPersonaRepository)
            _repository = repository
        End Sub

        Public Async Function GetPagedAsync(search As String, page As Integer, pageSize As Integer) As Task(Of PagedResult(Of PersonaListItemDto)) Implements IPersonaService.GetPagedAsync
            If page <= 0 Then page = 1
            If pageSize <= 0 Then pageSize = 10

            Dim result = Await _repository.GetPagedAsync(search, page, pageSize)

            Return New PagedResult(Of PersonaListItemDto) With {
                .Items = result.Items.Select(Function(p) New PersonaListItemDto With {
                    .PersonaId = p.PersonaId,
                    .Identificacion = p.Identificacion,
                    .NombreCompleto = $"{p.Nombre} {p.Apellidos}",
                    .Correo = p.Correo,
                    .Telefono = p.Telefono,
                    .Activo = p.Activo,
                    .FechaRegistro = p.FechaRegistro
                }).ToList(),
                .Total = result.Total,
                .Page = result.Page,
                .PageSize = result.PageSize
            }
        End Function

        Public Async Function GetByIdAsync(id As Integer) As Task(Of PersonaDetailDto) Implements IPersonaService.GetByIdAsync
            Dim persona = Await _repository.GetByIdAsync(id)

            If persona Is Nothing Then
                Throw New KeyNotFoundException("Persona no encontrada.")
            End If

            Return MapPersonaDetail(persona)
        End Function

        Public Async Function CreateAsync(dto As PersonaCreateUpdateDto) As Task(Of PersonaDetailDto) Implements IPersonaService.CreateAsync
            ValidatePersona(dto)

            If Await _repository.ExistsIdentificacionAsync(dto.Identificacion.Trim()) Then
                Throw New InvalidOperationException("La identificación ya existe.")
            End If

            Dim persona = New Persona With {
                .Identificacion = dto.Identificacion.Trim(),
                .Nombre = dto.Nombre.Trim(),
                .Apellidos = dto.Apellidos.Trim(),
                .FechaNacimiento = dto.FechaNacimiento,
                .Correo = dto.Correo.Trim(),
                .Telefono = dto.Telefono?.Trim(),
                .Activo = dto.Activo,
                .FechaRegistro = DateTime.UtcNow
            }

            Await _repository.AddAsync(persona)
            Await _repository.SaveChangesAsync()

            Dim created = Await _repository.GetByIdAsync(persona.PersonaId)
            Return MapPersonaDetail(created)
        End Function

        Public Async Function UpdateAsync(id As Integer, dto As PersonaCreateUpdateDto) As Task(Of PersonaDetailDto) Implements IPersonaService.UpdateAsync
            ValidatePersona(dto)

            If Await _repository.ExistsIdentificacionAsync(dto.Identificacion.Trim(), id) Then
                Throw New InvalidOperationException("La identificación ya existe.")
            End If

            Dim persona = Await _repository.GetByIdAsync(id)

            If persona Is Nothing Then
                Throw New KeyNotFoundException("Persona no encontrada.")
            End If

            persona.Identificacion = dto.Identificacion.Trim()
            persona.Nombre = dto.Nombre.Trim()
            persona.Apellidos = dto.Apellidos.Trim()
            persona.FechaNacimiento = dto.FechaNacimiento
            persona.Correo = dto.Correo.Trim()
            persona.Telefono = dto.Telefono?.Trim()
            persona.Activo = dto.Activo

            Await _repository.UpdateAsync(persona)
            Await _repository.SaveChangesAsync()

            Dim updated = Await _repository.GetByIdAsync(id)
            Return MapPersonaDetail(updated)
        End Function

        Public Async Function SetActivoAsync(id As Integer, activo As Boolean) As Task Implements IPersonaService.SetActivoAsync
            Dim persona = Await _repository.GetByIdAsync(id)

            If persona Is Nothing Then
                Throw New KeyNotFoundException("Persona no encontrada.")
            End If

            persona.Activo = activo
            Await _repository.UpdateAsync(persona)
            Await _repository.SaveChangesAsync()
        End Function

        Public Async Function AddDireccionAsync(personaId As Integer, dto As DireccionCreateUpdateDto) As Task(Of DireccionDto) Implements IPersonaService.AddDireccionAsync
            ValidateDireccion(dto)

            Dim persona = Await _repository.GetByIdAsync(personaId)
            If persona Is Nothing Then
                Throw New KeyNotFoundException("Persona no encontrada.")
            End If

            If dto.EsPrincipal Then
                Await _repository.ClearPrincipalAsync(personaId)
            ElseIf persona.Direcciones Is Nothing OrElse persona.Direcciones.Count = 0 Then
                dto.EsPrincipal = True
            End If

            Dim direccion = New Direccion With {
                .PersonaId = personaId,
                .Provincia = dto.Provincia.Trim(),
                .Canton = dto.Canton.Trim(),
                .Distrito = dto.Distrito.Trim(),
                .DireccionExacta = dto.DireccionExacta.Trim(),
                .EsPrincipal = dto.EsPrincipal
            }

            Await _repository.AddDireccionAsync(direccion)
            Await _repository.SaveChangesAsync()

            Return New DireccionDto With {
                .DireccionId = direccion.DireccionId,
                .PersonaId = direccion.PersonaId,
                .Provincia = direccion.Provincia,
                .Canton = direccion.Canton,
                .Distrito = direccion.Distrito,
                .DireccionExacta = direccion.DireccionExacta,
                .EsPrincipal = direccion.EsPrincipal
            }
        End Function

        Public Async Function UpdateDireccionAsync(id As Integer, dto As DireccionCreateUpdateDto) As Task(Of DireccionDto) Implements IPersonaService.UpdateDireccionAsync
            ValidateDireccion(dto)

            Dim direccion = Await _repository.GetDireccionByIdAsync(id)
            If direccion Is Nothing Then
                Throw New KeyNotFoundException("Dirección no encontrada.")
            End If

            If dto.EsPrincipal Then
                Await _repository.ClearPrincipalAsync(direccion.PersonaId, id)
            End If

            direccion.Provincia = dto.Provincia.Trim()
            direccion.Canton = dto.Canton.Trim()
            direccion.Distrito = dto.Distrito.Trim()
            direccion.DireccionExacta = dto.DireccionExacta.Trim()
            direccion.EsPrincipal = dto.EsPrincipal

            Await _repository.SaveChangesAsync()

            Return New DireccionDto With {
                .DireccionId = direccion.DireccionId,
                .PersonaId = direccion.PersonaId,
                .Provincia = direccion.Provincia,
                .Canton = direccion.Canton,
                .Distrito = direccion.Distrito,
                .DireccionExacta = direccion.DireccionExacta,
                .EsPrincipal = direccion.EsPrincipal
            }
        End Function

        Public Async Function DeleteDireccionAsync(id As Integer) As Task Implements IPersonaService.DeleteDireccionAsync
            Dim direccion = Await _repository.GetDireccionByIdAsync(id)
            If direccion Is Nothing Then
                Throw New KeyNotFoundException("Dirección no encontrada.")
            End If

            Await _repository.DeleteDireccionAsync(direccion)
            Await _repository.SaveChangesAsync()
        End Function

        Private Sub ValidatePersona(dto As PersonaCreateUpdateDto)
            If String.IsNullOrWhiteSpace(dto.Identificacion) Then Throw New InvalidOperationException("Identificación requerida.")
            If String.IsNullOrWhiteSpace(dto.Nombre) Then Throw New InvalidOperationException("Nombre requerido.")
            If String.IsNullOrWhiteSpace(dto.Apellidos) Then Throw New InvalidOperationException("Apellidos requeridos.")
            If String.IsNullOrWhiteSpace(dto.Correo) Then Throw New InvalidOperationException("Correo requerido.")
            If dto.FechaNacimiento = Date.MinValue Then Throw New InvalidOperationException("Fecha de nacimiento requerida.")
        End Sub

        Private Sub ValidateDireccion(dto As DireccionCreateUpdateDto)
            If String.IsNullOrWhiteSpace(dto.Provincia) Then Throw New InvalidOperationException("Provincia requerida.")
            If String.IsNullOrWhiteSpace(dto.Canton) Then Throw New InvalidOperationException("Canton requerido.")
            If String.IsNullOrWhiteSpace(dto.Distrito) Then Throw New InvalidOperationException("Distrito requerido.")
            If String.IsNullOrWhiteSpace(dto.DireccionExacta) Then Throw New InvalidOperationException("Dirección exacta requerida.")
        End Sub

        Private Function MapPersonaDetail(persona As Persona) As PersonaDetailDto
            Return New PersonaDetailDto With {
                .PersonaId = persona.PersonaId,
                .Identificacion = persona.Identificacion,
                .Nombre = persona.Nombre,
                .Apellidos = persona.Apellidos,
                .FechaNacimiento = persona.FechaNacimiento,
                .Correo = persona.Correo,
                .Telefono = persona.Telefono,
                .Activo = persona.Activo,
                .FechaRegistro = persona.FechaRegistro,
                .Direcciones = persona.Direcciones.Select(Function(d) New DireccionDto With {
                    .DireccionId = d.DireccionId,
                    .PersonaId = d.PersonaId,
                    .Provincia = d.Provincia,
                    .Canton = d.Canton,
                    .Distrito = d.Distrito,
                    .DireccionExacta = d.DireccionExacta,
                    .EsPrincipal = d.EsPrincipal
                }).ToList()
            }
        End Function

    End Class

End Namespace