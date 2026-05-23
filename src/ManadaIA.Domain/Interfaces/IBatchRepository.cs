using ManadaIA.Domain.Entities;

namespace ManadaIA.Domain.Interfaces;

public interface IBatchRepository : IRepository<Batch>
{
    Task<IReadOnlyList<Batch>> GetByPropertyIdAsync(Guid propertyId, CancellationToken ct = default);
    Task<IReadOnlyList<Batch>> GetActiveAsync(Guid propertyId, CancellationToken ct = default);
}
