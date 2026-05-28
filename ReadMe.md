```csharp
using Microsoft.Extensions.Hosting;
using Orleans.Configuration;
using StackExchange.Redis;
using System.Net;
using Orleans.MultiClusterClient;
using Microsoft.Extensions.DependencyInjection;
using LocalCluster.Contracts;

Console.WriteLine("Hello, Local Cluster!");

var host = Host.CreateApplicationBuilder(args);
host.UseOrleans(siloBuilder =>
{
	_ = siloBuilder.UseRedisClustering(opt => opt.ConfigurationOptions = ConfigurationOptions.Parse("127.0.0.1:6379,defaultDatabase=0"))
		.ConfigureEndpoints(IPAddress.Parse("127.0.0.1"), siloPort: 31111, gatewayPort: 31112)
		.Configure<ClusterOptions>(opt=>
		{
			opt.ServiceId = "local";
			opt.ClusterId = "local";
		});
});

host.UseMultiClusterClient(builder =>
{
	builder.AddClient("Remote1", cb =>
	{
		_ = cb.UseRedisClustering(opt => opt.ConfigurationOptions = ConfigurationOptions.Parse("127.0.0.1:6379,defaultDatabase=1"))
				.Configure<ClusterOptions>(opt =>
				{
					opt.ServiceId = "remote1";
					opt.ClusterId = "remote1";
				});
	});
	builder.AddClient("Remote2", cb =>
	{
		_ = cb.UseRedisClustering(opt => opt.ConfigurationOptions = ConfigurationOptions.Parse("127.0.0.1:6379,defaultDatabase=2"))
				.Configure<ClusterOptions>(opt =>
				{
					opt.ServiceId = "remote2";
					opt.ClusterId = "remote2";
				});
	});
});

var app = host.Build();
await app.StartAsync();
var grainFactory = app.Services.GetRequiredService<IGrainFactory>();
_ = await grainFactory.GetGrain<ILocalGrain>(2)
	.GetName();

Console.ReadKey();
await app.StopAsync();

```
```csharp
internal sealed class LocalGrain(IMultiClusterClient multiClusterClient) : Grain, ILocalGrain
{
    public async ValueTask<String> GetName()
    {
        var r1v = await multiClusterClient.GetGrain<IRemote2Grain>("remoteClusterName1", "grainKey1")
            .Get();
        var r2v = await multiClusterClient.GetGrain<IRemote1Grain>("remoteClusterName2", "grainKey2")
            .Get(r1v);
        return $"{r1v}/{r2v}";
    }
}
```