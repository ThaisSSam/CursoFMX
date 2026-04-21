using FluentValidation;
using ITB.Application.Commands;

namespace ITB.Application.Validations;

public class AdicionarMarcaValidation : AbstractValidator<AdicionarMarcaCommand>
{
    public AdicionarMarcaValidation()
    {
        RuleFor(m => m.Nome)
            .NotEmpty().WithMessage("O nome é de preenchimento obrigatório.");
    }
}