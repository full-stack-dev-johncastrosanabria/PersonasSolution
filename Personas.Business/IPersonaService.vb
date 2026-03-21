Namespace Personas.Business
    Public Interface IPersonaService
        Function GetPagedAsync(search As String, page As Integer, pageSize As Integer) As Task(Of PagedResult(Of PersonaListItemDto))
        Function GetByIdAsync(id As Integer) As Task(Of PersonaDetailDto)
        Function CreateAsync(dto As PersonaCreateUpdateDto) As Task(Of PersonaDetailDto)
        Function UpdateAsync(id As Integer, dto As PersonaCreateUpdateDto) As Task(Of PersonaDetailDto)
        Function SetActivoAsync(id As Integer, activo As Boolean) As Task
        Function AddDireccionAsync(personaId As Integer, dto As DireccionCreateUpdateDto) As Task(Of DireccionDto)
        Function UpdateDireccionAsync(id As Integer, dto As DireccionCreateUpdateDto) As Task(Of DireccionDto)
        Function DeleteDireccionAsync(id As Integer) As Task
    End Interface
End Namespace
