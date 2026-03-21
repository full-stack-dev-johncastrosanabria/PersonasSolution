Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Personas.EF
    <Table("persona")>
    Public Class Persona
        <Key>
        <Column("persona_id")>
        Public Property PersonaId As Integer

        <Required>
        <MaxLength(50)>
        <Column("identificacion")>
        Public Property Identificacion As String = String.Empty

        <Required>
        <MaxLength(100)>
        <Column("nombre")>
        Public Property Nombre As String = String.Empty

        <Required>
        <MaxLength(150)>
        <Column("apellidos")>
        Public Property Apellidos As String = String.Empty

        <Column("fecha_nacimiento")>
        Public Property FechaNacimiento As Date

        <Required>
        <MaxLength(150)>
        <Column("correo")>
        Public Property Correo As String = String.Empty

        <MaxLength(50)>
        <Column("telefono")>
        Public Property Telefono As String = String.Empty

        <Column("activo")>
        Public Property Activo As Boolean = True

        <Column("fecha_registro")>
        Public Property FechaRegistro As DateTime = DateTime.UtcNow

        Public Property Direcciones As New List(Of Direccion)
    End Class
End Namespace
