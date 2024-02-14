using CoffeeShop.Sections.Home;
using CommunityToolkit.Mvvm.Input;

namespace CoffeeShop.Sections.Welcome;

internal sealed partial class WelcomePageViewModel : BasePageViewModel
{
    [RelayCommand]
    Task GetStarted() => Navigate<HomePageViewModel>();
}
