namespace ManadaIA.Domain.Entities;

/// <summary>
/// Representa um registro de pesagem de animal
/// </summary>
public sealed class Weighing
{
    public Guid Id { get; private set; }
    public Guid AnimalId { get; private set; }
    public decimal Weight { get; private set; } // Em kg
    public DateTime WeighingDate { get; private set; }
    public string? Observations { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // EF/ORM private constructor
    private Weighing() { }

    public static Weighing Create(Guid animalId, decimal weight, DateTime weighingDate, string? observations = null)
    {
        if (weight <= 0)
            throw new ArgumentException("Peso deve ser maior que zero", nameof(weight));

        if (weighingDate > DateTime.UtcNow)
            throw new ArgumentException("Data da pesagem não pode ser futura", nameof(weighingDate));

        return new Weighing
        {
            Id = Guid.NewGuid(),
            AnimalId = animalId,
            Weight = weight,
            WeighingDate = weighingDate,
            Observations = observations,
            CreatedAt = DateTime.UtcNow
        };
    }
}
