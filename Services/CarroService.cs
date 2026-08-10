using EliteCarAPI.Data;
using EliteCarAPI.DTOs;
using EliteCarAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EliteCarAPI.Services;

public class CarroService(AppDbContext context)
{
    // ──────────────────────────────────────────────────────────
    // RF07 / RN07.1 — Listar apenas veículos ativos
    // ──────────────────────────────────────────────────────────
    public async Task<List<CarroResponseDTO>> ListarAtivosAsync()
    {
        return await context.Carros
            .Where(c => c.Ativo)
            .Select(c => ToResponseDTO(c))
            .ToListAsync();
    }

    // ──────────────────────────────────────────────────────────
    // RF08 / RN07.1 — Buscar veículo ativo por placa
    // ──────────────────────────────────────────────────────────
    public async Task<CarroResponseDTO?> BuscarPorPlacaAsync(string placa)
    {
        var carro = await context.Carros
            .FirstOrDefaultAsync(c => c.Placa == placa.ToUpper() && c.Ativo);

        return carro is null ? null : ToResponseDTO(carro);
    }

    // ──────────────────────────────────────────────────────────
    // RF06 / RN06.1, RN06.2 — Criar veículo
    // ──────────────────────────────────────────────────────────
    public async Task<(CarroResponseDTO? Response, string? Erro)> CriarAsync(CarroCreateDTO dto)
    {
        // RN06.1 — Placa única
        if (await context.Carros.AnyAsync(c => c.Placa == dto.Placa.ToUpper()))
            return (null, $"Já existe um veículo cadastrado com a placa '{dto.Placa}'.");

        var carro = new Carro
        {
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            Ano = dto.Ano,
            Cor = dto.Cor,
            Placa = dto.Placa.ToUpper(),
            Quilometragem = dto.Quilometragem,
            Preco = dto.Preco,
            // RN06.2 — Status e Ativo definidos pelo sistema
            Status = "Disponível",
            Ativo = true
        };

        context.Carros.Add(carro);
        await context.SaveChangesAsync();

        return (ToResponseDTO(carro), null);
    }

    // ──────────────────────────────────────────────────────────
    // RF09 / RN06.1 — Atualizar veículo
    // Status comercial NÃO é alterado diretamente pelo usuário
    // ──────────────────────────────────────────────────────────
    public async Task<(CarroResponseDTO? Response, string? Erro)> AtualizarAsync(int id, CarroUpdateDTO dto)
    {
        var carro = await context.Carros.FindAsync(id);
        if (carro is null || !carro.Ativo)
            return (null, "Veículo não encontrado.");

        // RN06.1 — Placa única (exceto o próprio registro)
        if (await context.Carros.AnyAsync(c => c.Placa == dto.Placa.ToUpper() && c.IdCarro != id))
            return (null, $"Já existe um veículo cadastrado com a placa '{dto.Placa}'.");

        carro.Marca = dto.Marca;
        carro.Modelo = dto.Modelo;
        carro.Ano = dto.Ano;
        carro.Cor = dto.Cor;
        carro.Placa = dto.Placa.ToUpper();
        carro.Quilometragem = dto.Quilometragem;
        carro.Preco = dto.Preco;
        // Status continua controlado pelas regras de venda (RN11.5 / RN15.2)

        await context.SaveChangesAsync();

        return (ToResponseDTO(carro), null);
    }

    // ──────────────────────────────────────────────────────────
    // RF10 / RN10.1 — Exclusão lógica de veículo
    // ──────────────────────────────────────────────────────────
    public async Task<(bool Sucesso, string? Erro)> ExcluirAsync(int id)
    {
        var carro = await context.Carros.FindAsync(id);
        if (carro is null || !carro.Ativo)
            return (false, "Veículo não encontrado.");

        // RN10.1 — Remoção lógica
        carro.Ativo = false;
        await context.SaveChangesAsync();

        return (true, null);
    }

    // ──────────────────────────────────────────────────────────
    // Helper de mapeamento
    // ──────────────────────────────────────────────────────────
    private static CarroResponseDTO ToResponseDTO(Carro c) => new()
    {
        IdCarro = c.IdCarro,
        Marca = c.Marca,
        Modelo = c.Modelo,
        Ano = c.Ano,
        Cor = c.Cor,
        Placa = c.Placa,
        Quilometragem = c.Quilometragem,
        Preco = c.Preco,
        Status = c.Status,
        Ativo = c.Ativo
    };
}
