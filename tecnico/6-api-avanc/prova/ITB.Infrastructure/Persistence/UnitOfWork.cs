using System;
using System.Threading.Tasks;
using ITB.Domain.Interfaces;

namespace ITB.Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        // Implementação das propriedades da Interface
        public IMarcaRepository marcas { get; }
        public IVeiculoRepository veiculos { get; }

        public UnitOfWork(AppDbContext context, IMarcaRepository marcaRepository, IVeiculoRepository veiculoRepository)
        {
            _context = context;
            marcas = marcaRepository; 
            veiculos = veiculoRepository; // Agora o UoW conhece o repositório de veículos
        }

        public async Task<bool> CommitAsync()
        {
            // Salva as alterações de todas as tabelas no banco de uma vez
            return await _context.SaveChangesAsync() > 0;
        }
    }
}