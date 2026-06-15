using System;

namespace Treinamento.Domain.Commands;

public record EditarTarefaCommand(
    int Id, 
    string Nome, 
    int Situacao, 
    int Prioridade, 
    int UsuarioId
);