using System;
using LojaApi.Entities;
using LojaApi.Infra.Context;
using LojaApi.Infra.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LojaApi.Infra.Repositories;

public class PedidoDBRepository : IPedidoRepository
{
    private readonly LojaContext _context;
    public PedidoDBRepository(LojaContext context) { _context = context; }

    public Pedido Adicionar(Pedido novoPedido)
    {
        _context.Pedidos.Add(novoPedido);
        _context.SaveChanges();
        return novoPedido;
    }

    public Pedido? ObterPorId(int id)
    {
        // Include(JOIN) em múltiplos níveis: Pedido -> Cliente e Pedido -> PedidoProdutos -> Produto
        return _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.PedidoProdutos) // Inclui a entidade de junção
                .ThenInclude(pp => pp.Produto) // A partir da junção, inclui o Produto (produtos que têm no pedido)
            .FirstOrDefault(p => p.Id == id);
    }
    
    public List<Pedido> ObterTodos()
    {
        return _context.Pedidos
            .Include(p => p.Cliente)
            .Include(p => p.PedidoProdutos)
                .ThenInclude(pp => pp.Produto)
            .ToList();
    }
}
