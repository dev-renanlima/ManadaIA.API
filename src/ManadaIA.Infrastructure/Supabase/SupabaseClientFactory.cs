using Supabase;

namespace ManadaIA.Infrastructure.Supabase;

public static class SupabaseClientFactory
{
    public static async Task<Client> CreateAsync(SupabaseSettings settings)
    {
        var options = new SupabaseOptions
        {
            AutoConnectRealtime = false,
            AutoRefreshToken = true
        };

        var client = new Client(
            settings.Url,
            settings.ServiceRoleKey,
            options
        );

        await client.InitializeAsync();

        return client;
    }
}
