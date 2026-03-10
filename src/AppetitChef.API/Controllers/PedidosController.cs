using AppetitChef.Application.Features.Pedidos.Commands;
using AppetitChef.Application.Features.Pedidos.Dtos;
using AppetitChef.Application.Features.Pedidos.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppetitChef.API.Controllers;

/// <summary>Gestao de Pedidos (Comandas)</summary>
[Authorize]
[Tags("Pedidos")]
public class PedidosController(IMediator mediator) : BaseController(mediator)
{
    /// <summary>Lista pedidos em aberto de uma filial</summary>
    [HttpGet("abertos/{filialId}")]
    [ProducesResponseType(typeof(IEnumerable<PedidoDto>), 200)]
    public async Task<IActionResult> ListarAbertos(int filialId, CancellationToken ct) =>
        Ok(await Mediator.Send(new GetPedidosAbertosQuery(filialId), ct));

    /// <summary>Obtem pedido pelo ID com itens</summary>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PedidoDto), 200)]
    public async Task<IActionResult> Obter(int id, CancellationToken ct)
    {
        var r = await Mediator.Send(new GetPedidoByIdQuery(id), ct);
        return r.IsSuccess ? Ok(r.Value) : NotFound(r.Error);
    }

    /// <summary>Abre um novo pedido</summary>
    [HttpPost]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> Abrir([FromBody] AbrirPedidoCommand cmd, CancellationToken ct)
    {
        var r = await Mediator.Send(cmd, ct);
        return r.IsSuccess ? CreatedAtAction(nameof(Obter), new { id = r.Value }, new { id = r.Value }) : BadRequest(r.Error);
    }

    /// <summary>Adiciona item ao pedido</summary>
    [HttpPost("{id}/itens")]
    public async Task<IActionResult> AdicionarItem(int id, [FromBody] AdicionarItemCommand cmd, CancellationToken ct)
    {
        var command = cmd with { PedidoId = id };
        var r = await Mediator.Send(command, ct);
        return r.IsSuccess ? Ok(new { itemId = r.Value }) : BadRequest(r.Error);
    }

    /// <summary>Fecha o pedido (fecha a comanda)</summary>
    [HttpPut("{id}/fechar")]
    public async Task<IActionResult> Fechar(int id, [FromQuery] bool taxaServico = true, CancellationToken ct = default)
    {
        var r = await Mediator.Send(new FecharPedidoCommand(id, taxaServico), ct);
        return r.IsSuccess ? Ok(new { total = r.Value }) : BadRequest(r.Error);
    }

    /// <summary>Cancela o pedido</summary>
    [HttpPut("{id}/cancelar")]
    [Authorize(Roles = "Admin,Gerente,Garcom")]
    public async Task<IActionResult> Cancelar(int id, CancellationToken ct)
    {
        var r = await Mediator.Send(new CancelarPedidoCommand(id), ct);
        return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
    }
}
