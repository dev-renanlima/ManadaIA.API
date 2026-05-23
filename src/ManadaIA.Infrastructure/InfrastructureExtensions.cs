using ManadaIA.Domain.Interfaces;
using ManadaIA.Infrastructure.Repositories;
using ManadaIA.Infrastructure.Supabase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ManadaIA.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Supabase Settings
        var supabaseSettings = configuration
            .GetSection(SupabaseSettings.SectionName)
            .Get<SupabaseSettings>() ?? throw new InvalidOperationException(
                "Configuração do Supabase não encontrada");

        services.AddSingleton(supabaseSettings);

        // Supabase Client
        services.AddSingleton(sp =>
        {
            var settings = sp.GetRequiredService<SupabaseSettings>();
            return SupabaseClientFactory.CreateAsync(settings).GetAwaiter().GetResult();
        });

        // Repositories
        services.AddScoped<IAnimalRepository, AnimalRepository>();
        services.AddScoped<IReproductiveCycleRepository, ReproductiveCycleRepository>();
        services.AddScoped<IAIPredictionRepository, AIPredictionRepository>();

        return services;
    }
}
