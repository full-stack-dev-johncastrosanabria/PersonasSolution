Namespace Personas.Business

    Public Class DireccionDto
        Public Property DireccionId As Integer
        Public Property PersonaId As Integer
        Public Property Provincia As String = String.Empty
        Public Property Canton As String = String.Empty
        Public Property Distrito As String = String.Empty
        Public Property DireccionExacta As String = String.Empty
        Public Property EsPrincipal As Boolean
    End Class

    Public Class DireccionCreateUpdateDto
        Public Property Provincia As String = String.Empty
        Public Property Canton As String = String.Empty
        Public Property Distrito As String = String.Empty
        Public Property DireccionExacta As String = String.Empty
        Public Property EsPrincipal As Boolean
    End Class

    Public Class PersonaListItemDto
        Public Property PersonaId As Integer
        Public Property Identificacion As String = String.Empty
        Public Property NombreCompleto As String = String.Empty
        Public Property Correo As String = String.Empty
        Public Property Telefono As String = String.Empty
        Public Property Activo As Boolean
        Public Property FechaRegistro As DateTime
    End Class

    Public Class PersonaCreateUpdateDto
        Public Property Identificacion As String = String.Empty
        Public Property Nombre As String = String.Empty
        Public Property Apellidos As String = String.Empty
        Public Property FechaNacimiento As Date
        Public Property Correo As String = String.Empty
        Public Property Telefono As String = String.Empty
        Public Property Activo As Boolean = True
    End Class

    Public Class PersonaDetailDto
        Public Property PersonaId As Integer
        Public Property Identificacion As String = String.Empty
        Public Property Nombre As String = String.Empty
        Public Property Apellidos As String = String.Empty
        Public Property FechaNacimiento As Date
        Public Property Correo As String = String.Empty
        Public Property Telefono As String = String.Empty
        Public Property Activo As Boolean = True
        Public Property FechaRegistro As DateTime
        Public Property Direcciones As New List(Of DireccionDto)
    End Class

    Public Class PersonaEstadoDto
        Public Property Activo As Boolean
    End Class

    Public Class PagedResult(Of T)
        Public Property Items As New List(Of T)
        Public Property Total As Integer
        Public Property Page As Integer
        Public Property PageSize As Integer
    End Class

End Namespace