using System;
using FluentValidation;
using ITB.Application.Commands;

namespace ITB.Application.Validations;

public class AdicionarVeiculoValidation : AbstractValidator<AdicionarVeiculoCommand>
{
    public AdicionarVeiculoValidation()
    {
        RuleFor(Validations => Validations.placa)
            .NotEmpty().WithMessage("A placa é de preenchimento obrigatório")
            .Length(7).WithMessage("A placa deve possuir exatamente 7 caracteres.")
            .Matches(@"^[A-Z]{3}[0-9][A-Z0-9][0-9]{2}$")
            .WithMessage("A placa deve seguir o padrão Mercosul (Ex: ABC1D23).");

        RuleFor(v => v.ano)
            .InclusiveBetween(1900, DateTime.Now.Year + 1)
            .WithMessage("O ano informado é inválido.");

        RuleFor(v => v.modeloId)
            .GreaterThan(0).WithMessage("Por favor, selecione um modelo de veículo válido.");        
    }
}
