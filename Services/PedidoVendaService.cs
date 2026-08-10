using EliteCarAPI.Data;
using EliteCarAPI.DTOs;
using EliteCarAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EliteCarAPI.Services;

public class PedidoVendaService(AppDbContext context)
{
    private static readonly string[] FormasDePagamentoPermitidas = ["À Vista", "Financiado", "Consórcio"];

    // ──────────────────────────────────────────────────────────
    // RF12 — Listar todas as vendas ativas
    // ──────────────────────────────────────────────────────────
    public async Task<List<PedidoVendaResponseDTO>> ListarAtivosAsync()
    {
        return await context.PedidosVenda
            .Where(p => p.Ativo)
            .Include(p => p.Cliente)
            .Include(p => p.Carro)
            .Select(p => ToResponseDTO(p))
            .ToListAsync();
    }

    // ──────────────────────────────────────────────────────────
    // RF13 — Consultar vendas por data
    // ──────────────────────────────────────────────────────────
    public async Task<List<PedidoVendaResponseDTO>> BuscarPorDataAsync(DateOnly data)
    {
        return await context.PedidosVenda
            .Where(p => p.Ativo && p.DataPedido == data)
            .Include(p => p.Cliente)
            .Include(p => p.Carro)
            .Select(p => ToResponseDTO(p))
            .ToListAsync();
    }

    // ──────────────────────────────────────────────────────────
    // RF11 / RN11.1–RN11.6 — Criar venda
    // ──────────────────────────────────────────────────────────
    public async Task<(PedidoVendaResponseDTO? Response, string? Erro)> CriarAsync(PedidoVendaCreateDTO dto)
    {
        // RN11.2 — Forma de pagamento válida
        if (!FormasDePagamentoPermitidas.Contains(dto.FormaDePagamento))
            return (null, $"Forma de pagamento inválida. Valores permitidos: {string.Join(", ", FormasDePagamentoPermitidas)}.");

        // RN11.1 — Cliente ativo
        var cliente = await context.Clientes.FindAsync(dto.IdCliente);
        if (cliente is null || !cliente.Ativo)
            return (null, "Cliente não encontrado ou inativo.");

        // RN11.1 — Veículo ativo e disponível
        var carro = await context.Carros.FindAsync(dto.IdCarro);
        if (carro is null || !carro.Ativo)
            return (null, "Veículo não encontrado ou inativo.");

        if (carro.Status != "Disponível")
            return (null, $"O veículo '{carro.Placa}' não está disponível para venda (status atual: {carro.Status}).");

        // RN11.6 — Uma venda ativa por veículo
        if (await context.PedidosVenda.AnyAsync(p => p.IdCarro == dto.IdCarro && p.Ativo))
            return (null, "Este veículo já possui uma venda ativa.");

        decimal valorFinal = dto.ValorPedido;

        if (dto.FormaDePagamento == "À Vista")
        {
            // RN11.4 — Desconto de 5% sobre o preço do veículo para vendas à vista
            valorFinal = carro.Preco * 0.95m;
        }
        else
        {
            // RN11.3 — Para Financiado/Consórcio, valor não pode ser inferior ao preço do veículo
            if (dto.ValorPedido < carro.Preco)
                return (null, $"Para vendas financiadas ou por consórcio, o valor ({dto.ValorPedido:C}) não pode ser inferior ao preço do veículo ({carro.Preco:C}).");
        }

        var pedido = new PedidoVenda
        {
            IdCliente = dto.IdCliente,
            IdCarro = dto.IdCarro,
            DataPedido = dto.DataPedido,
            ValorPedido = valorFinal,
            FormaDePagamento = dto.FormaDePagamento,
            Observacoes = dto.Observacoes,
            Ativo = true
        };

        // RN11.5 — Atualizar status do veículo para "Vendido"
        carro.Status = "Vendido";

        context.PedidosVenda.Add(pedido);
        await context.SaveChangesAsync();

        // Recarrega para popular navegação
        await context.Entry(pedido).Reference(p => p.Cliente).LoadAsync();
        await context.Entry(pedido).Reference(p => p.Carro).LoadAsync();

        return (ToResponseDTO(pedido), null);
    }

    // ──────────────────────────────────────────────────────────
    // RF14 / RN11.1–RN11.4 — Atualizar venda
    // ──────────────────────────────────────────────────────────
    public async Task<(PedidoVendaResponseDTO? Response, string? Erro)> AtualizarAsync(int id, PedidoVendaUpdateDTO dto)
    {
        var pedido = await context.PedidosVenda
            .Include(p => p.Cliente)
            .Include(p => p.Carro)
            .FirstOrDefaultAsync(p => p.IdPedido == id && p.Ativo);

        if (pedido is null)
            return (null, "Venda não encontrada.");

        // RN11.2 — Forma de pagamento válida
        if (!FormasDePagamentoPermitidas.Contains(dto.FormaDePagamento))
            return (null, $"Forma de pagamento inválida. Valores permitidos: {string.Join(", ", FormasDePagamentoPermitidas)}.");

        // RN11.1 — Cliente ativo
        var cliente = await context.Clientes.FindAsync(dto.IdCliente);
        if (cliente is null || !cliente.Ativo)
            return (null, "Cliente não encontrado ou inativo.");

        // RN11.1 — Veículo ativo
        var novoCarro = await context.Carros.FindAsync(dto.IdCarro);
        if (novoCarro is null || !novoCarro.Ativo)
            return (null, "Veículo não encontrado ou inativo.");

        // Se o carro mudou, libera o antigo e valida o novo
        if (dto.IdCarro != pedido.IdCarro)
        {
            if (novoCarro.Status != "Disponível")
                return (null, $"O veículo '{novoCarro.Placa}' não está disponível para venda (status atual: {novoCarro.Status}).");

            if (await context.PedidosVenda.AnyAsync(p => p.IdCarro == dto.IdCarro && p.Ativo && p.IdPedido != id))
                return (null, "Este veículo já possui uma venda ativa.");

            // Libera o carro anterior
            var carroAnterior = pedido.Carro!;
            if (carroAnterior.Ativo)
                carroAnterior.Status = "Disponível";

            // Marca o novo carro como vendido
            novoCarro.Status = "Vendido";
        }

        decimal valorFinal = dto.ValorPedido;

        if (dto.FormaDePagamento == "À Vista")
        {
            // RN11.4 — Desconto automático de 5%
            valorFinal = novoCarro.Preco * 0.95m;
        }
        else
        {
            // RN11.3 — Valor mínimo para financiamento/consórcio
            if (dto.ValorPedido < novoCarro.Preco)
                return (null, $"Para vendas financiadas ou por consórcio, o valor ({dto.ValorPedido:C}) não pode ser inferior ao preço do veículo ({novoCarro.Preco:C}).");
        }

        pedido.IdCliente = dto.IdCliente;
        pedido.IdCarro = dto.IdCarro;
        pedido.DataPedido = dto.DataPedido;
        pedido.ValorPedido = valorFinal;
        pedido.FormaDePagamento = dto.FormaDePagamento;
        pedido.Observacoes = dto.Observacoes;

        await context.SaveChangesAsync();

        // Recarrega navegação atualizada
        await context.Entry(pedido).Reference(p => p.Cliente).LoadAsync();
        await context.Entry(pedido).Reference(p => p.Carro).LoadAsync();

        return (ToResponseDTO(pedido), null);
    }

    // ──────────────────────────────────────────────────────────
    // RF15 / RN15.1, RN15.2 — Exclusão lógica de venda
    // ──────────────────────────────────────────────────────────
    public async Task<(bool Sucesso, string? Erro)> ExcluirAsync(int id)
    {
        var pedido = await context.PedidosVenda
            .Include(p => p.Carro)
            .FirstOrDefaultAsync(p => p.IdPedido == id && p.Ativo);

        if (pedido is null)
            return (false, "Venda não encontrada.");

        // RN15.1 — Remoção lógica
        pedido.Ativo = false;

        // RN15.2 — Devolve o veículo a "Disponível" se ele ainda estiver ativo
        if (pedido.Carro is not null && pedido.Carro.Ativo)
            pedido.Carro.Status = "Disponível";

        await context.SaveChangesAsync();

        return (true, null);
    }

    // ──────────────────────────────────────────────────────────
    // Helper de mapeamento
    // ──────────────────────────────────────────────────────────
    private static PedidoVendaResponseDTO ToResponseDTO(PedidoVenda p) => new()
    {
        IdPedido = p.IdPedido,
        IdCliente = p.IdCliente,
        NomeCliente = p.Cliente?.Nome ?? string.Empty,
        IdCarro = p.IdCarro,
        ModeloCarro = p.Carro is not null ? $"{p.Carro.Marca} {p.Carro.Modelo}" : string.Empty,
        PlacaCarro = p.Carro?.Placa ?? string.Empty,
        DataPedido = p.DataPedido,
        ValorPedido = p.ValorPedido,
        FormaDePagamento = p.FormaDePagamento,
        Observacoes = p.Observacoes,
        Ativo = p.Ativo
    };
}
