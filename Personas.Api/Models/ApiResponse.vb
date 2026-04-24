Namespace Personas.Api.Models
    Public Class ApiResponse(Of T)
        Public Property Success As Boolean
        Public Property Message As String
        Public Property Data As T
        Public Property Errors As List(Of String)

        Public Sub New()
            Errors = New List(Of String)
        End Sub

        Public Shared Function SuccessResponse(data As T, Optional message As String = "Operación exitosa") As ApiResponse(Of T)
            Return New ApiResponse(Of T) With {
                .Success = True,
                .Message = message,
                .Data = data
            }
        End Function

        Public Shared Function ErrorResponse(message As String, Optional errors As List(Of String) = Nothing) As ApiResponse(Of T)
            Return New ApiResponse(Of T) With {
                .Success = False,
                .Message = message,
                .Errors = If(errors, New List(Of String))
            }
        End Function
    End Class
End Namespace
