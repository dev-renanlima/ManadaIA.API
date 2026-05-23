using ManadaIA.Application.DTOs;
using MediatR;

namespace ManadaIA.Application.Features.Animals.Queries;

public sealed record GetAnimalByIdQuery(Guid Id) : IRequest<AnimalDto>;

public sealed record GetAnimalsByPropertyQuery(Guid PropertyId, bool OnlyActive = true) 
    : IRequest<IReadOnlyList<AnimalDto>>;

public sealed record GetAnimalsByBatchQuery(Guid BatchId) 
    : IRequest<IReadOnlyList<AnimalDto>>;
