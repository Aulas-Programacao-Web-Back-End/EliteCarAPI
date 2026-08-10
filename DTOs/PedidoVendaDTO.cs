using System.ComponentModel.DataAnnotations;

namespace EliteCarAPI.DTOs;

// ──────────────────────────────────────────────────────────
// RF11 — Cadastro de Venda (entrada)
// ──────────────────────────────────────────────────────────
public class PedidoVendaCreateDTO
{
    [Required(ErrorMessage = "O campo IdCliente é obrigatório.")]
    public int IdCliente { get; set; }

    [Required(ErrorMessage = "O campo IdCarro é obrigatório.")]
    public int IdCarro { get; set; }

    [Required(ErrorMessage = "O campo DataPedido é obrigatório.")]
    public DateOnly DataPedido { get; set; }

    [Required(ErrorMessage = "O campo ValorPedido é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O ValorPedido deve ser maior que zero.")]
    public decimal ValorPedido { get; set; }

    /// <summary>
    /// Valores permitidos: "À Vista", "Financiado", "Consórcio" (RN11.2).
    /// </summary>
    [Required(ErrorMessage = "O campo FormaDePagamento é obrigatório.")]
    [MaxLength(20)]
    public string FormaDePagamento { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Observações deve ter no máximo 500 caracteres.")]
    public string? Observacoes { get; set; }
}

// ──────────────────────────────────────────────────────────
// RF14 — Atualização de Venda (entrada)
// ──────────────────────────────────────────────────────────
public class PedidoVendaUpdateDTO : PedidoVendaCreateDTO { }

// ──────────────────────────────────────────────────────────
// RF12 / RF13 — Resposta de Venda (saída)
// Inclui dados resumidos do cliente e do carro
// ──────────────────────────────────────────────────────────
public class PedidoVendaResponseDTO
{
    public int IdPedido { get; set; }
    public int IdCliente { get; set; }
    public string NomeCliente { get; set; } = string.Empty;
    public int IdCarro { get; set; }
    public string ModeloCarro { get; set; } = string.Empty;
    public string PlacaCarro { get; set; } = string.Empty;
    public DateOnly DataPedido { get; set; }
    public decimal ValorPedido { get; set; }
    public string FormaDePagamento { get; set; } = string.Empty;
    public string? Observacoes { get; set; }
    public bool Ativo { get; set; }
}
