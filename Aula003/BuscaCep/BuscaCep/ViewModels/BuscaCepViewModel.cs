using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Net.Http.Json;

namespace BuscaCep.ViewModels
{
    sealed partial class BuscaCepViewModel : BaseViewModel
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(BuscarCommand))]
        string? _CEP;

        ViaCepDto? _dto = null;

        public string? Logradouro { get => _dto?.logradouro; }
        public string? Bairro { get => _dto?.bairro; }
        public string? Localidade { get => _dto?.localidade; }
        public string? UF { get => _dto?.uf; }
        public string? DDD { get => _dto?.ddd; }

        private bool BuscarCanExecute()
            => !string.IsNullOrWhiteSpace(CEP) &&
            CEP.Length == 8 &&
            IsNotBusy;

        [RelayCommand(CanExecute = nameof(BuscarCanExecute))]
        private async Task Buscar()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                BuscarCommand.NotifyCanExecuteChanged();

                _dto = await new HttpClient()
                        .GetFromJsonAsync<ViaCepDto>(requestUri: $"https://viacep.com.br/ws/{CEP}/json/") ??
                        throw new InvalidOperationException(message: "Algo de errado não deu certo ao consultar o CEP");

                if (_dto.erro)
                    throw new InvalidOperationException(message: "Algo de errado não deu certo ao consultar o CEP");

                WeakReferenceMessenger.Default.Send(_dto);
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ops", ex.Message, "OK");
            }
            finally
            {
                OnPropertyChanged(nameof(Logradouro));
                OnPropertyChanged(nameof(Bairro));
                OnPropertyChanged(nameof(Localidade));
                OnPropertyChanged(nameof(UF));
                OnPropertyChanged(nameof(DDD));

                IsBusy = false;
                BuscarCommand.NotifyCanExecuteChanged();
            }
        }
    }
}
