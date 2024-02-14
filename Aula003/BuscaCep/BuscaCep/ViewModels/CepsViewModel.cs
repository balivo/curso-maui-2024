using BuscaCep.Views;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.ObjectModel;

namespace BuscaCep.ViewModels;

sealed partial class CepsViewModel : BaseViewModel
{
    public ObservableCollection<ViaCepDto> Ceps { get; private set; } = [];

    [RelayCommand]
    Task NovaBusca()
    {
        if (!WeakReferenceMessenger.Default.IsRegistered<ViaCepDto>(this))
            WeakReferenceMessenger.Default.Register<ViaCepDto>(this, ViaCepMessageHandler);

        return App.Current.MainPage.Navigation.PushAsync(new BuscaCepPage());
    }

    private void ViaCepMessageHandler(object recipient, ViaCepDto message)
    {
        var currentCep = Ceps.FirstOrDefault(lbda => lbda.cep == message.cep);

        if (currentCep is not null)
            Ceps.Remove(currentCep);

        Ceps.Add(message);
    }
}
