using System.Windows.Input;
using Aurora.EndPoints.Procyon.Commands;
using Aurora.EndPoints.Procyon.Providers;

namespace Aurora.EndPoints.Procyon.ViewModels;

public class MainViewModel : ViewModelBase
{
    private const string ZAPRET_PATH =
        "C:\\Users\\safronov.a\\Downloads\\zapret-discord-youtube-1.7.0\\general (ALT).bat";

    public MainViewModel()
    {
        Console.WriteLine(ZapretProvider.GetOutput());
    }
    
    public ICommand StartZapret { get; } = new RelayCommand(_ => ZapretProvider.Start(ZAPRET_PATH));

    public ICommand CloseZapret { get; } = new RelayCommand(_ => ZapretProvider.Stop());
}