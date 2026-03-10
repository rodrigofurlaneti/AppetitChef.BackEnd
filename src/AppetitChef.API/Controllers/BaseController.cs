using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppetitChef.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public abstract class BaseController(IMediator mediator) : ControllerBase
{
    protected IMediator Mediator { get; } = mediator;
}