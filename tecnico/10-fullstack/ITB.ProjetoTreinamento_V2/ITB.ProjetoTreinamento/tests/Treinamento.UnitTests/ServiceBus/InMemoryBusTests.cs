using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Treinamento.Domain.Aggregates.Sistema.Commands;
using Treinamento.Domain.Core.Bus;
using Treinamento.Domain.Core.Events;
using Treinamento.Domain.Core.Interfaces;
using Treinamento.Domain.Core.Notifications;
using Treinamento.IoC.Bus;
using Treinamento.ServiceBus;

namespace Treinamento.UnitTests.ServiceBus;

public class InMemoryBusTests
{
    [Fact]
    public async Task SenderCommand_DeveExecutarPingCommandHandler()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddScoped<IDomainNotificationHandler<DomainNotification>, DomainNotificationHandler>();
        services.AddScoped<IHandler<PingCommand>, PingCommandHandler>();
        services.AddScoped<IUnitOfWork>(_ => Mock.Of<IUnitOfWork>());
        services.AddSingleton<IBusRequestServicesResolver, BusRequestServicesResolver>();
        services.AddHttpContextAccessor();
        services.AddScoped<IBus, InMemoryBus>();

        await using var provider = services.BuildServiceProvider();
        var bus = provider.GetRequiredService<IBus>();

        var command = new PingCommand { Mensagem = "teste" };
        await bus.SenderCommand(command);

        command.Resposta.Should().Be("pong: teste");
    }
}
