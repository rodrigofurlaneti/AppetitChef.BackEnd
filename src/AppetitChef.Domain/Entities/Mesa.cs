using AppetitChef.Domain.Common;
using AppetitChef.Domain.Enums;
using AppetitChef.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Entities
{
    // ─────────────────────────────────────────────
    // Mesa
    // ─────────────────────────────────────────────
    public class Mesa : BaseEntity
    {
        public int AreaId { get; private set; }
        public Area Area { get; private set; } = null!;
        public int Numero { get; private set; }
        public int Capacidade { get; private set; }
        public StatusMesa Status { get; private set; } = StatusMesa.Livre;

        protected Mesa() { }

        public static Mesa Criar(int areaId, int numero, int capacidade)
        {
            if (capacidade < 1 || capacidade > 50)
                throw new ArgumentOutOfRangeException(nameof(capacidade), "Capacidade deve ser entre 1 e 50.");
            return new Mesa { AreaId = areaId, Numero = numero, Capacidade = capacidade };
        }

        public void Ocupar()
        {
            if (Status is not StatusMesa.Livre and not StatusMesa.Reservada)
                throw new MesaIndisponivelException(Id);
            Status = StatusMesa.Ocupada;
            SetUpdatedAt();
        }

        public void Liberar()
        {
            Status = StatusMesa.Livre;
            SetUpdatedAt();
        }

        public void Reservar()
        {
            if (Status != StatusMesa.Livre)
                throw new MesaIndisponivelException(Id);
            Status = StatusMesa.Reservada;
            SetUpdatedAt();
        }

        public void ColocarEmManutencao()
        {
            Status = StatusMesa.Manutencao;
            SetUpdatedAt();
        }
    }
}
