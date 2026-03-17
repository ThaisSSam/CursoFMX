using System;
using System.Data.Common;
using ITB.Domain.Core.Messages.Interfaces;


namespace ITB.Application.Commands;

public class DescontoCommand: ICommand
{
    public string nomeMarca { get; set;} = string.Empty;

    public string Modelo { get; set;} = string.Empty;

    public int Ano { get; set;}

    public decimal PrecoAtualizado { get; set;}

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        throw new NotImplementedException();
    }

    // internal bool EhValido()
    // {
    //     throw new NotImplementedException();
    // }
    public bool EhValido() 
    {
        return !string.IsNullOrWhiteSpace(nomeMarca);
    }

    public void Execute(object? parameter)
    {
        throw new NotImplementedException();
    }
}

