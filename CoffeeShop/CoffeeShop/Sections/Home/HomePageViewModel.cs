using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Reflection.Metadata;

namespace CoffeeShop.Sections.Home;

internal sealed partial class HomePageViewModel : BasePageViewModel
{
    [ObservableProperty]
    HomeTabs _CurrentTab;

    [RelayCommand]
    void GoToTab(HomeTabs destinationTab)
    {
        if (CurrentTab == destinationTab)
            return;

        CurrentTab = destinationTab;

        switch (CurrentTab)
        {
            case HomeTabs.Home:
                InitializeHomeTab().SafeFireAndForget();
                break;
            case HomeTabs.Favorites:
                InitializeFavoritesTab().SafeFireAndForget();
                break;
            case HomeTabs.Notifications:
                InitializeNotificationsTab().SafeFireAndForget();
                break;
            case HomeTabs.Orders:
                InitializeOrdersTab().SafeFireAndForget();
                break;
            default:
                break;
        }
    }

    

    

    

    public override async Task Initialize(object? parameter = null)
    {
        try
        {
            await base.Initialize(parameter);

            switch (CurrentTab)
            {
                case HomeTabs.Home:
                    InitializeHomeTab(parameter).SafeFireAndForget();
                    break;
                case HomeTabs.Favorites:
                    InitializeFavoritesTab(parameter).SafeFireAndForget();
                    break;
                case HomeTabs.Notifications:
                    InitializeNotificationsTab(parameter).SafeFireAndForget();
                    break;
                case HomeTabs.Orders:
                    InitializeOrdersTab(parameter).SafeFireAndForget();
                    break;
                default:
                    break;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
}