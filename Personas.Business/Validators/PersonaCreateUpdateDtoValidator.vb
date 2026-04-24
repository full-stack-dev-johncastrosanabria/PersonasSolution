Imports FluentValidation

Namespace Personas.Business.Validators
    Public Class PersonaCreateUpdateDtoValidator
        Inherits AbstractValidator(Of PersonaCreateUpdateDto)

        Public Sub New()
            RuleFor(Function(x) x.Identificacion).
                NotEmpty().WithMessage("La identificación es requerida.").
                MaximumLength(50).WithMessage("La identificación no puede exceder 50 caracteres.")

            RuleFor(Function(x) x.Nombre).
                NotEmpty().WithMessage("El nombre es requerido.").
                MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres.")

            RuleFor(Function(x) x.Apellidos).
                NotEmpty().WithMessage("Los apellidos son requeridos.").
                MaximumLength(150).WithMessage("Los apellidos no pueden exceder 150 caracteres.")

            RuleFor(Function(x) x.FechaNacimiento).
                NotEmpty().WithMessage("La fecha de nacimiento es requerida.").
                LessThan(Date.Today).WithMessage("La fecha de nacimiento debe ser anterior a hoy.").
                GreaterThan(Date.Today.AddYears(-150)).WithMessage("La fecha de nacimiento no es válida.")

            RuleFor(Function(x) x.Correo).
                NotEmpty().WithMessage("El correo es requerido.").
                EmailAddress().WithMessage("El correo no tiene un formato válido.").
                MaximumLength(150).WithMessage("El correo no puede exceder 150 caracteres.")

            RuleFor(Function(x) x.Telefono).
                MaximumLength(50).WithMessage("El teléfono no puede exceder 50 caracteres.").
                When(Function(x) Not String.IsNullOrWhiteSpace(x.Telefono))
        End Sub
    End Class
End Namespace
