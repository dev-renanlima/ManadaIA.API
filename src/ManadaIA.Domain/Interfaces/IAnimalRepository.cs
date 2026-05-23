using ManadaIA.Domain.Entities;

namespace ManadaIA.Domain.Interfaces;

public interface IAnimalRepository : IRepository<Animal>
{
    Task<Animal?> GetByEarTagAsync(string earTag, CancellationToken ct = default);
    Task<IReadOnlyList<Animal>> GetByPropertyIdAsync(Guid propertyId, CancellationToken ct = default);
    Task<IReadOnlyList<Animal>> GetByBatchIdAsync(Guid batchId, CancellationToken ct = default);
    Task<IReadOnlyList<Animal>> GetActiveAsync(Guid propertyId, CancellationToken ct = default);
}
