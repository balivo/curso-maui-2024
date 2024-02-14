using CoffeeShop.Services.Navigation;

namespace CoffeeShop;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        NavigationService.Current.Initialize();
    }
}
