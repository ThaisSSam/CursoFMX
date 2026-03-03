using System;
using FluentValidation;
using ITB.Application.Commands;

namespace ITB.Application.Validations;

public class AdicionarMarcaValidation : AbstractValidator<AdicionarMarcaCommand>
{
    public AdicionarMarcaValidation()
    {
        RuleFor(Validations => Validations.nome)
            .NotEmpty().WithMessage("Nome é de preenchimento obrigatório");
    }
}
