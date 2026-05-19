namespace Treinamento.Domain.Core.Bus;

public interface IBusServiceScope : IAsyncDisposable
{
    IServiceProvider Services { get; }
}

public interface IBusRequestServicesResolver
{
    IBusServiceScope CriarEscopoParaDespacho();
}
