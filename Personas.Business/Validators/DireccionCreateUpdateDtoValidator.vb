Imports FluentValidation

Namespace Personas.Business.Validators
    Public Class DireccionCreateUpdateDtoValidator
        Inherits AbstractValidator(Of DireccionCreateUpdateDto)

        Public Sub New()
            RuleFor(Function(x) x.Provincia).
                NotEmpty().WithMessage("La provincia es requerida.").
                MaximumLength(100).WithMessage("La provincia no puede exceder 100 caracteres.")

            RuleFor(Function(x) x.Canton).
                NotEmpty().WithMessage("El cantón es requerido.").
                MaximumLength(100).WithMessage("El cantón no puede exceder 100 caracteres.")

            RuleFor(Function(x) x.Distrito).
                NotEmpty().WithMessage("El distrito es requerido.").
                MaximumLength(100).WithMessage("El distrito no puede exceder 100 caracteres.")

            RuleFor(Function(x) x.DireccionExacta).
                NotEmpty().WithMessage("La dirección exacta es requerida.").
                MaximumLength(250).WithMessage("La dirección exacta no puede exceder 250 caracteres.")
        End Sub
    End Class
End Namespace
