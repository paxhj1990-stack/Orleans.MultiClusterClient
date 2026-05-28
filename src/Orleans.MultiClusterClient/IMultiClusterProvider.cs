namespace Orleans.MultiClusterClient;

/// <summary>
/// Defines an interface for a provider of multiple cluster clients. This allows for managing and accessing multiple cluster clients by name, enabling applications to interact with multiple Orleans clusters seamlessly.
/// </summary>
public interface IMultiClusterProvider
{
    /// <summary>
    /// Gets the cluster client with the specified name. This allows applications to retrieve and interact with different cluster clients based on their names, facilitating communication with multiple Orleans clusters within the same application context.
    /// </summary>
    /// <param name="name">The name of the cluster client.</param>
    /// <returns>The cluster client with the specified name.</returns>
    IClusterClient this[String name] { get; }
}
