using Aurora.EndPoints.Procyon.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace Aurora.EndPoints.Procyon.Services.DependencyInjection;

public static class ServiceConfiguration
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
        => services
            .AddSingleton<MainViewModel>();
}