namespace CoffeeShop.Services.Navigation;

public partial class CustomNavigationPage : NavigationPage
{
    public CustomNavigationPage() : base()
    {
        InitializeComponent();
    }

    public CustomNavigationPage(Page root) : base(root)
    {
        InitializeComponent();
    }
}