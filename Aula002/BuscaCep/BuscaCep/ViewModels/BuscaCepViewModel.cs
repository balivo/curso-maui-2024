using System.Net.Http.Json;

namespace BuscaCep.ViewModels
{
    sealed class BuscaCepViewModel : BaseViewModel
    {
        private string? _CEP;
        public string? CEP
        {
            get => _CEP;
            set
            {
                _CEP = value;
                OnPropertyChanged();
                BuscarCommand.ChangeCanExecute();
            }
        }

        ViaCepDto? _dto = null;

        public string? Logradouro { get => _dto?.logradouro; }
        public string? Bairro { get => _dto?.bairro; }
        public string? Localidade { get => _dto?.localidade; }
        public string? UF { get => _dto?.uf; }
        public string? DDD { get => _dto?.ddd; }

        private Command _BuscarCommand;
        public Command BuscarCommand
            => _BuscarCommand ??= new Command(async () => await BuscarCommandExecute(), () => BuscarCommandCanExecute());

        private bool BuscarCommandCanExecute()
            => !string.IsNullOrWhiteSpace(CEP) &&
            CEP.Length == 8 &&
            IsNotBusy;

        private async Task BuscarCommandExecute()
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;
                BuscarCommand.ChangeCanExecute();

                _dto = await new HttpClient()
                        .GetFromJsonAsync<ViaCepDto>(requestUri: $"https://viacep.com.br/ws/{CEP}/json/") ??
                        throw new InvalidOperationException(message: "Algo de errado não deu certo ao consultar o CEP");

                if (_dto.erro)
                    throw new InvalidOperationException(message: "Algo de errado não deu certo ao consultar o CEP");
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
                BuscarCommand.ChangeCanExecute();
            }
        }
    }
}
