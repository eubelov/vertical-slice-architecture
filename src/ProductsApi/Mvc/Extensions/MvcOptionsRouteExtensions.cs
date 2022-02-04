using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

using ProductsApi.Mvc.Attributes;

namespace ProductsApi.Mvc.Extensions;

public static class MvcOptionsRouteExtensions
{
    public static void UseGlobalRoutePrefix(this MvcOptions opts, IRouteTemplateProvider routeAttribute)
    {
        if (routeAttribute is null)
        {
            throw new ArgumentNullException(nameof(routeAttribute));
        }

        opts.Conventions.Add(new GlobalRouteConvention(routeAttribute));
    }

    public static void UseGlobalRoutePrefix(this MvcOptions opts, string prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix))
        {
            throw new ArgumentException($"{nameof(prefix)} cannot be empty", nameof(prefix));
        }

        opts.UseGlobalRoutePrefix(new RouteAttribute(prefix));
    }
}

public class GlobalRouteConvention : IApplicationModelConvention
{
    private readonly AttributeRouteModel routePrefix;

    public GlobalRouteConvention(IRouteTemplateProvider route)
    {
        if (route is null)
        {
            throw new ArgumentNullException(nameof(route));
        }

        this.routePrefix = new(route);
    }

    public void Apply(ApplicationModel application)
    {
        foreach (var selector in application.Controllers.Where(x => x.Attributes.Any(attr => attr is VersionedEndpointAttribute))
                     .SelectMany(c => c.Selectors))
        {
            selector.AttributeRouteModel = selector.AttributeRouteModel != null
                                               ? AttributeRouteModel.CombineAttributeRouteModel(
                                                   this.routePrefix,
                                                   selector.AttributeRouteModel)
                                               : this.routePrefix;
        }
    }
}