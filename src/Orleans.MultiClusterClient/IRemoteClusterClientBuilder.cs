namespace Orleans.MultiClusterClient;

/// <summary>
/// Interface for building remote cluster clients. This is used to abstract away the implementation details of how remote cluster clients are built and configured, allowing for different implementations to be used if necessary.
/// </summary>
public interface IRemoteClusterClientBuilder
{
    /// <summary>
    /// The name of the remote cluster client being built. This is used to identify the client when retrieving it from the multi-cluster provider.
    /// </summary>
    String Name { get; }

    /// <summary>
    /// Builds the remote cluster client using the configured services and returns it.
    /// </summary>
    /// <returns>The configured remote cluster client.</returns>
    IClusterClient Build();
}
