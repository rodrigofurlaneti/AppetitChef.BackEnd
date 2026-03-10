using AppetitChef.Application.Features.Produtos.Commands; 
using AppetitChef.Application.Features.Produtos.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppetitChef.API.Controllers;

[Authorize]
[Tags("Produtos")]
public class ProdutosController(IMediator mediator) : BaseController(mediator)
{
    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] bool? somenteDisponiveis, CancellationToken ct) =>
        Ok(await Mediator.Send(new ListarProdutosQuery(somenteDisponiveis), ct));

    [HttpPost]
    [Authorize(Roles = "Admin,Gerente")]
    public async Task<IActionResult> Criar([FromBody] CriarProdutoCommand cmd, CancellationToken ct)
    {
        var r = await Mediator.Send(cmd, ct);
        // Note: Se o seu Result<T> usa 'IsSuccess', troque 'Sucesso' por 'IsSuccess'
        return r.IsSuccess ? Ok(r.Data) : BadRequest(r.Errors);
    }
}