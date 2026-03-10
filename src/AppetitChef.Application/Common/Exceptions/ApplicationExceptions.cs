namespace AppetitChef.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key)
        : base($"'{name}' com chave '{key}' não foi encontrado.") { }
}

public class ValidationException : Exception
{
    public IDictionary<string, string[]> Errors { get; }

    public ValidationException(IEnumerable<FluentValidation.Results.ValidationFailure> failures)
        : base("Um ou mais erros de validação ocorreram.")
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(g => g.Key, g => g.ToArray());
    }
}

public class ForbiddenAccessException : Exception
{
    public ForbiddenAccessException() : base("Acesso negado.") { }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException(string message = "Não autorizado.") : base(message) { }
}
