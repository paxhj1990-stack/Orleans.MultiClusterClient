using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Orleans.MultiClusterClient;

/// <summary>
/// Builder for configuring and creating a remote cluster client.
/// </summary>
internal sealed class RemoteClusterClientBuilder
    : ClientBuilder, IRemoteClusterClientBuilder
{
    public RemoteClusterClientBuilder(String name, IConfiguration configuration)
        : base(new ServiceCollection(), configuration)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name, nameof(name));
        Name = name;
    }

    public String Name { get; }

    public IClusterClient Build() => new RemoteClusterClient(Name, Services);
}
