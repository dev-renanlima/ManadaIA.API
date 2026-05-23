namespace ManadaIA.Domain.Entities;

/// <summary>
/// Representa um registro de vacinação de animal
/// </summary>
public sealed class Vaccine
{
    public Guid Id { get; private set; }
    public Guid AnimalId { get; private set; }
    public string VaccineName { get; private set; } = string.Empty;
    public DateTime ApplicationDate { get; private set; }
    public DateTime? NextDoseDate { get; private set; }
    public string? Batch { get; private set; }
    public string? Veterinarian { get; private set; }
    public string? Observations { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // EF/ORM private constructor
    private Vaccine() { }

    public static Vaccine Create(
        Guid animalId,
        string vaccineName,
        DateTime applicationDate,
        DateTime? nextDoseDate = null,
        string? batch = null,
        string? veterinarian = null,
        string? observations = null)
    {
        if (string.IsNullOrWhiteSpace(vaccineName))
            throw new ArgumentException("Nome da vacina é obrigatório", nameof(vaccineName));

        if (applicationDate > DateTime.UtcNow)
            throw new ArgumentException("Data de aplicação não pode ser futura", nameof(applicationDate));

        if (nextDoseDate.HasValue && nextDoseDate.Value <= applicationDate)
            throw new ArgumentException("Data da próxima dose deve ser posterior à aplicação", nameof(nextDoseDate));

        return new Vaccine
        {
            Id = Guid.NewGuid(),
            AnimalId = animalId,
            VaccineName = vaccineName,
            ApplicationDate = applicationDate,
            NextDoseDate = nextDoseDate,
            Batch = batch,
            Veterinarian = veterinarian,
            Observations = observations,
            CreatedAt = DateTime.UtcNow
        };
    }
}
