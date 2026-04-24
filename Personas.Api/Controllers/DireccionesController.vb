Imports Microsoft.AspNetCore.Mvc
Imports Personas.Business
Imports Microsoft.AspNetCore.Http
Imports Swashbuckle.AspNetCore.Annotations

Namespace Personas.Api.Controllers
    ''' <summary>
    ''' Controlador para la gestión de direcciones
    ''' </summary>
    <ApiController>
    <Route("api/[controller]")>
    <Produces("application/json")>
    Public Class DireccionesController
        Inherits ControllerBase

        Private ReadOnly _service As IPersonaService

        Public Sub New(service As IPersonaService)
            _service = service
        End Sub

        ''' <summary>
        ''' Actualiza una dirección existente
        ''' </summary>
        ''' <param name="id">ID de la dirección</param>
        ''' <param name="dto">Datos actualizados de la dirección</param>
        ''' <returns>Dirección actualizada</returns>
        <HttpPut("{id}")>
        <SwaggerOperation(Summary:="Actualizar dirección", Description:="Actualiza los datos de una dirección existente")>
        <ProducesResponseType(GetType(DireccionDto), StatusCodes.Status200OK)>
        <ProducesResponseType(StatusCodes.Status404NotFound)>
        <ProducesResponseType(StatusCodes.Status400BadRequest)>
        Public Async Function Update(id As Integer, <FromBody> dto As DireccionCreateUpdateDto) As Task(Of ActionResult(Of DireccionDto))
            Dim result = Await _service.UpdateDireccionAsync(id, dto)
            Return Ok(result)
        End Function

        ''' <summary>
        ''' Elimina una dirección
        ''' </summary>
        ''' <param name="id">ID de la dirección</param>
        ''' <returns>Sin contenido</returns>
        <HttpDelete("{id}")>
        <SwaggerOperation(Summary:="Eliminar dirección", Description:="Elimina una dirección del sistema")>
        <ProducesResponseType(StatusCodes.Status204NoContent)>
        <ProducesResponseType(StatusCodes.Status404NotFound)>
        Public Async Function Delete(id As Integer) As Task(Of IActionResult)
            Await _service.DeleteDireccionAsync(id)
            Return NoContent()
        End Function
    End Class
End Namespace
