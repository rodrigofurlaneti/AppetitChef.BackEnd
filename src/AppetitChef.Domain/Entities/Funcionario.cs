using AppetitChef.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppetitChef.Domain.Entities
{
    // ─────────────────────────────────────────────
    // Funcionario
    // ─────────────────────────────────────────────
    public class Funcionario : BaseEntity
    {
        public int FilialId { get; private set; }
        public Filial Filial { get; private set; } = null!;
        public int CargoId { get; private set; }
        public Cargo Cargo { get; private set; } = null!;
        public string Nome { get; private set; } = null!;
        public string Cpf { get; private set; } = null!;
        public string? Rg { get; private set; }
        public DateOnly DataNascimento { get; private set; }
        public DateOnly DataAdmissao { get; private set; }
        public DateOnly? DataDemissao { get; private set; }
        public string? Telefone { get; private set; }
        public string? Email { get; private set; }
        public string? Login { get; private set; }
        public string? SenhaHash { get; private set; }
        public bool Ativo { get; private set; } = true;

        protected Funcionario() { }

        public static Funcionario Criar(int filialId, int cargoId, string nome, string cpf,
            DateOnly dataNascimento, DateOnly dataAdmissao, string? email = null, string? login = null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(nome);
            ArgumentException.ThrowIfNullOrWhiteSpace(cpf);
            return new Funcionario
            {
                FilialId = filialId,
                CargoId = cargoId,
                Nome = nome,
                Cpf = cpf,
                DataNascimento = dataNascimento,
                DataAdmissao = dataAdmissao,
                Email = email,
                Login = login
            };
        }

        public void DefinirSenha(string hash) { SenhaHash = hash; SetUpdatedAt(); }
        public void Desligar(DateOnly dataDemissao)
        {
            if (dataDemissao < DataAdmissao)
                throw new ArgumentException("Data de demissão não pode ser anterior à admissão.");
            DataDemissao = dataDemissao;
            Ativo = false;
            SetUpdatedAt();
        }
    }
}
