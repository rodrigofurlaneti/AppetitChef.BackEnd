using AppetitChef.Application.Features.Auth.Commands;
using AppetitChef.Application.Features.Auth.Dtos;

using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppetitChef.API.Controllers;

/// <summary>Autenticacao</summary>
[Tags("Auth")]
public class AuthController(IMediator mediator) : BaseController(mediator)
{
    /// <summary>Realiza login e retorna token JWT</summary>
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), 200)]
    [ProducesResponseType(401)]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var result = await Mediator.Send(command, ct);
        if (!result.Sucesso) return Unauthorized(new { result.Erros });
        return Ok(result.Dados);
    }
}