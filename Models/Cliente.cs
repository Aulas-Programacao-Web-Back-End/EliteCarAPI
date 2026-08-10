using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EliteCarAPI.Models;

[Table("clientes")]
public class Cliente
{
    [Key]
    [Column("id_cliente")]
    public int IdCliente { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(14)]
    [Column("cpf")]
    public string Cpf { get; set; } = string.Empty;

    [MaxLength(20)]
    [Column("telefone")]
    public string? Telefone { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("cidade")]
    public string Cidade { get; set; } = string.Empty;

    [Required]
    [MaxLength(2)]
    [Column("uf")]
    public string Uf { get; set; } = string.Empty;

    [Column("ativo")]
    public bool Ativo { get; set; } = true;

    // Navegação
    public ICollection<PedidoVenda> PedidosVenda { get; set; } = [];
}
