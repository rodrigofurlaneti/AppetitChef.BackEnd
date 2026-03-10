namespace AppetitChef.Application.Features.Clientes.Dto
{
    public record ClienteDto(int Id, string Nome, string? Cpf, string? Email, string? Telefone, int PontosFidelidade);
}
