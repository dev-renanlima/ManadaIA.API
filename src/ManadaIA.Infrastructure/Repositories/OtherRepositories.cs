using ManadaIA.Domain.Entities;
using ManadaIA.Domain.Interfaces;
using ManadaIA.Infrastructure.Models;
using Supabase;

namespace ManadaIA.Infrastructure.Repositories;

public sealed class BatchRepository(Client supabase) : IBatchRepository
{
    public async Task<Batch?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var result = await supabase
            .From<BatchModel>()
            .Where(x => x.Id == id)
            .Single();

        return result != null ? Batch.Create(result.Name, result.PropertyId, result.Description) : null;
    }

    public async Task<IReadOnlyList<Batch>> GetAllAsync(CancellationToken ct = default)
    {
        var result = await supabase.From<BatchModel>().Get();
        return result.Models.Select(m => Batch.Create(m.Name, m.PropertyId, m.Description)).ToList();
    }

    public async Task<IReadOnlyList<Batch>> GetByPropertyIdAsync(Guid propertyId, CancellationToken ct = default)
    {
        var result = await supabase
            .From<BatchModel>()
            .Where(x => x.PropertyId == propertyId)
            .Get();
        return result.Models.Select(m => Batch.Create(m.Name, m.PropertyId, m.Description)).ToList();
    }

    public async Task<IReadOnlyList<Batch>> GetActiveAsync(Guid propertyId, CancellationToken ct = default)
    {
        var result = await supabase
            .From<BatchModel>()
            .Where(x => x.PropertyId == propertyId)
            .Where(x => x.IsActive == true)
            .Get();
        return result.Models.Select(m => Batch.Create(m.Name, m.PropertyId, m.Description)).ToList();
    }

    public async Task AddAsync(Batch entity, CancellationToken ct = default)
    {
        var model = BatchModel.FromDomain(entity);
        await supabase.From<BatchModel>().Insert(model);
    }

    public async Task UpdateAsync(Batch entity, CancellationToken ct = default)
    {
        var model = BatchModel.FromDomain(entity);
        await supabase.From<BatchModel>().Update(model);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await supabase.From<BatchModel>().Where(x => x.Id == id).Delete();
    }
}

public sealed class WeighingRepository(Client supabase) : IWeighingRepository
{
    public async Task<Weighing?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var result = await supabase.From<WeighingModel>().Where(x => x.Id == id).Single();
        return result != null ? Weighing.Create(result.AnimalId, result.Weight, result.WeighingDate, result.Observations) : null;
    }

    public async Task<IReadOnlyList<Weighing>> GetAllAsync(CancellationToken ct = default)
    {
        var result = await supabase.From<WeighingModel>().Get();
        return result.Models.Select(m => Weighing.Create(m.AnimalId, m.Weight, m.WeighingDate, m.Observations)).ToList();
    }

    public async Task<IReadOnlyList<Weighing>> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
    {
        var result = await supabase.From<WeighingModel>().Where(x => x.AnimalId == animalId).Get();
        return result.Models.Select(m => Weighing.Create(m.AnimalId, m.Weight, m.WeighingDate, m.Observations)).ToList();
    }

    public async Task<Weighing?> GetLastWeighingAsync(Guid animalId, CancellationToken ct = default)
    {
        var result = await supabase
            .From<WeighingModel>()
            .Where(x => x.AnimalId == animalId)
            .Order("data_pesagem", Postgrest.Constants.Ordering.Descending)
            .Limit(1)
            .Get();
        
        var model = result.Models.FirstOrDefault();
        return model != null ? Weighing.Create(model.AnimalId, model.Weight, model.WeighingDate, model.Observations) : null;
    }

    public async Task AddAsync(Weighing entity, CancellationToken ct = default)
    {
        var model = WeighingModel.FromDomain(entity);
        await supabase.From<WeighingModel>().Insert(model);
    }

    public async Task UpdateAsync(Weighing entity, CancellationToken ct = default)
    {
        var model = WeighingModel.FromDomain(entity);
        await supabase.From<WeighingModel>().Update(model);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await supabase.From<WeighingModel>().Where(x => x.Id == id).Delete();
    }
}

public sealed class VaccineRepository(Client supabase) : IVaccineRepository
{
    public async Task<Vaccine?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var result = await supabase.From<VaccineModel>().Where(x => x.Id == id).Single();
        return result != null 
            ? Vaccine.Create(result.AnimalId, result.VaccineName, result.ApplicationDate, result.NextDoseDate, result.Batch, result.Veterinarian, result.Observations)
            : null;
    }

    public async Task<IReadOnlyList<Vaccine>> GetAllAsync(CancellationToken ct = default)
    {
        var result = await supabase.From<VaccineModel>().Get();
        return result.Models.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyList<Vaccine>> GetByAnimalIdAsync(Guid animalId, CancellationToken ct = default)
    {
        var result = await supabase.From<VaccineModel>().Where(x => x.AnimalId == animalId).Get();
        return result.Models.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyList<Vaccine>> GetUpcomingAsync(DateTime limitDate, CancellationToken ct = default)
    {
        var result = await supabase
            .From<VaccineModel>()
            .Where(x => x.NextDoseDate <= limitDate)
            .Get();
        return result.Models.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(Vaccine entity, CancellationToken ct = default)
    {
        var model = VaccineModel.FromDomain(entity);
        await supabase.From<VaccineModel>().Insert(model);
    }

    public async Task UpdateAsync(Vaccine entity, CancellationToken ct = default)
    {
        var model = VaccineModel.FromDomain(entity);
        await supabase.From<VaccineModel>().Update(model);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await supabase.From<VaccineModel>().Where(x => x.Id == id).Delete();
    }

    private static Vaccine MapToDomain(VaccineModel m) =>
        Vaccine.Create(m.AnimalId, m.VaccineName, m.ApplicationDate, m.NextDoseDate, m.Batch, m.Veterinarian, m.Observations);
}
