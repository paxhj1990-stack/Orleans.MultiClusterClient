using Microsoft.Extensions.DependencyInjection;

namespace Orleans.MultiClusterClient;

/// <summary>
/// A wrapper around an Orleans cluster client that allows for multiple cluster clients to be used within the same application.
/// </summary>
internal sealed class RemoteClusterClient : IClusterClient, IAsyncDisposable
{
    private readonly IClusterClient _clusterClient;

    public RemoteClusterClient(String name, IServiceCollection services)
    {
        Name = name;
        var serviceProvider = services.BuildServiceProvider();
        _clusterClient = serviceProvider.GetRequiredService<IClusterClient>();
    }

    public String Name { get; }

    /// <inheritdoc/>
    public IServiceProvider ServiceProvider => _clusterClient.ServiceProvider;

    /// <summary>
    /// Disposes the underlying cluster client. 
    /// If the underlying cluster client implements IAsyncDisposable, it will be disposed asynchronously. 
    /// If it only implements IDisposable, it will be disposed synchronously. 
    /// If it implements neither, this method will do nothing.
    /// </summary>
    /// <returns>A ValueTask representing the asynchronous dispose operation.</returns>
    public ValueTask DisposeAsync()
        => ServiceProvider switch
        {
            IAsyncDisposable asyncDisposable => asyncDisposable.DisposeAsync(),
            IDisposable disposable => FakeDisposeAsync(disposable),
            _ => default
        };

    private static ValueTask FakeDisposeAsync(IDisposable disposable)
    {
        disposable.Dispose();
        return default;
    }

    /// <inheritdoc/>
    public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithGuidKey
        => _clusterClient.GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
    /// <inheritdoc/>
    public TGrainInterface GetGrain<TGrainInterface>(Int64 primaryKey, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithIntegerKey
        => _clusterClient.GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
    /// <inheritdoc/>
    public TGrainInterface GetGrain<TGrainInterface>(String primaryKey, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithStringKey
        => _clusterClient.GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
    /// <inheritdoc/>
    public TGrainInterface GetGrain<TGrainInterface>(Guid primaryKey, String keyExtension, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithGuidCompoundKey => _clusterClient.GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
    /// <inheritdoc/>
    public TGrainInterface GetGrain<TGrainInterface>(Int64 primaryKey, String keyExtension, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithIntegerCompoundKey
        => _clusterClient.GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
    /// <inheritdoc/>
    public TGrainObserverInterface CreateObjectReference<TGrainObserverInterface>(IGrainObserver obj)
        where TGrainObserverInterface : IGrainObserver
        => _clusterClient.CreateObjectReference<TGrainObserverInterface>(obj);
    /// <inheritdoc/>
    public void DeleteObjectReference<TGrainObserverInterface>(IGrainObserver obj)
        where TGrainObserverInterface : IGrainObserver
        => _clusterClient.DeleteObjectReference<TGrainObserverInterface>(obj);
    /// <inheritdoc/>
    public IGrain GetGrain(Type grainInterfaceType, Guid grainPrimaryKey)
        => _clusterClient.GetGrain(grainInterfaceType, grainPrimaryKey);
    /// <inheritdoc/>
    public IGrain GetGrain(Type grainInterfaceType, Int64 grainPrimaryKey)
        => _clusterClient.GetGrain(grainInterfaceType, grainPrimaryKey);
    /// <inheritdoc/>
    public IGrain GetGrain(Type grainInterfaceType, String grainPrimaryKey)
        => _clusterClient.GetGrain(grainInterfaceType, grainPrimaryKey);
    /// <inheritdoc/>
    public IGrain GetGrain(Type grainInterfaceType, Guid grainPrimaryKey, String keyExtension)
        => _clusterClient.GetGrain(grainInterfaceType, grainPrimaryKey, keyExtension);
    /// <inheritdoc/>
    public IGrain GetGrain(Type grainInterfaceType, Int64 grainPrimaryKey, String keyExtension)
        => _clusterClient.GetGrain(grainInterfaceType, grainPrimaryKey, keyExtension);
    /// <inheritdoc/>
    public TGrainInterface GetGrain<TGrainInterface>(GrainId grainId) where TGrainInterface : IAddressable
        => _clusterClient.GetGrain<TGrainInterface>(grainId);
    /// <inheritdoc/>
    public IAddressable GetGrain(GrainId grainId)
        => _clusterClient.GetGrain(grainId);
    /// <inheritdoc/>
    public IAddressable GetGrain(GrainId grainId, GrainInterfaceType interfaceType)
        => _clusterClient.GetGrain(grainId, interfaceType);
    /// <inheritdoc/>
    public IAddressable GetGrain(Type interfaceType, IdSpan grainKey, String grainClassNamePrefix)
        => _clusterClient.GetGrain(interfaceType, grainKey, grainClassNamePrefix);
    /// <inheritdoc/>
    public IAddressable GetGrain(Type interfaceType, IdSpan grainKey)
        => _clusterClient.GetGrain(interfaceType, grainKey);
}
