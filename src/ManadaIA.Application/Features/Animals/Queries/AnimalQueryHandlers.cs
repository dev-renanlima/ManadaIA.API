using ManadaIA.Application.DTOs;
using ManadaIA.Domain.Entities;
using ManadaIA.Domain.Exceptions;
using ManadaIA.Domain.Interfaces;
using MediatR;

namespace ManadaIA.Application.Features.Animals.Queries;

public sealed class GetAnimalByIdHandler(IAnimalRepository repository)
    : IRequestHandler<GetAnimalByIdQuery, AnimalDto>
{
    public async Task<AnimalDto> Handle(GetAnimalByIdQuery request, CancellationToken ct)
    {
        var animal = await repository.GetByIdAsync(request.Id, ct);
        if (animal is null)
            throw new NotFoundException("Animal", request.Id);

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

public sealed class GetAnimalsByPropertyHandler(IAnimalRepository repository)
    : IRequestHandler<GetAnimalsByPropertyQuery, IReadOnlyList<AnimalDto>>
{
    public async Task<IReadOnlyList<AnimalDto>> Handle(GetAnimalsByPropertyQuery request, CancellationToken ct)
    {
        var animals = request.OnlyActive
            ? await repository.GetActiveAsync(request.PropertyId, ct)
            : await repository.GetByPropertyIdAsync(request.PropertyId, ct);

        return animals.Select(MapToDto).ToList();
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
