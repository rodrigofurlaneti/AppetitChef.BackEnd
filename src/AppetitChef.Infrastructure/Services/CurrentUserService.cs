using AppetitChef.Application.Common.Interfaces;
using AppetitChef.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace AppetitChef.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private ClaimsPrincipal? User => httpContextAccessor.HttpContext?.User;

    // Converte a string do Claim para int? para bater com a Interface
    public int? UserId => int.TryParse(User?.FindFirstValue(ClaimTypes.NameIdentifier), out var id) ? id : null;

    public string? UserName => User?.Identity?.Name;

    public IEnumerable<string> Roles => User?.FindAll(ClaimTypes.Role).Select(x => x.Value) ?? [];

    public bool IsInRole(string role) => User?.IsInRole(role) ?? false;
}