using ManadaIA.Domain.Entities;

namespace ManadaIA.Domain.Interfaces;

public interface IAnimalRepository : IRepository<Animal>
{
    Task<Animal?> GetByCodeAsync(string code, CancellationToken ct = default);
    Task<IReadOnlyList<Animal>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<IReadOnlyList<Animal>> GetBySpeciesAsync(Guid userId, Species species, CancellationToken ct = default);
}
