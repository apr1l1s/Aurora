using System.Windows.Controls;
using System.Windows.Navigation;
using Aurora.EndPoints.Procyon.ViewModels;
using Aurora.EndPoints.Procyon.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Aurora.EndPoints.Procyon.Services.Navigation;

public static class NavigationService
{
    private static readonly Dictionary<PageType, ContentControl> PageCache = new();
    private static IServiceProvider? _services;
    private static NavigationWindow? _mainWindow;

    public static void Configure(IServiceProvider services)
    {
        _services = services;
    }

    public static void SetMainWindow(NavigationWindow mainWindow)
    {
        _mainWindow = mainWindow;
    }

    public static void Navigate(PageType pageKey, bool useCache = true)
    {
        if (_mainWindow == null || _services == null)
            return;

        if (!useCache)
            PageCache.Remove(pageKey);

        if (!PageCache.TryGetValue(pageKey, out var targetPage))
        {
            targetPage = pageKey switch
            {
                PageType.Home => new HomeView { DataContext = _services.GetRequiredService<MainViewModel>() },
                _ => null
            };

            if (targetPage != null && useCache)
                PageCache[pageKey] = targetPage;
        }

        if (targetPage != null)
            _mainWindow.Navigate(targetPage);
    }
}

public enum PageType
{
    Admin, User, Home
}