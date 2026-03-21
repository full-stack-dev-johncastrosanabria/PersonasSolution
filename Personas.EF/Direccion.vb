Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Personas.EF
    <Table("direccion")>
    Public Class Direccion
        <Key>
        <Column("direccion_id")>
        Public Property DireccionId As Integer

        <Column("persona_id")>
        Public Property PersonaId As Integer

        <Required>
        <MaxLength(100)>
        <Column("provincia")>
        Public Property Provincia As String = String.Empty

        <Required>
        <MaxLength(100)>
        <Column("canton")>
        Public Property Canton As String = String.Empty

        <Required>
        <MaxLength(100)>
        <Column("distrito")>
        Public Property Distrito As String = String.Empty

        <Required>
        <MaxLength(250)>
        <Column("direccion_exacta")>
        Public Property DireccionExacta As String = String.Empty

        <Column("es_principal")>
        Public Property EsPrincipal As Boolean

        Public Property Persona As Persona
    End Class
End Namespace
