using AppetitChef.Domain.Interfaces; // Certifique-se de que este using existe
using BCrypt.Net; // Requer o pacote instalado no passo 1

namespace AppetitChef.Infrastructure.Security;

public class PasswordService : IPasswordService
{
    public string Hash(string senha) => BCrypt.Net.BCrypt.HashPassword(senha);

    public bool Verify(string senha, string hash) => BCrypt.Net.BCrypt.Verify(senha, hash);
}