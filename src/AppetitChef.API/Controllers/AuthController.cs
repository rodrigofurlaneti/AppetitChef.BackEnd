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
        return Ok(result);
    }

    /// <summary>Registra um novo funcionario</summary>
    [HttpPost("registrar")]
    [ProducesResponseType(typeof(int), 201)]
    public async Task<IActionResult> Registrar([FromBody] RegistrarFuncionarioCommand command, CancellationToken ct)
    {
        var id = await Mediator.Send(command, ct);
        return CreatedAtAction(nameof(Login), new { }, new { id });
    }
}