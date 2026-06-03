using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Orleans.MultiClusterClient;

internal sealed class MultiClusterClientBuilder(IServiceCollection services, IConfiguration configuration)
    : IMultiClusterClientBuilder
{
    private readonly Dictionary<String, RemoteClusterClientBuilder> _clientBuilders = new(StringComparer.OrdinalIgnoreCase);

    public void AddClient(String name, Action<IClientBuilder> configureDelegate)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        var clientBuilder = new RemoteClusterClientBuilder(name, configuration);
        configureDelegate(clientBuilder);
        _clientBuilders.Add(name, clientBuilder);
    }

    public void Build()
    {
        var provider = new MultiClusterProvider(_clientBuilders);
        services.TryAddSingleton<IMultiClusterClient>(provider);
        services.TryAddSingleton<IMultiClusterProvider>(provider);
        foreach (var name in _clientBuilders.Keys)
        {
            services.TryAddKeyedSingleton(name, (sp, key) => sp.GetRequiredService<IMultiClusterProvider>()[(String)key!]);
        }
        _ = services.AddHostedService(sp => provider);
    }
}
