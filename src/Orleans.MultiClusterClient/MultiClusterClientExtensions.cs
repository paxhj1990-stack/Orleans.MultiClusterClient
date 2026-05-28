using Microsoft.Extensions.Hosting;

namespace Orleans.MultiClusterClient;

/// <summary>
/// Extension methods for setting up multi-cluster client services in an <see cref="IHostApplicationBuilder" />.
/// </summary>
public static class MultiClusterClientExtensions
{
    /// <summary>
    /// Adds multi-cluster client services to the specified <see cref="IHostApplicationBuilder" />. This method allows you to configure multiple cluster clients using the provided delegate.
    /// </summary>
    /// <param name="hostAppBuilder">The host application builder.</param>
    /// <param name="configureDelegate">The delegate to configure the multi-cluster client builder.</param>
    /// <returns>The host application builder.</returns>
    public static IHostApplicationBuilder UseMultiClusterClient(this IHostApplicationBuilder hostAppBuilder, Action<IMultiClusterClientBuilder> configureDelegate)
    {
        var multiClusterClientBuilder = new MultiClusterClientBuilder(hostAppBuilder.Services, hostAppBuilder.Configuration);
        configureDelegate(multiClusterClientBuilder);
        multiClusterClientBuilder.Build();
        return hostAppBuilder;
    }
}
