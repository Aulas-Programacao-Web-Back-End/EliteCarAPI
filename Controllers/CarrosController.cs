using EliteCarAPI.DTOs;
using EliteCarAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EliteCarAPI.Controllers;

[ApiController]
[Route("api/carros")]
[Produces("application/json")]
public class CarrosController(CarroService carroService) : ControllerBase
{
    // GET api/carros
    // RF07 — Listar veículos ativos
    [HttpGet]
    [ProducesResponseType(typeof(List<CarroResponseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var carros = await carroService.ListarAtivosAsync();
        return Ok(carros);
    }

    // GET api/carros/placa/{placa}
    // RF08 — Buscar veículo por placa
    [HttpGet("placa/{placa}")]
    [ProducesResponseType(typeof(CarroResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByPlaca(string placa)
    {
        var carro = await carroService.BuscarPorPlacaAsync(placa);
        if (carro is null)
            return NotFound(new { mensagem = $"Nenhum veículo ativo encontrado com a placa '{placa}'." });

        return Ok(carro);
    }

    // POST api/carros
    // RF06 — Cadastrar veículo
    [HttpPost]
    [ProducesResponseType(typeof(CarroResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CarroCreateDTO dto)
    {
        var (response, erro) = await carroService.CriarAsync(dto);
        if (erro is not null)
            return Conflict(new { mensagem = erro });

        return CreatedAtAction(nameof(GetByPlaca), new { placa = response!.Placa }, response);
    }

    // PUT api/carros/{id}
    // RF09 — Atualizar veículo
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CarroResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] CarroUpdateDTO dto)
    {
        var (response, erro) = await carroService.AtualizarAsync(id, dto);

        if (response is null && erro == "Veículo não encontrado.")
            return NotFound(new { mensagem = erro });

        if (erro is not null)
            return Conflict(new { mensagem = erro });

        return Ok(response);
    }

    // DELETE api/carros/{id}
    // RF10 — Exclusão lógica de veículo
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var (sucesso, erro) = await carroService.ExcluirAsync(id);
        if (!sucesso)
            return NotFound(new { mensagem = erro });

        return NoContent();
    }
}
