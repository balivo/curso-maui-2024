using BuscaCep.ViewModels;

namespace BuscaCep.Views;

public partial class BuscaCepPage : ContentPage
{
    public BuscaCepPage()
    {
        InitializeComponent();

        BindingContext = new BuscaCepViewModel();
    }
}