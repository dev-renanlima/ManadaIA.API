namespace ManadaIA.Domain.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message) { }
    
    public DomainException(string message, Exception innerException) 
        : base(message, innerException) { }
}

public class NotFoundException : DomainException
{
    public NotFoundException(string resource, object id)
        : base($"{resource} com id '{id}' não foi encontrado.") { }
}

public class ConflictException : DomainException
{
    public ConflictException(string message) : base(message) { }
}

public class UnauthorizedException : DomainException
{
    public UnauthorizedException(string message = "Não autorizado.")
        : base(message) { }
}

public class ValidationException : DomainException
{
    public ValidationException(string message) : base(message) { }
}
