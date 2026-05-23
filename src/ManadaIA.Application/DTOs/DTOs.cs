namespace ManadaIA.Application.DTOs;

public sealed record AnimalDto(
    Guid Id,
    string EarTag,
    string Name,
    string Breed,
    string Gender,
    DateTime BirthDate,
    int AgeMonths,
    decimal? CurrentWeight,
    string Status,
    Guid PropertyId,
    Guid? BatchId,
    DateTime CreatedAt
);

public sealed record CreateAnimalRequest(
    string EarTag,
    string Name,
    string Breed,
    int Gender,
    DateTime BirthDate,
    Guid PropertyId,
    decimal? InitialWeight
);

public sealed record UpdateAnimalRequest(
    string? Name,
    string? Breed,
    Guid? BatchId
);

public sealed record PropertyDto(
    Guid Id,
    string Name,
    string City,
    string State,
    decimal? AreaHectares,
    string? Registration,
    bool IsActive,
    Guid OwnerId,
    DateTime CreatedAt
);

public sealed record CreatePropertyRequest(
    string Name,
    string City,
    string State,
    decimal? AreaHectares,
    string? Registration
);

public sealed record BatchDto(
    Guid Id,
    string Name,
    string? Description,
    Guid PropertyId,
    bool IsActive,
    DateTime CreatedAt
);

public sealed record CreateBatchRequest(
    string Name,
    Guid PropertyId,
    string? Description
);

public sealed record WeighingDto(
    Guid Id,
    Guid AnimalId,
    decimal Weight,
    DateTime WeighingDate,
    string? Observations,
    DateTime CreatedAt
);

public sealed record CreateWeighingRequest(
    Guid AnimalId,
    decimal Weight,
    DateTime WeighingDate,
    string? Observations
);

public sealed record VaccineDto(
    Guid Id,
    Guid AnimalId,
    string VaccineName,
    DateTime ApplicationDate,
    DateTime? NextDoseDate,
    string? Batch,
    string? Veterinarian,
    string? Observations,
    DateTime CreatedAt
);

public sealed record CreateVaccineRequest(
    Guid AnimalId,
    string VaccineName,
    DateTime ApplicationDate,
    DateTime? NextDoseDate,
    string? Batch,
    string? Veterinarian,
    string? Observations
);

public sealed record PagedResult<T>(
    IReadOnlyList<T> Items,
    int TotalCount,
    int Page,
    int PageSize
);
