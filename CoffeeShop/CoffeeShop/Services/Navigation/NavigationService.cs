using CoffeeShop.Sections.Home;
using CoffeeShop.Sections.Welcome;

namespace CoffeeShop.Services.Navigation;

internal sealed class NavigationService
{
    static readonly Lazy<NavigationService> _Lazy = new(() => new());

    public static NavigationService Current => _Lazy.Value;

    readonly Dictionary<Type, Type> _Mappings;

    private NavigationService()
    {
        _Mappings = [];

        CreateViewModelMappings();
    }

    static INavigation Navigation { get => ((CustomNavigationPage)Application.Current!.MainPage!).Navigation; }

    void CreateViewModelMappings()
    {
        _Mappings.Add(typeof(WelcomePageViewModel), typeof(WelcomePage));
        _Mappings.Add(typeof(HomePageViewModel), typeof(HomePage));
    }

    public async Task Navigate<TViewModel>(object? parameter = null, bool animated = true)
        where TViewModel : BasePageViewModel
    {
        try
        {
            List<Page>? pagesToRemove = null;

            var viewModelType = typeof(TViewModel);

            var page = (Page)CreateAndBindPage(viewModelType);

            if (viewModelType == typeof(HomePageViewModel))
            {
                pagesToRemove = Navigation.NavigationStack?.ToList();
            }


            if (viewModelType.BaseType == typeof(BaseModalViewModel))
                await Navigation.PushModalAsync(page, animated);
            else
                await Navigation.PushAsync(page, animated);

            await ((BasePageViewModel)page.BindingContext).Initialize(parameter);

            if (pagesToRemove is not null)
            {
                foreach (var pageToRemove in pagesToRemove)
                {
                    Navigation.RemovePage(pageToRemove);
                }
            }
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private BindableObject CreateAndBindPage(Type viewModelType)
    {
        // Identifique qual é a página que está mapeada para esta ViewModel
        var pageType = _Mappings!.ContainsKey(viewModelType) ?
            _Mappings[viewModelType] :
            throw new KeyNotFoundException(message: "A ViewModel de destino não possui um mapeamento registrado");

        // Criar uma instância da página através do tipo da página
        var page = (BindableObject?)(Activator.CreateInstance(pageType))
            ?? throw new NullReferenceException(message: $"Não foi possível criar uma instância da página {pageType.Name}");

        // "Bindar" uma instância da minha ViewModel para a página instanciada
        page.BindingContext = Activator.CreateInstance(viewModelType) as BasePageViewModel;

        return page;
    }

    public void Initialize(object? parameters = null)
    {
        Application.Current!.MainPage = new CustomNavigationPage(
            VersionTracking.Default.IsFirstLaunchEver ?
            new WelcomePage() { BindingContext = new WelcomePageViewModel() } :
            new HomePage() { BindingContext = new HomePageViewModel() });

        MainThread.InvokeOnMainThreadAsync(async () => await ((BasePageViewModel)Navigation.NavigationStack[0].BindingContext).Initialize(parameters));
    }
}
