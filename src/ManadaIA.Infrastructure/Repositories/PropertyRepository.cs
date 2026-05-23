using ManadaIA.Domain.Entities;
using ManadaIA.Domain.Interfaces;
using ManadaIA.Infrastructure.Models;
using Supabase;

namespace ManadaIA.Infrastructure.Repositories;

public sealed class PropertyRepository(Client supabase) : IPropertyRepository
{
    public async Task<Property?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var result = await supabase
            .From<PropertyModel>()
            .Where(x => x.Id == id)
            .Single();

        if (result == null) return null;

        return Property.Create(
            result.Name,
            result.City,
            result.State,
            result.OwnerId,
            result.AreaHectares,
            result.Registration
        );
    }

    public async Task<IReadOnlyList<Property>> GetAllAsync(CancellationToken ct = default)
    {
        var result = await supabase
            .From<PropertyModel>()
            .Get();

        return result.Models.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyList<Property>> GetByOwnerIdAsync(Guid ownerId, CancellationToken ct = default)
    {
        var result = await supabase
            .From<PropertyModel>()
            .Where(x => x.OwnerId == ownerId)
            .Get();

        return result.Models.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyList<Property>> GetActiveAsync(Guid ownerId, CancellationToken ct = default)
    {
        var result = await supabase
            .From<PropertyModel>()
            .Where(x => x.OwnerId == ownerId)
            .Where(x => x.IsActive == true)
            .Get();

        return result.Models.Select(MapToDomain).ToList();
    }

    public async Task AddAsync(Property entity, CancellationToken ct = default)
    {
        var model = PropertyModel.FromDomain(entity);
        await supabase.From<PropertyModel>().Insert(model);
    }

    public async Task UpdateAsync(Property entity, CancellationToken ct = default)
    {
        var model = PropertyModel.FromDomain(entity);
        await supabase.From<PropertyModel>().Update(model);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await supabase
            .From<PropertyModel>()
            .Where(x => x.Id == id)
            .Delete();
    }

    private static Property MapToDomain(PropertyModel model)
    {
        return Property.Create(
            model.Name,
            model.City,
            model.State,
            model.OwnerId,
            model.AreaHectares,
            model.Registration
        );
    }
}
