using AppetitChef.Domain.Entities;

namespace AppetitChef.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(Funcionario funcionario, IEnumerable<string> roles);
    string GenerateRefreshToken();
    int? GetUserIdFromToken(string token);
}

public interface ICurrentUserService
{
    int? UserId { get; }
    string? UserName { get; }
    IEnumerable<string> Roles { get; }
    bool IsInRole(string role);
}

// IPasswordHasher mantido como alias de IPasswordService para compatibilidade
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hash);
}

public interface IEmailService
{
    Task SendAsync(string to, string subject, string body, CancellationToken ct = default);
}

public interface IDateTimeService
{
    DateTime UtcNow { get; }
    DateOnly Today { get; }
}
