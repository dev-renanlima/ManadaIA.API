using ManadaIA.Domain.Entities;

namespace ManadaIA.Domain.Interfaces;

public interface IVaccineRepository : IRepository<Vaccine>
{
    Task<IReadOnlyList<Vaccine>> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default);
    Task<IReadOnlyList<Vaccine>> GetUpcomingAsync(DateTime limitDate, CancellationToken ct = default);
}
