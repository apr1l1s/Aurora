using System.Windows;
using System.Windows.Navigation;
using Aurora.EndPoints.Procyon.Services.Navigation;
using Microsoft.Extensions.DependencyInjection;
using Aurora.EndPoints.Procyon.Services.DependencyInjection;
using NavigationService = Aurora.EndPoints.Procyon.Services.Navigation.NavigationService;

namespace Aurora.EndPoints.Procyon;

public partial class App : Application
{
    private IServiceProvider Services { get; } = new ServiceCollection().ConfigureServices().BuildServiceProvider();

    protected override void OnStartup(StartupEventArgs e)
    {
        NavigationService.Configure(Services);

        var nav = new NavigationWindow {ShowsNavigationUI = false, Height = 300, Width = 300};
        NavigationService.SetMainWindow(nav);
        NavigationService.Navigate(PageType.Home);
        nav.Show();

        base.OnStartup(e);
    }

    
}