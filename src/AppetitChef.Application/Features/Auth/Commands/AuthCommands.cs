using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Application.Common.Interfaces;
using AppetitChef.Application.Features.Auth.Dtos; // IMPORTANTE: Para enxergar o LoginResponse
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace AppetitChef.Application.Features.Auth.Commands;

// ── Login (Comando e Resposta) ──────────────────────────────────────────────

// O comando agora solicita um LoginResponse (que está no seu arquivo separado)
public record LoginCommand(string Login, string Senha) : IRequest<LoginResponse>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Login).NotEmpty().WithMessage("Login é obrigatório.");
        RuleFor(x => x.Senha).NotEmpty().WithMessage("Senha é obrigatória.");
    }
}

public class LoginCommandHandler(
    IUnitOfWork uow,
    IPasswordHasher hasher, // Certifique-se que esta interface existe no Domain ou Application.Interfaces
    IJwtService jwt) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        // 1. Busca o funcionário e carrega o Cargo (necessário para o Perfil)
        var funcionario = await uow.Funcionarios.GetByLoginAsync(request.Login, ct)
            ?? throw new UnauthorizedException("Login ou senha inválidos.");

        // 2. Validações de segurança
        if (!funcionario.Ativo)
            throw new UnauthorizedException("Usuário inativo.");

        if (string.IsNullOrEmpty(funcionario.SenhaHash) || !hasher.Verify(request.Senha, funcionario.SenhaHash))
            throw new UnauthorizedException("Login ou senha inválidos.");

        // 3. Preparação dos dados do Token
        // Nota: Ajuste 'funcionario.Cargo.Nome' conforme o nome da propriedade na sua entidade Funcionario
        var roles = new[] { funcionario.Cargo?.Nome ?? "Funcionario" };

        var token = jwt.GenerateToken(funcionario, roles);
        var refreshToken = jwt.GenerateRefreshToken();
        var expiracao = DateTime.UtcNow.AddHours(8);

        // 4. Retorno do DTO LoginResponse que você criou no outro arquivo
        return new LoginResponse(
            token,
            refreshToken,
            expiracao,
            funcionario.Nome,
            roles.FirstOrDefault() ?? "Sem Perfil",
            funcionario.FilialId
        );
    }
}

// ── Registrar Funcionário ─────────────────────────────────────────────────────

public record RegistrarFuncionarioCommand(
    int FilialId,
    int CargoId,
    string Nome,
    string Cpf,
    DateOnly DataNascimento,
    DateOnly DataAdmissao,
    string Email,
    string Login,
    string Senha) : IRequest<int>;

public class RegistrarFuncionarioValidator : AbstractValidator<RegistrarFuncionarioCommand>
{
    public RegistrarFuncionarioValidator()
    {
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Cpf).NotEmpty().Length(14).WithMessage("CPF deve ter 14 caracteres (000.000.000-00).");
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Login).NotEmpty().MinimumLength(4).MaximumLength(60);
        RuleFor(x => x.Senha).NotEmpty().MinimumLength(6).WithMessage("Senha deve ter pelo menos 6 caracteres.");
        RuleFor(x => x.FilialId).GreaterThan(0);
        RuleFor(x => x.CargoId).GreaterThan(0);
    }
}

public class RegistrarFuncionarioHandler(
    IUnitOfWork uow,
    IPasswordHasher hasher) : IRequestHandler<RegistrarFuncionarioCommand, int>
{
    public async Task<int> Handle(RegistrarFuncionarioCommand request, CancellationToken ct)
    {
        // Valida unicidade de Login
        var existente = await uow.Funcionarios.GetByLoginAsync(request.Login, ct);
        if (existente is not null)
            throw new InvalidOperationException($"Login '{request.Login}' já está em uso.");

        // Valida unicidade de CPF
        var existenteCpf = await uow.Funcionarios.GetByCpfAsync(request.Cpf, ct);
        if (existenteCpf is not null)
            throw new InvalidOperationException("CPF já cadastrado.");

        // Cria a entidade usando o Factory Method do Domain
        var funcionario = Funcionario.Criar(
            request.FilialId,
            request.CargoId,
            request.Nome,
            request.Cpf,
            request.DataNascimento,
            request.DataAdmissao,
            request.Email,
            request.Login);

        // Define a senha hasheada
        funcionario.DefinirSenha(hasher.Hash(request.Senha));

        await uow.Funcionarios.AddAsync(funcionario, ct);
        await uow.CommitAsync(ct);

        return funcionario.Id;
    }
}