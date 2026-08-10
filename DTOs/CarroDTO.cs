using System.ComponentModel.DataAnnotations;

namespace EliteCarAPI.DTOs;

// ──────────────────────────────────────────────────────────
// RF06 — Cadastro de Carro (entrada)
// Usuário NÃO informa status nem ativo (RN06.2)
// ──────────────────────────────────────────────────────────
public class CarroCreateDTO
{
    [Required(ErrorMessage = "O campo Marca é obrigatório.")]
    [MaxLength(80, ErrorMessage = "A Marca deve ter no máximo 80 caracteres.")]
    public string Marca { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Modelo é obrigatório.")]
    [MaxLength(100, ErrorMessage = "O Modelo deve ter no máximo 100 caracteres.")]
    public string Modelo { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Ano é obrigatório.")]
    [Range(1886, 9999, ErrorMessage = "O Ano informado é inválido.")]
    public int Ano { get; set; }

    [Required(ErrorMessage = "O campo Cor é obrigatório.")]
    [MaxLength(50, ErrorMessage = "A Cor deve ter no máximo 50 caracteres.")]
    public string Cor { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Placa é obrigatório.")]
    [MaxLength(10, ErrorMessage = "A Placa deve ter no máximo 10 caracteres.")]
    public string Placa { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Quilometragem é obrigatório.")]
    [Range(0, double.MaxValue, ErrorMessage = "A Quilometragem não pode ser negativa.")]
    public decimal Quilometragem { get; set; }

    [Required(ErrorMessage = "O campo Preço é obrigatório.")]
    [Range(0.01, double.MaxValue, ErrorMessage = "O Preço deve ser maior que zero.")]
    public decimal Preco { get; set; }
}

// ──────────────────────────────────────────────────────────
// RF09 — Atualização de Carro (entrada)
// ──────────────────────────────────────────────────────────
public class CarroUpdateDTO : CarroCreateDTO { }

// ──────────────────────────────────────────────────────────
// RF07 / RF08 — Resposta de Carro (saída)
// ──────────────────────────────────────────────────────────
public class CarroResponseDTO
{
    public int IdCarro { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
    public string Cor { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public decimal Quilometragem { get; set; }
    public decimal Preco { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}
