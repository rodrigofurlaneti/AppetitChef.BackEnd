using AppetitChef.Application.Features.Reservas.Commands;
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
    public async Task<IActionResult> Criar([FromBody] CriarReservaCommand cmd, CancellationToken ct)
    {
        var r = await Mediator.Send(cmd, ct);
        return r.Sucesso ? Ok(r) : BadRequest(r.Erros);
    }
}