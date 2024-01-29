namespace NLayer.ApiGateway.ConfigurationOptions;

public class AppSettings
{
    public string ProxyProvider { get; set; }

    public OcelotOptions Ocelot { get; set; }
}
