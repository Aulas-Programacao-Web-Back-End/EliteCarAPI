using EliteCarAPI.DTOs;
using EliteCarAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EliteCarAPI.Controllers;

[ApiController]
[Route("api/pedidosvenda")]
[Produces("application/json")]
public class PedidosVendaController(PedidoVendaService pedidoVendaService) : ControllerBase
{
    // GET api/pedidosvenda
    // RF12 — Listar todas as vendas ativas
    [HttpGet]
    [ProducesResponseType(typeof(List<PedidoVendaResponseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var pedidos = await pedidoVendaService.ListarAtivosAsync();
        return Ok(pedidos);
    }

    // GET api/pedidosvenda/data/{data}
    // RF13 — Consultar vendas por data (ex: 2026-08-10)
    [HttpGet("data/{data}")]
    [ProducesResponseType(typeof(List<PedidoVendaResponseDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetByData(string data)
    {
        if (!DateOnly.TryParseExact(data, "yyyy-MM-dd", out var dataParsed))
            return BadRequest(new { mensagem = "Formato de data inválido. Utilize yyyy-MM-dd (ex: 2026-08-10)." });

        var pedidos = await pedidoVendaService.BuscarPorDataAsync(dataParsed);
        return Ok(pedidos);
    }

    // POST api/pedidosvenda
    // RF11 — Cadastrar venda
    [HttpPost]
    [ProducesResponseType(typeof(PedidoVendaResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] PedidoVendaCreateDTO dto)
    {
        var (response, erro) = await pedidoVendaService.CriarAsync(dto);

        if (erro is not null)
        {
            // Conflito de regra de negócio (veículo já vendido, cliente inativo, etc.)
            return Conflict(new { mensagem = erro });
        }

        return CreatedAtAction(nameof(GetAll), null, response);
    }

    // PUT api/pedidosvenda/{id}
    // RF14 — Atualizar venda
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PedidoVendaResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] PedidoVendaUpdateDTO dto)
    {
        var (response, erro) = await pedidoVendaService.AtualizarAsync(id, dto);

        if (response is null && erro == "Venda não encontrada.")
            return NotFound(new { mensagem = erro });

        if (erro is not null)
            return Conflict(new { mensagem = erro });

        return Ok(response);
    }

    // DELETE api/pedidosvenda/{id}
    // RF15 — Exclusão lógica de venda (restitui o veículo)
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var (sucesso, erro) = await pedidoVendaService.ExcluirAsync(id);
        if (!sucesso)
            return NotFound(new { mensagem = erro });

        return NoContent();
    }
}
