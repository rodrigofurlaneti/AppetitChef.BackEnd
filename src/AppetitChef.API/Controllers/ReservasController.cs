using AppetitChef.Application.Features.Reservas.Commands;
using AppetitChef.Application.Features.Reservas.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppetitChef.API.Controllers;

/// <summary>Gestao de Reservas</summary>
[Authorize]
[Tags("Reservas")]
public class ReservasController(IMediator mediator) : BaseController(mediator)
{
    /// <summary>Cria uma nova reserva</summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> Criar([FromBody] CriarReservaCommand cmd, CancellationToken ct)
    {
        var r = await Mediator.Send(cmd, ct);
        return r.IsSuccess ? CreatedAtAction(nameof(ListarPorData), new { }, r.Value) : BadRequest(r.Error);
    }

    /// <summary>Confirma uma reserva</summary>
    [HttpPut("{id}/confirmar")]
    public async Task<IActionResult> Confirmar(int id, CancellationToken ct)
    {
        var r = await Mediator.Send(new ConfirmarReservaCommand(id), ct);
        return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
    }

    /// <summary>Cancela uma reserva</summary>
    [HttpPut("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(int id, CancellationToken ct)
    {
        var r = await Mediator.Send(new CancelarReservaCommand(id), ct);
        return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
    }

    /// <summary>Lista reservas por data</summary>
    [HttpGet]
    public async Task<IActionResult> ListarPorData([FromQuery] int filialId, [FromQuery] DateTime data, CancellationToken ct)
    {
        var r = await Mediator.Send(new GetReservasByDataQuery(filialId, data), ct);
        return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
    }
}
