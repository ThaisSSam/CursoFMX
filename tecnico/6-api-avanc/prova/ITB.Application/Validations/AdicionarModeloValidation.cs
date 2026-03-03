using System;
using FluentValidation;
using ITB.Application.Commands;

namespace ITB.Application.Validations;

public class AdicionarModeloValidation : AbstractValidator<AdicionarModeloCommand>
{
    public AdicionarModeloValidation()
    {
        RuleFor(Validations => Validations.marcaId)
            .GreaterThan(0).WithMessage("Por favor, selecione uma marca de veículo válida.");
        
        RuleFor(Validations => Validations.ativo)
            .NotNull().WithMessage("Ativo é de preenchimento obrigatório");

        RuleFor(Validations => Validations.nome)
            .NotEmpty().WithMessage("Nome é de preenchimento obrigatório");
    }
}
