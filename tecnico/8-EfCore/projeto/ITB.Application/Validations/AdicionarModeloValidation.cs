using FluentValidation;
using ITB.Application.Commands;

namespace ITB.Application.Validations;

public class AdicionarModeloValidation : AbstractValidator<AdicionarModeloCommand>
{
    public AdicionarModeloValidation()
    {
        RuleFor(m => m.Nome)
            .NotEmpty().WithMessage("O nome é de preenchimento obrigatório.");
        RuleFor(v => v.MarcaId)
            .GreaterThan(0).WithMessage("Por favor, selecione uma marca de veículo válida.");
    }
}