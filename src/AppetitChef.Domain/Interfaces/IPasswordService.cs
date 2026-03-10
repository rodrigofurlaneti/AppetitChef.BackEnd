namespace AppetitChef.Domain.Interfaces;

public interface IPasswordService
{
    string Hash(string senha);
    bool Verify(string senha, string hash);
}
