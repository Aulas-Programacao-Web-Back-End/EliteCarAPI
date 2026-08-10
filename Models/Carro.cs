using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EliteCarAPI.Models;

[Table("carros")]
public class Carro
{
    [Key]
    [Column("id_carro")]
    public int IdCarro { get; set; }

    [Required]
    [MaxLength(80)]
    [Column("marca")]
    public string Marca { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    [Column("modelo")]
    public string Modelo { get; set; } = string.Empty;

    [Required]
    [Column("ano")]
    public int Ano { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("cor")]
    public string Cor { get; set; } = string.Empty;

    [Required]
    [MaxLength(10)]
    [Column("placa")]
    public string Placa { get; set; } = string.Empty;

    [Required]
    [Column("quilometragem")]
    public decimal Quilometragem { get; set; }

    [Required]
    [Column("preco")]
    public decimal Preco { get; set; }

    /// <summary>
    /// Situação comercial do veículo: "Disponível" ou "Vendido" (RN06.2, RN11.5).
    /// Não confundir com o campo Ativo (exclusão lógica).
    /// </summary>
    [Required]
    [MaxLength(20)]
    [Column("status")]
    public string Status { get; set; } = "Disponível";

    [Column("ativo")]
    public bool Ativo { get; set; } = true;

    // Navegação
    public ICollection<PedidoVenda> PedidosVenda { get; set; } = [];
}
