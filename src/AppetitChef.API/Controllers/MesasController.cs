using AppetitChef.Application.Features.Mesa.Commands;
using AppetitChef.Application.Features.Mesa.Dtos;
using AppetitChef.Application.Features.Mesa.Queries;
using AppetitChef.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppetitChef.API.Controllers;

/// <summary>Gestao de Mesas</summary>
[Authorize]
[Tags("Mesas")]
public class MesasController(IMediator mediator) : BaseController(mediator)
{
    /// <summary>Lista mesas de uma filial</summary>
    [HttpGet("{filialId}")]
    [ProducesResponseType(typeof(IEnumerable<MesaQueryDto>), 200)]
    public async Task<IActionResult> Listar(int filialId, [FromQuery] bool? somenteDisponiveis, CancellationToken ct) =>
        Ok(await Mediator.Send(new ListarMesasQuery(filialId, somenteDisponiveis), ct));

    /// <summary>Cria uma nova mesa</summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Gerente")]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> Criar([FromBody] CriarMesaCommand cmd, CancellationToken ct)
    {
        var r = await Mediator.Send(cmd, ct);
        return r.IsSuccess ? CreatedAtAction(nameof(Listar), new { }, new { id = r.Value }) : BadRequest(r.Error);
    }

    /// <summary>Altera o status de uma mesa manualmente</summary>
    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin,Gerente,Garcom")]
    public async Task<IActionResult> AlterarStatus(int id, [FromBody] StatusMesa novoStatus, CancellationToken ct)
    {
        var r = await Mediator.Send(new AlterarStatusMesaCommand(id, novoStatus), ct);
        return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
    }
}
