namespace ProductsApi.Mvc.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class VersionedEndpointAttribute : Attribute
{
    public VersionedEndpointAttribute(string prefix)
    {
        this.Prefix = prefix;
    }

    public string Prefix { get; }
}