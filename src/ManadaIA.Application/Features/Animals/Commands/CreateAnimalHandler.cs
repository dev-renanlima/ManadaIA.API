using ManadaIA.Application.DTOs;
using ManadaIA.Domain.Entities;
using ManadaIA.Domain.Exceptions;
using ManadaIA.Domain.Interfaces;
using MediatR;

namespace ManadaIA.Application.Features.Animals.Commands;

public sealed class CreateAnimalHandler(
    IAnimalRepository animalRepository,
    IPropertyRepository propertyRepository)
    : IRequestHandler<CreateAnimalCommand, AnimalDto>
{
    public async Task<AnimalDto> Handle(CreateAnimalCommand request, CancellationToken ct)
    {
        // Validar se propriedade existe
        var property = await propertyRepository.GetByIdAsync(request.PropertyId, ct);
        if (property is null)
            throw new NotFoundException("Propriedade", request.PropertyId);

        // Validar se já existe animal com mesmo brinco
        var existing = await animalRepository.GetByEarTagAsync(request.EarTag, ct);
        if (existing is not null)
            throw new ConflictException($"Já existe um animal com o brinco '{request.EarTag}'");

        var gender = request.Gender == 1 ? Gender.Male : Gender.Female;

        var animal = Animal.Create(
            request.EarTag,
            request.Name,
            request.Breed,
            gender,
            request.BirthDate,
            request.PropertyId,
            request.InitialWeight
        );

        await animalRepository.AddAsync(animal, ct);

        return MapToDto(animal);
    }

    private static AnimalDto MapToDto(Animal animal)
    {
        var ageMonths = CalculateAgeMonths(animal.BirthDate);
        
        return new AnimalDto(
            animal.Id,
            animal.EarTag,
            animal.Name,
            animal.Breed,
            animal.Gender.ToString(),
            animal.BirthDate,
            ageMonths,
            animal.CurrentWeight,
            animal.Status.ToString(),
            animal.PropertyId,
            animal.BatchId,
            animal.CreatedAt
        );
    }

    private static int CalculateAgeMonths(DateTime birthDate)
    {
        var today = DateTime.UtcNow;
        var months = (today.Year - birthDate.Year) * 12 + today.Month - birthDate.Month;
        return months;
    }
}
