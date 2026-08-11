using EliteCarAPI.DTOs;
using EliteCarAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace EliteCarAPI.Controllers;

[ApiController]
[Route("api/clientes")]
[Produces("application/json")]
public class ClientesController(ClienteService clienteService) : ControllerBase
{
    // GET api/clientes
    // RF02 — Listar clientes ativos
    [HttpGet]
    [ProducesResponseType(typeof(List<ClienteResponseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var clientes = await clienteService.ListarAtivosAsync();
        return Ok(clientes);
    }

    // GET api/clientes/cpf/{cpf}
    // RF03 — Buscar cliente por CPF
    [HttpGet("cpf/{cpf}")]
    [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByCpf(string cpf)
    {
        var cliente = await clienteService.BuscarPorCpfAsync(cpf);
        if (cliente is null)
            return NotFound(new { mensagem = $"Nenhum cliente ativo encontrado com o CPF '{cpf}'." });

        return Ok(cliente);
    }

    // GET api/clientes/{id}
    // Buscar cliente por ID
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var cliente = await clienteService.BuscarPorIdAsync(id);
        if (cliente is null)
            return NotFound(new { mensagem = $"Nenhum cliente ativo encontrado com o ID {id}." });

        return Ok(cliente);
    }

    // POST api/clientes
    // RF01 — Cadastrar cliente
    [HttpPost]
    [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] ClienteCreateDTO dto)
    {
        var (response, erro) = await clienteService.CriarAsync(dto);
        if (erro is not null)
            return Conflict(new { mensagem = erro });

        return CreatedAtAction(nameof(GetByCpf), new { cpf = response!.Cpf }, response);
    }

    // PUT api/clientes/{id}
    // RF04 — Atualizar cliente
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ClienteResponseDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(int id, [FromBody] ClienteUpdateDTO dto)
    {
        var (response, erro) = await clienteService.AtualizarAsync(id, dto);

        if (response is null && erro == "Cliente não encontrado.")
            return NotFound(new { mensagem = erro });

        if (erro is not null)
            return Conflict(new { mensagem = erro });

        return Ok(response);
    }

    // DELETE api/clientes/{id}
    // RF05 — Exclusão lógica de cliente
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var (sucesso, erro) = await clienteService.ExcluirAsync(id);
        if (!sucesso)
            return NotFound(new { mensagem = erro });

        return NoContent();
    }
}
