using ManadaIA.Domain.Entities;
using Postgrest.Attributes;
using Postgrest.Models;

namespace ManadaIA.Infrastructure.Models;

[Table("animais")]
public sealed class AnimalModel : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("brinco")]
    public string EarTag { get; set; } = string.Empty;

    [Column("nome")]
    public string Name { get; set; } = string.Empty;

    [Column("raca")]
    public string Breed { get; set; } = string.Empty;

    [Column("sexo")]
    public int Gender { get; set; }

    [Column("data_nascimento")]
    public DateTime BirthDate { get; set; }

    [Column("peso_atual")]
    public decimal? CurrentWeight { get; set; }

    [Column("status")]
    public int Status { get; set; }

    [Column("propriedade_id")]
    public Guid PropertyId { get; set; }

    [Column("lote_id")]
    public Guid? BatchId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public Animal ToDomain() => Animal.Create(
        EarTag,
        Name,
        Breed,
        (Gender)Gender,
        BirthDate,
        PropertyId,
        CurrentWeight
    );

    public static AnimalModel FromDomain(Animal animal) => new()
    {
        Id = animal.Id,
        EarTag = animal.EarTag,
        Name = animal.Name,
        Breed = animal.Breed,
        Gender = (int)animal.Gender,
        BirthDate = animal.BirthDate,
        CurrentWeight = animal.CurrentWeight,
        Status = (int)animal.Status,
        PropertyId = animal.PropertyId,
        BatchId = animal.BatchId,
        CreatedAt = animal.CreatedAt,
        UpdatedAt = animal.UpdatedAt
    };
}

[Table("propriedades")]
public sealed class PropertyModel : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("nome")]
    public string Name { get; set; } = string.Empty;

    [Column("cidade")]
    public string City { get; set; } = string.Empty;

    [Column("estado")]
    public string State { get; set; } = string.Empty;

    [Column("area_hectares")]
    public decimal? AreaHectares { get; set; }

    [Column("inscricao")]
    public string? Registration { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("proprietario_id")]
    public Guid OwnerId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public static PropertyModel FromDomain(Property property) => new()
    {
        Id = property.Id,
        Name = property.Name,
        City = property.City,
        State = property.State,
        AreaHectares = property.AreaHectares,
        Registration = property.Registration,
        IsActive = property.IsActive,
        OwnerId = property.OwnerId,
        CreatedAt = property.CreatedAt,
        UpdatedAt = property.UpdatedAt
    };
}

[Table("lotes")]
public sealed class BatchModel : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("nome")]
    public string Name { get; set; } = string.Empty;

    [Column("descricao")]
    public string? Description { get; set; }

    [Column("propriedade_id")]
    public Guid PropertyId { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    public static BatchModel FromDomain(Batch batch) => new()
    {
        Id = batch.Id,
        Name = batch.Name,
        Description = batch.Description,
        PropertyId = batch.PropertyId,
        IsActive = batch.IsActive,
        CreatedAt = batch.CreatedAt,
        UpdatedAt = batch.UpdatedAt
    };
}

[Table("pesagens")]
public sealed class WeighingModel : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("animal_id")]
    public Guid AnimalId { get; set; }

    [Column("peso")]
    public decimal Weight { get; set; }

    [Column("data_pesagem")]
    public DateTime WeighingDate { get; set; }

    [Column("observacoes")]
    public string? Observations { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    public static WeighingModel FromDomain(Weighing weighing) => new()
    {
        Id = weighing.Id,
        AnimalId = weighing.AnimalId,
        Weight = weighing.Weight,
        WeighingDate = weighing.WeighingDate,
        Observations = weighing.Observations,
        CreatedAt = weighing.CreatedAt
    };
}

[Table("vacinas")]
public sealed class VaccineModel : BaseModel
{
    [PrimaryKey("id", false)]
    public Guid Id { get; set; }

    [Column("animal_id")]
    public Guid AnimalId { get; set; }

    [Column("nome_vacina")]
    public string VaccineName { get; set; } = string.Empty;

    [Column("data_aplicacao")]
    public DateTime ApplicationDate { get; set; }

    [Column("data_proxima_dose")]
    public DateTime? NextDoseDate { get; set; }

    [Column("lote")]
    public string? Batch { get; set; }

    [Column("veterinario")]
    public string? Veterinarian { get; set; }

    [Column("observacoes")]
    public string? Observations { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    public static VaccineModel FromDomain(Vaccine vaccine) => new()
    {
        Id = vaccine.Id,
        AnimalId = vaccine.AnimalId,
        VaccineName = vaccine.VaccineName,
        ApplicationDate = vaccine.ApplicationDate,
        NextDoseDate = vaccine.NextDoseDate,
        Batch = vaccine.Batch,
        Veterinarian = vaccine.Veterinarian,
        Observations = vaccine.Observations,
        CreatedAt = vaccine.CreatedAt
    };
}
