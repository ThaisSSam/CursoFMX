using System;

namespace abstracao.Interfaces;

public interface IImprimivel
{
    void Imprimir();

    string ObterConteudoImpressao{ get; }
}