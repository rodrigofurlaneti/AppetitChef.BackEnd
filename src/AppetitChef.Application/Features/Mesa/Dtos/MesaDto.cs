namespace AppetitChef.Application.Features.Mesa.Dtos
{
    public record MesaDto(int Id, int AreaId, string Area, int Numero, int Capacidade, string Status);
}
