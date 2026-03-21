Imports Microsoft.AspNetCore.Mvc
Imports Personas.Business

Namespace Personas.Api.Controllers
    <ApiController>
    <Route("api/personas")>
    Public Class PersonasController
        Inherits ControllerBase

        Private ReadOnly _service As IPersonaService

        Public Sub New(service As IPersonaService)
            _service = service
        End Sub

        <HttpGet>
        Public Async Function GetPaged(<FromQuery> Optional search As String = "",
                               <FromQuery> Optional page As Integer = 1,
                               <FromQuery> Optional pageSize As Integer = 10) As Task(Of ActionResult(Of PagedResult(Of PersonaListItemDto)))

            Dim result = Await _service.GetPagedAsync(search, page, pageSize)
            Return Ok(result)

        End Function

        <HttpGet("{id}")>
        Public Async Function GetById(id As Integer) As Task(Of ActionResult(Of PersonaDetailDto))
            Try
                Return Ok(Await _service.GetByIdAsync(id))
            Catch ex As KeyNotFoundException
                Return NotFound(New With {.message = ex.Message})
            End Try
        End Function

        <HttpPost>
        Public Async Function Create(<FromBody> dto As PersonaCreateUpdateDto) As Task(Of ActionResult(Of PersonaDetailDto))
            Try
                Dim created = Await _service.CreateAsync(dto)
                Return CreatedAtAction(NameOf(GetById), New With {.id = created.PersonaId}, created)
            Catch ex As Exception
                Return BadRequest(New With {.message = ex.Message})
            End Try
        End Function

        <HttpPut("{id}")>
        Public Async Function Update(id As Integer, <FromBody> dto As PersonaCreateUpdateDto) As Task(Of ActionResult(Of PersonaDetailDto))
            Try
                Return Ok(Await _service.UpdateAsync(id, dto))
            Catch ex As KeyNotFoundException
                Return NotFound(New With {.message = ex.Message})
            Catch ex As Exception
                Return BadRequest(New With {.message = ex.Message})
            End Try
        End Function

        <HttpPatch("{id}/activo")>
        Public Async Function SetActivo(id As Integer, <FromBody> dto As PersonaEstadoDto) As Task(Of IActionResult)
            Try
                Await _service.SetActivoAsync(id, dto.Activo)
                Return NoContent()
            Catch ex As KeyNotFoundException
                Return NotFound(New With {.message = ex.Message})
            End Try
        End Function

        <HttpPost("{id}/direcciones")>
        Public Async Function AddDireccion(id As Integer, <FromBody> dto As DireccionCreateUpdateDto) As Task(Of ActionResult(Of DireccionDto))
            Try
                Return Ok(Await _service.AddDireccionAsync(id, dto))
            Catch ex As KeyNotFoundException
                Return NotFound(New With {.message = ex.Message})
            Catch ex As Exception
                Return BadRequest(New With {.message = ex.Message})
            End Try
        End Function
    End Class
End Namespace
