using System.ComponentModel.DataAnnotations;

namespace EliteCarAPI.DTOs;

// ──────────────────────────────────────────────────────────
// RF01 — Cadastro de Cliente (entrada)
// ──────────────────────────────────────────────────────────
public class ClienteCreateDTO
{
    [Required(ErrorMessage = "O campo Nome é obrigatório.")]
    [MaxLength(150, ErrorMessage = "O Nome deve ter no máximo 150 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo CPF é obrigatório.")]
    [MaxLength(14, ErrorMessage = "O CPF deve ter no máximo 14 caracteres.")]
    public string Cpf { get; set; } = string.Empty;

    [MaxLength(20, ErrorMessage = "O Telefone deve ter no máximo 20 caracteres.")]
    public string? Telefone { get; set; }

    [Required(ErrorMessage = "O campo E-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "Formato de e-mail inválido.")]
    [MaxLength(150, ErrorMessage = "O E-mail deve ter no máximo 150 caracteres.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Cidade é obrigatório.")]
    [MaxLength(100, ErrorMessage = "A Cidade deve ter no máximo 100 caracteres.")]
    public string Cidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo UF é obrigatório.")]
    [MaxLength(2, ErrorMessage = "A UF deve ter exatamente 2 caracteres.")]
    [MinLength(2, ErrorMessage = "A UF deve ter exatamente 2 caracteres.")]
    public string Uf { get; set; } = string.Empty;
}

// ──────────────────────────────────────────────────────────
// RF04 — Atualização de Cliente (entrada)
// ──────────────────────────────────────────────────────────
public class ClienteUpdateDTO : ClienteCreateDTO { }

// ──────────────────────────────────────────────────────────
// RF02 / RF03 — Resposta de Cliente (saída)
// ──────────────────────────────────────────────────────────
public class ClienteResponseDTO
{
    public int IdCliente { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Uf { get; set; } = string.Empty;
    public bool Ativo { get; set; }
}
