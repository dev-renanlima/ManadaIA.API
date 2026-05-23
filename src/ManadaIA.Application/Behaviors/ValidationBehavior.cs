using FluentValidation;
using MediatR;

namespace ManadaIA.Application.Behaviors;

/// <summary>
/// Pipeline Behavior que executa validações FluentValidation antes de qualquer handler
/// </summary>
public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct)
    {
        if (!validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, ct)));

        var failures = validationResults
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count != 0)
        {
            var errors = failures
                .Select(f => $"{f.PropertyName}: {f.ErrorMessage}")
                .ToList();
            
            throw new FluentValidation.ValidationException(
                $"Erros de validação: {string.Join(", ", errors)}");
        }

        return await next();
    }
}
