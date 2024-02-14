using BuscaCep.ViewModels;

namespace BuscaCep.Views;

public partial class CepsPage : ContentPage
{
    public CepsPage()
    {
        InitializeComponent();

        BindingContext = new CepsViewModel();
    }
}