using System;
using ITB.Domain.Core.Messages.Interfaces;

namespace ITB.Domain.Core.Notifications;

/// <summary>
/// Representa um erro de negócio específico.
/// Utilizamos os Primary Constructors do C# 12 para simplificar o código.
/// </summary>
public class DomainNotification(string key, string value) : Event // Event representa algo que já aconteceu
{
    // Gera um identificador único para cada notificação criada
    public Guid DomainNotificationId { get; private set; } = Guid.NewGuid();

    // A chave representa ONDE o erro ocorreu (ex: "MarcaId", "Placa", "BancoDeDados")
    public string Key { get; } = key;

    // O valor é a mensagem amigável que o usuário final vai ler na tela
    public string Value { get; } = value;
}