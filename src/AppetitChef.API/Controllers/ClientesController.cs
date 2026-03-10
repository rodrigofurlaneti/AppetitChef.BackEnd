using AppetitChef.Application.Features.Clientes.Commands;
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
    public async Task<IActionResult> Criar([FromBody] CriarClienteCommand cmd, CancellationToken ct)
    {
        var r = await Mediator.Send(cmd, ct);
        return r.Sucesso ? Ok(r) : BadRequest(r.Erros);
    }
}