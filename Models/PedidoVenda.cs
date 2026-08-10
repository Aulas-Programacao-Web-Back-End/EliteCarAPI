using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EliteCarAPI.Models;

[Table("pedidos_venda")]
public class PedidoVenda
{
    [Key]
    [Column("id_pedido")]
    public int IdPedido { get; set; }

    [Required]
    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [Required]
    [Column("id_carro")]
    public int IdCarro { get; set; }

    [Required]
    [Column("data_pedido")]
    public DateOnly DataPedido { get; set; }

    [Required]
    [Column("valor_pedido")]
    public decimal ValorPedido { get; set; }

    /// <summary>
    /// Valores permitidos: "À Vista", "Financiado", "Consórcio" (RN11.2).
    /// </summary>
    [Required]
    [MaxLength(20)]
    [Column("forma_de_pagamento")]
    public string FormaDePagamento { get; set; } = string.Empty;

    [MaxLength(500)]
    [Column("observacoes")]
    public string? Observacoes { get; set; }

    [Column("ativo")]
    public bool Ativo { get; set; } = true;

    // Navegação
    [ForeignKey(nameof(IdCliente))]
    public Cliente? Cliente { get; set; }

    [ForeignKey(nameof(IdCarro))]
    public Carro? Carro { get; set; }
}
