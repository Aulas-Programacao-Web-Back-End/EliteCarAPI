using EliteCarAPI.Data;
using EliteCarAPI.DTOs;
using EliteCarAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EliteCarAPI.Services;

public class ClienteService(AppDbContext context)
{
    // ──────────────────────────────────────────────────────────
    // RF02 / RN02.1 — Listar apenas clientes ativos
    // ──────────────────────────────────────────────────────────
    public async Task<List<ClienteResponseDTO>> ListarAtivosAsync()
    {
        return await context.Clientes
            .Where(c => c.Ativo)
            .Select(c => ToResponseDTO(c))
            .ToListAsync();
    }

    // ──────────────────────────────────────────────────────────
    // RF03 / RN02.1 — Buscar cliente ativo por CPF
    // ──────────────────────────────────────────────────────────
    public async Task<ClienteResponseDTO?> BuscarPorCpfAsync(string cpf)
    {
        var cliente = await context.Clientes
            .FirstOrDefaultAsync(c => c.Cpf == cpf && c.Ativo);

        return cliente is null ? null : ToResponseDTO(cliente);
    }

    // ──────────────────────────────────────────────────────────
    // RF01 / RN01.1, RN01.2 — Criar cliente
    // ──────────────────────────────────────────────────────────
    public async Task<(ClienteResponseDTO? Response, string? Erro)> CriarAsync(ClienteCreateDTO dto)
    {
        // RN01.1 — CPF único
        if (await context.Clientes.AnyAsync(c => c.Cpf == dto.Cpf))
            return (null, $"Já existe um cliente cadastrado com o CPF '{dto.Cpf}'.");

        // RN01.2 — E-mail único
        if (await context.Clientes.AnyAsync(c => c.Email == dto.Email))
            return (null, $"Já existe um cliente cadastrado com o e-mail '{dto.Email}'.");

        var cliente = new Cliente
        {
            Nome = dto.Nome,
            Cpf = dto.Cpf,
            Telefone = dto.Telefone,
            Email = dto.Email,
            Cidade = dto.Cidade,
            Uf = dto.Uf.ToUpper(),
            Ativo = true
        };

        context.Clientes.Add(cliente);
        await context.SaveChangesAsync();

        return (ToResponseDTO(cliente), null);
    }

    // ──────────────────────────────────────────────────────────
    // RF04 / RN01.1, RN01.2 — Atualizar cliente
    // ──────────────────────────────────────────────────────────
    public async Task<(ClienteResponseDTO? Response, string? Erro)> AtualizarAsync(int id, ClienteUpdateDTO dto)
    {
        var cliente = await context.Clientes.FindAsync(id);
        if (cliente is null || !cliente.Ativo)
            return (null, "Cliente não encontrado.");

        // RN01.1 — CPF único (exceto o próprio registro)
        if (await context.Clientes.AnyAsync(c => c.Cpf == dto.Cpf && c.IdCliente != id))
            return (null, $"Já existe um cliente cadastrado com o CPF '{dto.Cpf}'.");

        // RN01.2 — E-mail único (exceto o próprio registro)
        if (await context.Clientes.AnyAsync(c => c.Email == dto.Email && c.IdCliente != id))
            return (null, $"Já existe um cliente cadastrado com o e-mail '{dto.Email}'.");

        cliente.Nome = dto.Nome;
        cliente.Cpf = dto.Cpf;
        cliente.Telefone = dto.Telefone;
        cliente.Email = dto.Email;
        cliente.Cidade = dto.Cidade;
        cliente.Uf = dto.Uf.ToUpper();

        await context.SaveChangesAsync();

        return (ToResponseDTO(cliente), null);
    }

    // ──────────────────────────────────────────────────────────
    // RF05 / RN05.1, RN05.2 — Exclusão lógica de cliente
    // ──────────────────────────────────────────────────────────
    public async Task<(bool Sucesso, string? Erro)> ExcluirAsync(int id)
    {
        var cliente = await context.Clientes.FindAsync(id);
        if (cliente is null || !cliente.Ativo)
            return (false, "Cliente não encontrado.");

        // RN05.1 — Remoção lógica: marca como inativo (histórico preservado — RN05.2)
        cliente.Ativo = false;
        await context.SaveChangesAsync();

        return (true, null);
    }

    // ──────────────────────────────────────────────────────────
    // Helper de mapeamento
    // ──────────────────────────────────────────────────────────
    private static ClienteResponseDTO ToResponseDTO(Cliente c) => new()
    {
        IdCliente = c.IdCliente,
        Nome = c.Nome,
        Cpf = c.Cpf,
        Telefone = c.Telefone,
        Email = c.Email,
        Cidade = c.Cidade,
        Uf = c.Uf,
        Ativo = c.Ativo
    };
}
