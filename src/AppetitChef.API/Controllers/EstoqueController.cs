using AppetitChef.Application.Features.Estoque.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppetitChef.API.Controllers;

/// <summary>Controle de Estoque</summary>
[Authorize(Roles = "Admin,Gerente,Cozinheiro")]
[Tags("Estoque")]
public class EstoqueController(IMediator mediator) : BaseController(mediator)
{
    /// <summary>Registra movimentacao de estoque (entrada, saida, ajuste)</summary>
    [HttpPost("movimentos")]
    public async Task<IActionResult> Registrar([FromBody] RegistrarMovimentoCommand cmd, CancellationToken ct)
    {
        var r = await Mediator.Send(cmd, ct);
        return r.Sucesso ? Ok(r) : BadRequest(r.Erros);
    }
}