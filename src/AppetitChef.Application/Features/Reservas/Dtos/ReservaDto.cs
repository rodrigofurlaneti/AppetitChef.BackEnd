namespace AppetitChef.Application.Features.Reservas.Dtos
{
    public record ReservaDto(int Id, int FilialId, string NomeContato, string Telefone,
        DateTime DataReserva, int NumPessoas, string Status, string? Observacao);
}
