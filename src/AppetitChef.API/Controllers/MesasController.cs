using AppetitChef.Application.Features.Mesa.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppetitChef.API.Controllers;

[Authorize]
[Tags("Mesas")]
public class MesasController(IMediator mediator) : BaseController(mediator)
{
    /// <summary>Lista mesas de uma filial</summary>
    [HttpGet("{filialId}")]
    public async Task<IActionResult> Listar(int filialId, [FromQuery] bool? somenteDisponiveis, CancellationToken ct) =>
        Ok(await Mediator.Send(new ListarMesasQuery(filialId, somenteDisponiveis), ct));
}