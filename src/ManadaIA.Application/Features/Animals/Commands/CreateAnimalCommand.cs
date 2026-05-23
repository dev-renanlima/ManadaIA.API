using ManadaIA.Application.DTOs;
using MediatR;

namespace ManadaIA.Application.Features.Animals.Commands;

public sealed record CreateAnimalCommand(
    string EarTag,
    string Name,
    string Breed,
    int Gender,
    DateTime BirthDate,
    Guid PropertyId,
    decimal? InitialWeight
) : IRequest<AnimalDto>;
