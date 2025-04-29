using System.Windows.Input;
using Aurora.EndPoints.Procyon.Commands;
using Aurora.EndPoints.Procyon.Providers;

namespace Aurora.EndPoints.Procyon.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        Console.WriteLine(ZapretProvider.GetOutput());
    }
    
    public ICommand StartZapret { get; } = new RelayCommand(_ => ZapretProvider.Start());

    public ICommand CloseZapret { get; } = new RelayCommand(_ => ZapretProvider.Stop());
}