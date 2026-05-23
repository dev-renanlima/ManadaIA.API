using ManadaIA.Domain.Entities;

namespace ManadaIA.Domain.Interfaces;

public interface IAIPredictionRepository : IRepository<AIPrediction>
{
    Task<IReadOnlyList<AIPrediction>> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
    Task<AIPrediction?> GetLatestByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
}
