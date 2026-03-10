using AppetitChef.Application.Features.Estoque;
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
        return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
    }

    /// <summary>Movimenta estoque (entrada, saida, ajuste, perda)</summary>
    [HttpPost("movimentar")]
    public async Task<IActionResult> Movimentar([FromBody] MovimentarEstoqueCommand cmd, CancellationToken ct)
    {
        await Mediator.Send(cmd, ct);
        return Ok();
    }

    /// <summary>Lista insumos com estoque critico</summary>
    [HttpGet("criticos")]
    [ProducesResponseType(typeof(IEnumerable<InsumoDto>), 200)]
    public async Task<IActionResult> ListarCriticos(CancellationToken ct)
    {
        var insumos = await Mediator.Send(new GetInsumoCriticosQuery(), ct);
        return Ok(insumos);
    }

    /// <summary>Cria um novo insumo</summary>
    [HttpPost("insumos")]
    public async Task<IActionResult> CriarInsumo([FromBody] CriarInsumoCommand cmd, CancellationToken ct)
    {
        var id = await Mediator.Send(cmd, ct);
        return CreatedAtAction(nameof(ListarCriticos), new { }, new { id });
    }
}
