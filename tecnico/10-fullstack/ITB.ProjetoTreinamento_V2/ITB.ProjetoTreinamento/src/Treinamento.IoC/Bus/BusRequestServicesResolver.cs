using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Treinamento.Domain.Core.Bus;

namespace Treinamento.IoC.Bus;

public sealed class BusRequestServicesResolver(
    IHttpContextAccessor httpContextAccessor,
    IServiceScopeFactory scopeFactory) : IBusRequestServicesResolver
{
    public IBusServiceScope CriarEscopoParaDespacho()
    {
        var requestServices = httpContextAccessor.HttpContext?.RequestServices;
        if (requestServices is not null)
            return new EscopoRequisicaoHttpBus(requestServices);

        var owned = scopeFactory.CreateScope();
        return new EscopoProprioBus(owned);
    }

    private sealed class EscopoRequisicaoHttpBus(IServiceProvider services) : IBusServiceScope
    {
        public IServiceProvider Services => services;
        public ValueTask DisposeAsync() => ValueTask.CompletedTask;
    }

    private sealed class EscopoProprioBus(IServiceScope scope) : IBusServiceScope
    {
        public IServiceProvider Services => scope.ServiceProvider;

        public ValueTask DisposeAsync()
        {
            scope.Dispose();
            return ValueTask.CompletedTask;
        }
    }
}
