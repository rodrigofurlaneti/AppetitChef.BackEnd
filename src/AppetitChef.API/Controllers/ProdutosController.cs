using AppetitChef.Application.Features.Produtos.Commands;
using AppetitChef.Application.Features.Produtos.Dtos;
using AppetitChef.Application.Features.Produtos.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppetitChef.API.Controllers;

/// <summary>Gestao de Produtos (Cardapio)</summary>
[Authorize]
[Tags("Produtos")]
public class ProdutosController(IMediator mediator) : BaseController(mediator)
{
    /// <summary>Lista todos os produtos</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProdutoDto>), 200)]
    public async Task<IActionResult> Listar([FromQuery] bool? somenteDisponiveis, CancellationToken ct) =>
        Ok(await Mediator.Send(new ListarProdutosQuery(somenteDisponiveis), ct));

    /// <summary>Cria um novo produto no cardapio</summary>
    [HttpPost]
    [Authorize(Roles = "Admin,Gerente")]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> Criar([FromBody] CriarProdutoCommand cmd, CancellationToken ct)
    {
        var r = await Mediator.Send(cmd, ct);
        return r.IsSuccess ? CreatedAtAction(nameof(Listar), new { }, new { id = r.Value }) : BadRequest(r.Error);
    }

    /// <summary>Atualiza preco de um produto</summary>
    [HttpPatch("{id}/preco")]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<IActionResult> AtualizarPreco(int id, [FromBody] decimal novoPreco, CancellationToken ct)
    {
        var r = await Mediator.Send(new AtualizarPrecoProdutoCommand(id, novoPreco), ct);
        return r.IsSuccess ? Ok(r.Value) : BadRequest(r.Error);
    }
}
