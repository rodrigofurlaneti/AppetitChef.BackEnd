using AppetitChef.Application.Features.Clientes.Commands;
using AppetitChef.Application.Features.Clientes.Dto;
using AppetitChef.Application.Features.Clientes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppetitChef.API.Controllers;

/// <summary>Gestao de Clientes</summary>
[Authorize]
[Tags("Clientes")]
public class ClientesController(IMediator mediator) : BaseController(mediator)
{
    /// <summary>Cadastra novo cliente</summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> Criar([FromBody] CriarClienteCommand cmd, CancellationToken ct)
    {
        var id = await Mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id }, new { id });
    }

    /// <summary>Busca cliente por ID</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ClienteDto), 200)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct)
    {
        var dto = await Mediator.Send(new GetClienteByIdQuery(id), ct);
        return Ok(dto);
    }

    /// <summary>Busca cliente por CPF</summary>
    [HttpGet("cpf/{cpf}")]
    [ProducesResponseType(typeof(ClienteDto), 200)]
    public async Task<IActionResult> ObterPorCpf(string cpf, CancellationToken ct)
    {
        var dto = await Mediator.Send(new GetClienteByCpfQuery(cpf), ct);
        return Ok(dto);
    }
}
