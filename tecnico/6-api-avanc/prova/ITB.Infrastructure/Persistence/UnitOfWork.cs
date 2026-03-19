using System;
using System.Threading.Tasks;
using ITB.Domain.Interfaces;


namespace ITB.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        // Adicione as propriedades exigidas pela interface
        public IMarcaRepository marcas { get; }

        IVeiculoRepository IUnitOfWork.veiculos => throw new NotImplementedException();

        IMarcaRepository IUnitOfWork.marcas => throw new NotImplementedException();

        // Se a interface pedir veiculos, adicione aqui também:
        // public IVeiculoRepository veiculos { get; }

        public UnitOfWork(AppDbContext context, IMarcaRepository marcaRepository)
        {
            _context = context;
            marcas = marcaRepository; 
        }

        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        Task<bool> IUnitOfWork.CommitAsync()
        {
            throw new NotImplementedException();
        }
    }
}