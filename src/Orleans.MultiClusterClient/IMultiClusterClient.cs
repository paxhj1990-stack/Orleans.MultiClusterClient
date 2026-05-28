namespace Orleans.MultiClusterClient;

/// <summary>
/// Defines an interface for a multi-cluster client that allows interaction with multiple Orleans clusters. This interface provides methods for retrieving grain references and managing grain observers across different clusters.
/// </summary>
public interface IMultiClusterClient
{
    /// <summary>
    /// Gets a grain reference of the specified grain interface type in the given cluster using a Guid primary key.
    /// </summary>
    /// <typeparam name="TGrainInterface">The grain interface type. Must implement <see cref="IGrainWithGuidKey"/>.</typeparam>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="primaryKey">The grain's Guid primary key.</param>
    /// <param name="grainClassNamePrefix">Optional grain class name prefix to disambiguate implementations.</param>
    /// <returns>A grain reference of the specified interface type.</returns>
    TGrainInterface GetGrain<TGrainInterface>(String clusterName, Guid primaryKey, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithGuidKey;

    /// <summary>
    /// Gets a grain reference of the specified grain interface type in the given cluster using a long primary key.
    /// </summary>
    /// <typeparam name="TGrainInterface">The grain interface type. Must implement <see cref="IGrainWithIntegerKey"/>.</typeparam>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="primaryKey">The grain's long primary key.</param>
    /// <param name="grainClassNamePrefix">Optional grain class name prefix to disambiguate implementations.</param>
    /// <returns>A grain reference of the specified interface type.</returns>
    TGrainInterface GetGrain<TGrainInterface>(String clusterName, Int64 primaryKey, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithIntegerKey;

    /// <summary>
    /// Gets a grain reference of the specified grain interface type in the given cluster using a string primary key.
    /// </summary>
    /// <typeparam name="TGrainInterface">The grain interface type. Must implement <see cref="IGrainWithStringKey"/>.</typeparam>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="primaryKey">The grain's string primary key.</param>
    /// <param name="grainClassNamePrefix">Optional grain class name prefix to disambiguate implementations.</param>
    /// <returns>A grain reference of the specified interface type.</returns>
    TGrainInterface GetGrain<TGrainInterface>(String clusterName, String primaryKey, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithStringKey;

    /// <summary>
    /// Gets a reference to a compound-key grain in the given cluster using a Guid primary key and a key extension.
    /// </summary>
    /// <typeparam name="TGrainInterface">The grain interface type. Must implement <see cref="IGrainWithGuidCompoundKey"/>.</typeparam>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="primaryKey">The grain's Guid primary key.</param>
    /// <param name="keyExtension">The extension part of the compound key.</param>
    /// <param name="grainClassNamePrefix">Optional grain class name prefix to disambiguate implementations.</param>
    /// <returns>A grain reference of the specified interface type.</returns>
    TGrainInterface GetGrain<TGrainInterface>(String clusterName, Guid primaryKey, String keyExtension, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithGuidCompoundKey;

    /// <summary>
    /// Gets a reference to a compound-key grain in the given cluster using a long primary key and a key extension.
    /// </summary>
    /// <typeparam name="TGrainInterface">The grain interface type. Must implement <see cref="IGrainWithIntegerCompoundKey"/>.</typeparam>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="primaryKey">The grain's long primary key.</param>
    /// <param name="keyExtension">The extension part of the compound key.</param>
    /// <param name="grainClassNamePrefix">Optional grain class name prefix to disambiguate implementations.</param>
    /// <returns>A grain reference of the specified interface type.</returns>
    TGrainInterface GetGrain<TGrainInterface>(String clusterName, Int64 primaryKey, String keyExtension, String? grainClassNamePrefix = null)
        where TGrainInterface : IGrainWithIntegerCompoundKey;

    /// <summary>
    /// Creates an object reference to a local grain observer for the specified cluster so remote grains can call back to it.
    /// </summary>
    /// <typeparam name="TGrainObserverInterface">The observer interface type. Must implement <see cref="IGrainObserver"/>.</typeparam>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="obj">The local observer object to create a reference for.</param>
    /// <returns>An object reference to the local observer of type <typeparamref name="TGrainObserverInterface"/>.</returns>
    TGrainObserverInterface CreateObjectReference<TGrainObserverInterface>(String clusterName, IGrainObserver obj)
        where TGrainObserverInterface : IGrainObserver;

    /// <summary>
    /// Deletes a previously created observer object reference for the specified cluster.
    /// </summary>
    /// <typeparam name="TGrainObserverInterface">The observer interface type. Must implement <see cref="IGrainObserver"/>.</typeparam>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="obj">The local observer object whose reference should be deleted.</param>
    void DeleteObjectReference<TGrainObserverInterface>(String clusterName, IGrainObserver obj)
        where TGrainObserverInterface : IGrainObserver;

    /// <summary>
    /// Gets a non-generic <see cref="IGrain"/> reference in the given cluster for the specified interface type and Guid primary key.
    /// </summary>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="grainInterfaceType">The grain interface type.</param>
    /// <param name="grainPrimaryKey">The grain's Guid primary key.</param>
    /// <returns>A non-generic <see cref="IGrain"/> reference.</returns>
    IGrain GetGrain(String clusterName, Type grainInterfaceType, Guid grainPrimaryKey);

    /// <summary>
    /// Gets a non-generic <see cref="IGrain"/> reference in the given cluster for the specified interface type and long primary key.
    /// </summary>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="grainInterfaceType">The grain interface type.</param>
    /// <param name="grainPrimaryKey">The grain's long primary key.</param>
    /// <returns>A non-generic <see cref="IGrain"/> reference.</returns>
    IGrain GetGrain(String clusterName, Type grainInterfaceType, Int64 grainPrimaryKey);

    /// <summary>
    /// Gets a non-generic <see cref="IGrain"/> reference in the given cluster for the specified interface type and string primary key.
    /// </summary>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="grainInterfaceType">The grain interface type.</param>
    /// <param name="grainPrimaryKey">The grain's string primary key.</param>
    /// <returns>A non-generic <see cref="IGrain"/> reference.</returns>
    IGrain GetGrain(String clusterName, Type grainInterfaceType, String grainPrimaryKey);

    /// <summary>
    /// Gets a non-generic <see cref="IGrain"/> reference in the given cluster for a compound-key grain using a Guid primary key and key extension.
    /// </summary>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="grainInterfaceType">The grain interface type.</param>
    /// <param name="grainPrimaryKey">The grain's Guid primary key.</param>
    /// <param name="keyExtension">The extension part of the compound key.</param>
    /// <returns>A non-generic <see cref="IGrain"/> reference.</returns>
    IGrain GetGrain(String clusterName, Type grainInterfaceType, Guid grainPrimaryKey, String keyExtension);

    /// <summary>
    /// Gets a non-generic <see cref="IGrain"/> reference in the given cluster for a compound-key grain using a long primary key and key extension.
    /// </summary>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="grainInterfaceType">The grain interface type.</param>
    /// <param name="grainPrimaryKey">The grain's long primary key.</param>
    /// <param name="keyExtension">The extension part of the compound key.</param>
    /// <returns>A non-generic <see cref="IGrain"/> reference.</returns>
    IGrain GetGrain(String clusterName, Type grainInterfaceType, Int64 grainPrimaryKey, String keyExtension);

    /// <summary>
    /// Gets an <see cref="IAddressable"/> reference of the specified addressable interface type in the given cluster for the provided <see cref="GrainId"/>.
    /// </summary>
    /// <typeparam name="TGrainInterface">The expected addressable interface type. Must implement <see cref="IAddressable"/>.</typeparam>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="grainId">The GrainId identifier.</param>
    /// <returns>An addressable reference of the specified interface type.</returns>
    TGrainInterface GetGrain<TGrainInterface>(String clusterName, GrainId grainId) where TGrainInterface : IAddressable;

    /// <summary>
    /// Gets an <see cref="IAddressable"/> reference in the given cluster for the provided <see cref="GrainId"/>.
    /// </summary>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="grainId">The GrainId identifier.</param>
    /// <returns>An addressable grain reference.</returns>
    IAddressable GetGrain(String clusterName, GrainId grainId);

    /// <summary>
    /// Gets an <see cref="IAddressable"/> reference in the given cluster for the provided <see cref="GrainId"/> and interface type.
    /// </summary>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="grainId">The GrainId identifier.</param>
    /// <param name="interfaceType">The desired grain interface type.</param>
    /// <returns>An addressable grain reference.</returns>
    IAddressable GetGrain(String clusterName, GrainId grainId, GrainInterfaceType interfaceType);

    /// <summary>
    /// Gets an <see cref="IAddressable"/> reference in the given cluster for the specified interface type and raw key (<see cref="IdSpan"/>).
    /// An optional grain class name prefix may be provided to narrow the implementation.
    /// </summary>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="interfaceType">The grain interface type.</param>
    /// <param name="grainKey">The raw key (<see cref="IdSpan"/>).</param>
    /// <param name="grainClassNamePrefix">Optional grain class name prefix.</param>
    /// <returns>An addressable grain reference.</returns>
    IAddressable GetGrain(String clusterName, Type interfaceType, IdSpan grainKey, String grainClassNamePrefix);

    /// <summary>
    /// Gets an <see cref="IAddressable"/> reference in the given cluster for the specified interface type and raw key (<see cref="IdSpan"/>).
    /// </summary>
    /// <param name="clusterName">The name of the target cluster.</param>
    /// <param name="interfaceType">The grain interface type.</param>
    /// <param name="grainKey">The raw key (<see cref="IdSpan"/>).</param>
    /// <returns>An addressable grain reference.</returns>
    IAddressable GetGrain(String clusterName, Type interfaceType, IdSpan grainKey);
}
