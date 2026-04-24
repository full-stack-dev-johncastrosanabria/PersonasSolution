Imports Microsoft.AspNetCore.Mvc
Imports Personas.Business
Imports Microsoft.AspNetCore.Http
Imports Swashbuckle.AspNetCore.Annotations

Namespace Personas.Api.Controllers
    ''' <summary>
    ''' Controlador para la gestión de personas
    ''' </summary>
    <ApiController>
    <Route("api/[controller]")>
    <Produces("application/json")>
    Public Class PersonasController
        Inherits ControllerBase

        Private ReadOnly _service As IPersonaService

        Public Sub New(service As IPersonaService)
            _service = service
        End Sub

        ''' <summary>
        ''' Obtiene una lista paginada de personas
        ''' </summary>
        ''' <param name="search">Término de búsqueda (opcional)</param>
        ''' <param name="page">Número de página (por defecto: 1)</param>
        ''' <param name="pageSize">Tamaño de página (por defecto: 10)</param>
        ''' <returns>Lista paginada de personas</returns>
        <HttpGet>
        <SwaggerOperation(Summary:="Obtener personas paginadas", Description:="Retorna una lista paginada de personas con opción de búsqueda")>
        <ProducesResponseType(GetType(PagedResult(Of PersonaListItemDto)), StatusCodes.Status200OK)>
        <ProducesResponseType(StatusCodes.Status400BadRequest)>
        Public Async Function GetPaged(<FromQuery> Optional search As String = "",
                               <FromQuery> Optional page As Integer = 1,
                               <FromQuery> Optional pageSize As Integer = 10) As Task(Of ActionResult(Of PagedResult(Of PersonaListItemDto)))

            If page < 1 Then page = 1
            If pageSize < 1 Then pageSize = 10
            If pageSize > 100 Then pageSize = 100

            Dim result = Await _service.GetPagedAsync(search, page, pageSize)
            Return Ok(result)

        End Function

        ''' <summary>
        ''' Obtiene una persona por su ID
        ''' </summary>
        ''' <param name="id">ID de la persona</param>
        ''' <returns>Detalle de la persona</returns>
        <HttpGet("{id}")>
        <SwaggerOperation(Summary:="Obtener persona por ID", Description:="Retorna el detalle completo de una persona incluyendo sus direcciones")>
        <ProducesResponseType(GetType(PersonaDetailDto), StatusCodes.Status200OK)>
        <ProducesResponseType(StatusCodes.Status404NotFound)>
        Public Async Function GetById(id As Integer) As Task(Of ActionResult(Of PersonaDetailDto))
            Dim result = Await _service.GetByIdAsync(id)
            Return Ok(result)
        End Function

        ''' <summary>
        ''' Crea una nueva persona
        ''' </summary>
        ''' <param name="dto">Datos de la persona a crear</param>
        ''' <returns>Persona creada</returns>
        <HttpPost>
        <SwaggerOperation(Summary:="Crear persona", Description:="Crea una nueva persona en el sistema")>
        <ProducesResponseType(GetType(PersonaDetailDto), StatusCodes.Status201Created)>
        <ProducesResponseType(StatusCodes.Status400BadRequest)>
        Public Async Function Create(<FromBody> dto As PersonaCreateUpdateDto) As Task(Of ActionResult(Of PersonaDetailDto))
            Dim created = Await _service.CreateAsync(dto)
            Return CreatedAtAction(NameOf(GetById), New With {.id = created.PersonaId}, created)
        End Function

        ''' <summary>
        ''' Actualiza una persona existente
        ''' </summary>
        ''' <param name="id">ID de la persona</param>
        ''' <param name="dto">Datos actualizados de la persona</param>
        ''' <returns>Persona actualizada</returns>
        <HttpPut("{id}")>
        <SwaggerOperation(Summary:="Actualizar persona", Description:="Actualiza los datos de una persona existente")>
        <ProducesResponseType(GetType(PersonaDetailDto), StatusCodes.Status200OK)>
        <ProducesResponseType(StatusCodes.Status404NotFound)>
        <ProducesResponseType(StatusCodes.Status400BadRequest)>
        Public Async Function Update(id As Integer, <FromBody> dto As PersonaCreateUpdateDto) As Task(Of ActionResult(Of PersonaDetailDto))
            Dim result = Await _service.UpdateAsync(id, dto)
            Return Ok(result)
        End Function

        ''' <summary>
        ''' Activa o desactiva una persona
        ''' </summary>
        ''' <param name="id">ID de la persona</param>
        ''' <param name="dto">Estado activo</param>
        ''' <returns>Sin contenido</returns>
        <HttpPatch("{id}/activo")>
        <SwaggerOperation(Summary:="Cambiar estado de persona", Description:="Activa o desactiva una persona")>
        <ProducesResponseType(StatusCodes.Status204NoContent)>
        <ProducesResponseType(StatusCodes.Status404NotFound)>
        Public Async Function SetActivo(id As Integer, <FromBody> dto As PersonaEstadoDto) As Task(Of IActionResult)
            Await _service.SetActivoAsync(id, dto.Activo)
            Return NoContent()
        End Function

        ''' <summary>
        ''' Agrega una dirección a una persona
        ''' </summary>
        ''' <param name="id">ID de la persona</param>
        ''' <param name="dto">Datos de la dirección</param>
        ''' <returns>Dirección creada</returns>
        <HttpPost("{id}/direcciones")>
        <SwaggerOperation(Summary:="Agregar dirección", Description:="Agrega una nueva dirección a una persona")>
        <ProducesResponseType(GetType(DireccionDto), StatusCodes.Status200OK)>
        <ProducesResponseType(StatusCodes.Status404NotFound)>
        <ProducesResponseType(StatusCodes.Status400BadRequest)>
        Public Async Function AddDireccion(id As Integer, <FromBody> dto As DireccionCreateUpdateDto) As Task(Of ActionResult(Of DireccionDto))
            Dim result = Await _service.AddDireccionAsync(id, dto)
            Return Ok(result)
        End Function
    End Class
End Namespace
