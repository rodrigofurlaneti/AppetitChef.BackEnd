using AppetitChef.Application.Common.Exceptions;
using AppetitChef.Application.Common.Interfaces;
using AppetitChef.Application.Features.Auth.Dtos;
using AppetitChef.Domain.Entities;
using AppetitChef.Domain.Interfaces;
using FluentValidation;
using MediatR;

namespace AppetitChef.Application.Features.Auth.Commands;

// ─── Login ───────────────────────────────────────────────────────────────────
public record LoginCommand(string Login, string Senha) : IRequest<LoginResponse>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Login).NotEmpty().WithMessage("Login e obrigatorio.");
        RuleFor(x => x.Senha).NotEmpty().WithMessage("Senha e obrigatoria.");
    }
}

public class LoginCommandHandler(
    IUnitOfWork uow,
    IPasswordService hasher,
    IJwtService jwt) : IRequestHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken ct)
    {
        var funcionario = await uow.Funcionarios.GetByLoginAsync(request.Login, ct)
            ?? throw new UnauthorizedException("Login ou senha invalidos.");

        if (!funcionario.Ativo)
            throw new UnauthorizedException("Usuario inativo.");

        if (string.IsNullOrEmpty(funcionario.SenhaHash) || !hasher.Verify(request.Senha, funcionario.SenhaHash))
            throw new UnauthorizedException("Login ou senha invalidos.");

        var roles = new[] { funcionario.Cargo?.Nome ?? "Funcionario" };
        var token = jwt.GenerateToken(funcionario, roles);
        var refreshToken = jwt.GenerateRefreshToken();
        var expiracao = DateTime.UtcNow.AddHours(8);

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

// ─── Registrar Funcionario ────────────────────────────────────────────────────
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
    IPasswordService hasher) : IRequestHandler<RegistrarFuncionarioCommand, int>
{
    public async Task<int> Handle(RegistrarFuncionarioCommand request, CancellationToken ct)
    {
        var existente = await uow.Funcionarios.GetByLoginAsync(request.Login, ct);
        if (existente is not null)
            throw new InvalidOperationException($"Login '{request.Login}' ja esta em uso.");

        var existenteCpf = await uow.Funcionarios.GetByCpfAsync(request.Cpf, ct);
        if (existenteCpf is not null)
            throw new InvalidOperationException("CPF ja cadastrado.");

        var funcionario = Funcionario.Criar(
            request.FilialId,
            request.CargoId,
            request.Nome,
            request.Cpf,
            request.DataNascimento,
            request.DataAdmissao,
            request.Email,
            request.Login);

        funcionario.DefinirSenha(hasher.Hash(request.Senha));
        await uow.Funcionarios.AddAsync(funcionario, ct);
        await uow.CommitAsync(ct);
        return funcionario.Id;
    }
}
