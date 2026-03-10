using AppetitChef.Application.Features.Mesa.Dtos;
using AppetitChef.Domain.Interfaces;
using MediatR;
using AppetitChef.Domain.Enums; // Garante acesso ao enum StatusMesa

namespace AppetitChef.Application.Features.Mesa.Queries
{
    // 1. Definição da Query solicitando o DTO correto
    public record ListarMesasQuery(int FilialId, bool? SomenteDisponiveis) : IRequest<IEnumerable<MesaQueryDto>>;

    // 2. Handler implementando a interface do MediatR
    public class ListarMesasHandler(IUnitOfWork uow) : IRequestHandler<ListarMesasQuery, IEnumerable<MesaQueryDto>>
    {
        public async Task<IEnumerable<MesaQueryDto>> Handle(ListarMesasQuery request, CancellationToken ct)
        {
            // Busca as mesas através do Unit of Work
            var mesas = await uow.Mesas.GetByFilialAsync(request.FilialId, ct);

            // 3. Filtro de Status
            if (request.SomenteDisponiveis == true)
            {
                // ATENÇÃO: Se 'Disponivel' continuar dando erro vermelho:
                // Tente mudar para StatusMesa.Livre ou StatusMesa.Disponível (com acento)
                // Ou comente a linha abaixo temporariamente para o build passar.
                mesas = mesas.Where(m => m.Status == StatusMesa.Disponivel);
            }

            // 4. Mapeamento para o DTO de Resposta
            return mesas.Select(m => new MesaQueryDto(
                m.Id,
                m.Numero,
                m.Status.ToString(),
                m.Area?.Nome ?? "Geral"
            ));
        }
    }
}