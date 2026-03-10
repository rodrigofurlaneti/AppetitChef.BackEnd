using AppetitChef.Domain.Common;
using AppetitChef.Domain.Enums;

namespace AppetitChef.Domain.Entities;

public class Reserva : BaseEntity
{
    public int FilialId { get; private set; }
    public int? ClienteId { get; private set; }
    public int? MesaId { get; private set; }
    public int FuncionarioId { get; private set; }
    public string NomeContato { get; private set; } = null!;
    public string TelefoneContato { get; private set; } = null!;
    public DateTime DataReserva { get; private set; }
    public int NumPessoas { get; private set; }
    public StatusReserva Status { get; private set; } = StatusReserva.Pendente;
    public string? Observacao { get; private set; }

    // --- PROPRIEDADES DE NAVEGAÇÃO (Faltavam estas linhas) ---
    public virtual Cliente? Cliente { get; private set; }
    public virtual Mesa? Mesa { get; private set; }
    public virtual Funcionario Funcionario { get; private set; } = null!;

    protected Reserva() { }

    public static Reserva Criar(int filialId, int funcionarioId, string nomeContato,
        string telefone, DateTime dataReserva, int numPessoas, int? clienteId = null, int? mesaId = null)
    {
        if (numPessoas < 1) throw new ArgumentException("Número de pessoas deve ser pelo menos 1.");

        return new Reserva
        {
            FilialId = filialId,
            FuncionarioId = funcionarioId,
            NomeContato = nomeContato,
            TelefoneContato = telefone,
            DataReserva = dataReserva,
            NumPessoas = numPessoas,
            ClienteId = clienteId,
            MesaId = mesaId
        };
    }

    public void Confirmar() { Status = StatusReserva.Confirmada; SetUpdatedAt(); }
    public void Cancelar() { Status = StatusReserva.Cancelada; SetUpdatedAt(); }
    public void Concluir() { Status = StatusReserva.Concluida; SetUpdatedAt(); }
}