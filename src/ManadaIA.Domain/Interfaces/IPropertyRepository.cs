using ManadaIA.Domain.Entities;

namespace ManadaIA.Domain.Interfaces;

public interface IPropertyRepository : IRepository<Property>
{
    Task<IReadOnlyList<Property>> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct = default);
    Task<IReadOnlyList<Property>> GetActiveAsync(Guid ownerId, CancellationToken ct = default);
}
