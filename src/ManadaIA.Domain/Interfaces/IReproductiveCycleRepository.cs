using ManadaIA.Domain.Entities;

namespace ManadaIA.Domain.Interfaces;

public interface IReproductiveCycleRepository : IRepository<ReproductiveCycle>
{
    Task<IReadOnlyList<ReproductiveCycle>> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
    Task<IReadOnlyList<ReproductiveCycle>> GetByEventTypeAsync(EventType eventType, CancellationToken ct = default);
}
