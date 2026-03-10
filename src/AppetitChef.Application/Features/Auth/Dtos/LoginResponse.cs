using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Application.Features.Auth.Dtos
{
    public record LoginResponse(
        string Token,
        string RefreshToken,
        DateTime Expiracao,
        string Nome,
        string Perfil,
        int? FilialId
    );
}
