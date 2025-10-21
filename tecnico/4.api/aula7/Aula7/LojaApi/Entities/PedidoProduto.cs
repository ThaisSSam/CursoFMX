using System;

namespace LojaApi.Entities;

public class PedidoProduto
{
    public int PedidoId { get; set; } // Chave Estrangeira para Pedido

    public int ProdutoId { get; set; } // Chave Estrangeira para Produto

    public int Quantidade { get; set; }

    // Propriedades de Navegação
    public Pedido? Pedido { get; set; }
    public Produto? Produto { get; set; }
}
