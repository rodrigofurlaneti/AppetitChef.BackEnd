using AppetitChef.Application.Features.Pedidos.Commands;
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
    public async Task<IActionResult> ListarAbertos(int filialId, CancellationToken ct) =>
        Ok(await Mediator.Send(new ListarPedidosAbertosQuery(filialId), ct));

    /// <summary>Obtem pedido pelo ID com itens</summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> Obter(int id, CancellationToken ct)
    {
        var r = await Mediator.Send(new ObterPedidoQuery(id), ct);
        return r.Sucesso ? Ok(r.Dados) : NotFound(r.Erros);
    }

    /// <summary>Abre um novo pedido</summary>
    [HttpPost]
    public async Task<IActionResult> Abrir([FromBody] AbrirPedidoCommand cmd, CancellationToken ct)
    {
        var r = await Mediator.Send(cmd, ct);
        return r.Sucesso ? CreatedAtAction(nameof(Obter), new { id = r.Dados }, r) : BadRequest(r.Erros);
    }

    /// <summary>Adiciona item ao pedido</summary>
    [HttpPost("{id}/itens")]
    public async Task<IActionResult> AdicionarItem(int id, [FromBody] AdicionarItemCommand cmd, CancellationToken ct)
    {
        var command = cmd with { PedidoId = id };
        var r = await Mediator.Send(command, ct);
        return r.Sucesso ? Ok(r) : BadRequest(r.Erros);
    }

    /// <summary>Fecha o pedido (fecha a comanda)</summary>
    [HttpPut("{id}/fechar")]
    public async Task<IActionResult> Fechar(int id, [FromQuery] bool taxaServico = true, CancellationToken ct = default)
    {
        var r = await Mediator.Send(new FecharPedidoCommand(id, taxaServico), ct);
        return r.Sucesso ? Ok(r) : BadRequest(r.Erros);
    }

    /// <summary>Cancela o pedido</summary>
    [HttpPut("{id}/cancelar")]
    [Authorize(Roles = "Admin,Gerente,Garcom")]
    public async Task<IActionResult> Cancelar(int id, CancellationToken ct)
    {
        var r = await Mediator.Send(new CancelarPedidoCommand(id), ct);
        return r.Sucesso ? Ok(r) : BadRequest(r.Erros);
    }
}