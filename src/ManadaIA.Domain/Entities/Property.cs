namespace ManadaIA.Domain.Entities;

/// <summary>
/// Representa uma propriedade/fazenda
/// </summary>
public sealed class Property
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public decimal? AreaHectares { get; private set; }
    public string? Registration { get; private set; } // Inscrição estadual/CAR
    public bool IsActive { get; private set; }
    public Guid OwnerId { get; private set; } // Referência ao usuário proprietário
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // EF/ORM private constructor
    private Property() { }

    public static Property Create(
        string name,
        string city,
        string state,
        Guid ownerId,
        decimal? areaHectares = null,
        string? registration = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome é obrigatório", nameof(name));

        if (string.IsNullOrWhiteSpace(city))
            throw new ArgumentException("Cidade é obrigatória", nameof(city));

        if (string.IsNullOrWhiteSpace(state) || state.Length != 2)
            throw new ArgumentException("Estado deve ter 2 caracteres (UF)", nameof(state));

        if (areaHectares.HasValue && areaHectares.Value <= 0)
            throw new ArgumentException("Área deve ser maior que zero", nameof(areaHectares));

        return new Property
        {
            Id = Guid.NewGuid(),
            Name = name,
            City = city,
            State = state.ToUpper(),
            AreaHectares = areaHectares,
            Registration = registration,
            OwnerId = ownerId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, string city, string state, decimal? areaHectares, string? registration)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Nome é obrigatório", nameof(name));

        Name = name;
        City = city;
        State = state.ToUpper();
        AreaHectares = areaHectares;
        Registration = registration;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
