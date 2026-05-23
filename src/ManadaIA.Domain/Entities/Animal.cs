namespace ManadaIA.Domain.Entities;

/// <summary>
/// Representa um animal do rebanho
/// </summary>
public sealed class Animal
{
    public Guid Id { get; private set; }
    public string EarTag { get; private set; } = string.Empty; // Identificação do animal
    public string Name { get; private set; } = string.Empty;
    public string Breed { get; private set; } = string.Empty;
    public Gender Gender { get; private set; }
    public DateTime BirthDate { get; private set; }
    public decimal? CurrentWeight { get; private set; }
    public AnimalStatus Status { get; private set; }
    public Guid PropertyId { get; private set; }
    public Guid? BatchId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // EF/ORM private constructor
    private Animal() { }

    public static Animal Create(
        string earTag,
        string name,
        string breed,
        Gender gender,
        DateTime birthDate,
        Guid propertyId,
        decimal? initialWeight = null)
    {
        if (string.IsNullOrWhiteSpace(earTag))
            throw new ArgumentException("Brinco é obrigatório", nameof(earTag));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome é obrigatório", nameof(name));

        if (birthDate > DateTime.UtcNow)
            throw new ArgumentException("Data de nascimento não pode ser futura", nameof(birthDate));

        return new Animal
        {
            Id = Guid.NewGuid(),
            EarTag = earTag,
            Name = name,
            Breed = breed,
            Gender = gender,
            BirthDate = birthDate,
            CurrentWeight = initialWeight,
            Status = AnimalStatus.Active,
            PropertyId = propertyId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateWeight(decimal newWeight)
    {
        if (newWeight <= 0)
            throw new ArgumentException("Peso deve ser maior que zero", nameof(newWeight));

        CurrentWeight = newWeight;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AssignBatch(Guid batchId)
    {
        BatchId = batchId;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RemoveBatch()
    {
        BatchId = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = AnimalStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Sell()
    {
        Status = AnimalStatus.Sold;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkAsDead()
    {
        Status = AnimalStatus.Dead;
        UpdatedAt = DateTime.UtcNow;
    }
}

public enum Gender
{
    Male = 1,
    Female = 2
}

public enum AnimalStatus
{
    Active = 1,
    Sold = 2,
    Dead = 3,
    Inactive = 4
}
