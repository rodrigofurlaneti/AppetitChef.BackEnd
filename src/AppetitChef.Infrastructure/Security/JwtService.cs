using AppetitChef.Application.Common.Interfaces;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces; // Usando a interface do Domain
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AppetitChef.Infrastructure.Security;

public class JwtService(IConfiguration config) : IJwtService
{
    private readonly string _secret = config["Jwt:Secret"] ?? "SuaChaveSuperSecretaDePeloMenos32Caracteres";
    private readonly string _issuer = config["Jwt:Issuer"] ?? "AppetitChef";
    private readonly string _audience = config["Jwt:Audience"] ?? "AppetitChefClients";
    private readonly int _expiracaoMinutos = int.Parse(config["Jwt:ExpiracaoMinutos"] ?? "480");

    // Implementação exigida: GenerateToken com Roles
    public string GenerateToken(Funcionario funcionario, IEnumerable<string> roles)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, funcionario.Id.ToString()),
            new(JwtRegisteredClaimNames.UniqueName, funcionario.Login),
            new(ClaimTypes.Name, funcionario.Nome),
            new("filial_id", funcionario.FilialId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        // Adiciona as roles enviadas pelo Handler
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            _issuer,
            _audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_expiracaoMinutos),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    // Implementação exigida: GenerateRefreshToken
    public string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }

    // Implementação exigida: GetUserIdFromToken
    // Altere o método GetUserIdFromToken para converter o Subject em int
    public int? GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadJwtToken(token);
        return int.TryParse(jsonToken.Subject, out var id) ? id : null;
    }
}