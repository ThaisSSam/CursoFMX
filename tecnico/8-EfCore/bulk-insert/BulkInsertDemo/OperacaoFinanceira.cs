
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkInsertDemo;

[Table("operacao_financeira")]
public class OperacaoFinanceira
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }
    [Column("numero_contrato")]
    public string NumeroContrato { get; set; } = string.Empty;
    [Column("valor_total")]
    public decimal ValorTotal { get; set; }
    [Column("data_processamento")]
    public DateTime DataProcessamento { get; set; }
}