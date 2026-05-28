namespace Orleans.MultiClusterClient;

/// <summary>
/// Defines a builder for configuring and building multiple Orleans clients for connecting to multiple clusters.
/// </summary>
public interface IMultiClusterClientBuilder
{
    /// <summary>
    /// Adds a client with the specified name and configuration delegate.
    /// </summary>
    /// <param name="name">The name of the client.</param>
    /// <param name="configureDelegate">The configuration delegate for the client.</param>
    void AddClient(String name, Action<IClientBuilder> configureDelegate);

    /// <summary>
    /// Builds the multi-cluster client configuration.
    /// </summary>
    void Build();
}
