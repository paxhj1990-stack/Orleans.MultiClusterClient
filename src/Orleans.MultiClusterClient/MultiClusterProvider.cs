using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Frozen;

namespace Orleans.MultiClusterClient;

internal sealed class MultiClusterProvider(Dictionary<String, RemoteClusterClientBuilder> clientBuilders)
    : IMultiClusterProvider, IHostedService, IMultiClusterClient
{
    private Object _obj = clientBuilders;

    /// <summary>
    /// Gets the cluster client with the specified name.
    /// </summary>
    /// <param name="name">The name of the cluster client.</param>
    /// <returns>The cluster client with the specified name.</returns>
    public IClusterClient this[String name]
    {
        get
        {
            if(_obj is FrozenDictionary<String, IClusterClient> clients)
            {
                return clients[name];
            }
            throw new InvalidOperationException("The multi-cluster provider has not been started.");
        }
    }

    async Task IHostedService.StartAsync(CancellationToken cancellationToken)
    {
        var dic = (Dictionary<String, RemoteClusterClientBuilder>)_obj;
        var clientDic = new Dictionary<String, IClusterClient>(dic.Count, StringComparer.OrdinalIgnoreCase);
        foreach (var builder in dic.Values)
        {
            clientDic[builder.Name] = builder.Build();
        }

        foreach (var client in clientDic.Values)
        {
            var hostedServices = client.ServiceProvider.GetServices<IHostedService>();
            foreach (var service in hostedServices)
            {
                await service.StartAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        _obj = clientDic.ToFrozenDictionary(StringComparer.OrdinalIgnoreCase);
    }

    async Task IHostedService.StopAsync(CancellationToken cancellationToken)
    {
        if (_obj is not FrozenDictionary<String, IClusterClient> clients)
        {
            return;
        }

        foreach (var client in clients.Values)
        {
            var hostedServices = client.ServiceProvider.GetServices<IHostedService>();
            foreach (var service in hostedServices)
            {
                await service.StopAsync(cancellationToken).ConfigureAwait(false);
            }
        }
    }

    public TGrainInterface GetGrain<TGrainInterface>(String clusterName, Guid primaryKey, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithGuidKey
        => this[clusterName].GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
    public TGrainInterface GetGrain<TGrainInterface>(String clusterName, Int64 primaryKey, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithIntegerKey
        => this[clusterName].GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
    public TGrainInterface GetGrain<TGrainInterface>(String clusterName, String primaryKey, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithStringKey
        => this[clusterName].GetGrain<TGrainInterface>(primaryKey, grainClassNamePrefix);
    public TGrainInterface GetGrain<TGrainInterface>(String clusterName, Guid primaryKey, String keyExtension, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithGuidCompoundKey
        => this[clusterName].GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
    public TGrainInterface GetGrain<TGrainInterface>(String clusterName, Int64 primaryKey, String keyExtension, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithIntegerCompoundKey
        => this[clusterName].GetGrain<TGrainInterface>(primaryKey, keyExtension, grainClassNamePrefix);
    public TGrainObserverInterface CreateObjectReference<TGrainObserverInterface>(String clusterName, IGrainObserver obj)
        where TGrainObserverInterface : IGrainObserver
        => this[clusterName].CreateObjectReference<TGrainObserverInterface>(obj);
    public void DeleteObjectReference<TGrainObserverInterface>(String clusterName, IGrainObserver obj)
        where TGrainObserverInterface : IGrainObserver
        => this[clusterName].DeleteObjectReference<TGrainObserverInterface>(obj);
    public IGrain GetGrain(String clusterName, Type grainInterfaceType, Guid grainPrimaryKey)
        => this[clusterName].GetGrain(grainInterfaceType, grainPrimaryKey);
    public IGrain GetGrain(String clusterName, Type grainInterfaceType, Int64 grainPrimaryKey)
        => this[clusterName].GetGrain(grainInterfaceType, grainPrimaryKey);
    public IGrain GetGrain(String clusterName, Type grainInterfaceType, String grainPrimaryKey)
        => this[clusterName].GetGrain(grainInterfaceType, grainPrimaryKey);
    public IGrain GetGrain(String clusterName, Type grainInterfaceType, Guid grainPrimaryKey, String keyExtension)
        => this[clusterName].GetGrain(grainInterfaceType, grainPrimaryKey, keyExtension);
    public IGrain GetGrain(String clusterName, Type grainInterfaceType, Int64 grainPrimaryKey, String keyExtension)
        => this[clusterName].GetGrain(grainInterfaceType, grainPrimaryKey, keyExtension);
    public TGrainInterface GetGrain<TGrainInterface>(String clusterName, GrainId grainId)
        where TGrainInterface : IAddressable
        => this[clusterName].GetGrain<TGrainInterface>(grainId);
    public IAddressable GetGrain(String clusterName, GrainId grainId)
        => this[clusterName].GetGrain(grainId);
    public IAddressable GetGrain(String clusterName, GrainId grainId, GrainInterfaceType interfaceType)
        => this[clusterName].GetGrain(grainId, interfaceType);
    public IAddressable GetGrain(String clusterName, Type interfaceType, IdSpan grainKey, String grainClassNamePrefix)
        => this[clusterName].GetGrain(interfaceType, grainKey, grainClassNamePrefix);
    public IAddressable GetGrain(String clusterName, Type interfaceType, IdSpan grainKey)
        => this[clusterName].GetGrain(interfaceType, grainKey);
}
