using FluentValidation;
using ManadaIA.Application.Features.Animals.Commands;

namespace ManadaIA.Application.Validators;

public sealed class CreateAnimalValidator : AbstractValidator<CreateAnimalCommand>
{
    public CreateAnimalValidator()
    {
        RuleFor(x => x.EarTag)
            .NotEmpty().WithMessage("Brinco é obrigatório")
            .MaximumLength(50).WithMessage("Brinco deve ter no máximo 50 caracteres");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Nome é obrigatório")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres");

        RuleFor(x => x.Breed)
            .NotEmpty().WithMessage("Raça é obrigatória")
            .MaximumLength(50).WithMessage("Raça deve ter no máximo 50 caracteres");

        RuleFor(x => x.Gender)
            .InclusiveBetween(1, 2).WithMessage("Sexo deve ser 1 (Macho) ou 2 (Fêmea)");

        RuleFor(x => x.BirthDate)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Data de nascimento não pode ser futura");

        RuleFor(x => x.InitialWeight)
            .GreaterThan(0).When(x => x.InitialWeight.HasValue)
            .WithMessage("Peso inicial deve ser maior que zero");
    }
}
