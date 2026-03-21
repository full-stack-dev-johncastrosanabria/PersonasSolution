Imports Microsoft.AspNetCore.Mvc
Imports Personas.Business

Namespace Personas.Api.Controllers
    <ApiController>
    <Route("api/direcciones")>
    Public Class DireccionesController
        Inherits ControllerBase

        Private ReadOnly _service As IPersonaService

        Public Sub New(service As IPersonaService)
            _service = service
        End Sub

        <HttpPut("{id}")>
        Public Async Function Update(id As Integer, <FromBody> dto As DireccionCreateUpdateDto) As Task(Of ActionResult(Of DireccionDto))
            Try
                Return Ok(Await _service.UpdateDireccionAsync(id, dto))
            Catch ex As KeyNotFoundException
                Return NotFound(New With {.message = ex.Message})
            Catch ex As Exception
                Return BadRequest(New With {.message = ex.Message})
            End Try
        End Function

        <HttpDelete("{id}")>
        Public Async Function Delete(id As Integer) As Task(Of IActionResult)
            Try
                Await _service.DeleteDireccionAsync(id)
                Return NoContent()
            Catch ex As KeyNotFoundException
                Return NotFound(New With {.message = ex.Message})
            End Try
        End Function
    End Class
End Namespace
