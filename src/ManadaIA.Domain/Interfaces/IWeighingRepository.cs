using ManadaIA.Domain.Entities;

namespace ManadaIA.Domain.Interfaces;

public interface IWeighingRepository : IRepository<Weighing>
{
    Task<IReadOnlyList<Weighing>> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
    Task<Weighing?> GetLastWeighingAsync(Guid animalId, CancellationToken ct = default);
}
