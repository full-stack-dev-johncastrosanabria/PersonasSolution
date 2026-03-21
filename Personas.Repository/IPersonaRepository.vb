Imports Personas.EF

Namespace Personas.Repository
    Public Interface IPersonaRepository
        Function GetPagedAsync(search As String, page As Integer, pageSize As Integer) As Task(Of PagedData(Of Persona))
        Function GetByIdAsync(id As Integer) As Task(Of Persona)
        Function ExistsIdentificacionAsync(identificacion As String, Optional personaId As Integer? = Nothing) As Task(Of Boolean)
        Function AddAsync(persona As Persona) As Task
        Function UpdateAsync(persona As Persona) As Task
        Function SaveChangesAsync() As Task
        Function GetDireccionByIdAsync(id As Integer) As Task(Of Direccion)
        Function AddDireccionAsync(direccion As Direccion) As Task
        Function DeleteDireccionAsync(direccion As Direccion) As Task
        Function ClearPrincipalAsync(personaId As Integer, Optional direccionIdExcluir As Integer? = Nothing) As Task
    End Interface
End Namespace
