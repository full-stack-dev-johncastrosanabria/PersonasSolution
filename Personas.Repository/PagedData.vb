Namespace Personas.Repository
    Public Class PagedData(Of T)
        Public Property Items As New List(Of T)
        Public Property Total As Integer
        Public Property Page As Integer
        Public Property PageSize As Integer
    End Class
End Namespace
