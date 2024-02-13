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
            }
        }

        private string? _Logradouro;
        public string? Logradouro
        {
            get => _Logradouro;
            set
            {
                _Logradouro = value;
                OnPropertyChanged();
            }
        }

        private string? _Bairro;
        public string? Bairro
        {
            get => _Bairro;
            set
            {
                _Bairro = value;
                OnPropertyChanged();
            }
        }

        private string? _Localidade;
        public string? Localidade
        {
            get => _Localidade;
            set
            {
                _Localidade = value;
                OnPropertyChanged();
            }
        }

        private string? _UF;
        public string? UF
        {
            get => _UF;
            set
            {
                _UF = value;
                OnPropertyChanged();
            }
        }

        private string? _DDD;
        public string? DDD
        {
            get => _DDD;
            set
            {
                _DDD = value;
                OnPropertyChanged();
            }
        }

        //public BuscaCepViewModel()
        //{

        //    _BuscarCommand = new Command()
        //}

        private Command _BuscarCommand;
        public Command BuscarCommand
            => _BuscarCommand ??= new Command(async () => await BuscarCommandExecute());

        //=> _BuscarCommand ?? (_BuscarCommand = new Command(async () => await BuscarCommandExecute()));

        //{
        //    get
        //    {
        //        if (_BuscarCommand is null)
        //            _BuscarCommand = new Command(async () => await BuscarCommandExecute());

        //        return _BuscarCommand;
        //    }
        //}

        private async Task BuscarCommandExecute()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(CEP))
                    throw new InvalidOperationException(message: "Você precisa informar o CEP");

                var viaCepResult = await new HttpClient()
                        .GetFromJsonAsync<ViaCepDto>(requestUri: $"https://viacep.com.br/ws/{CEP}/json/") ??
                        throw new InvalidOperationException(message: "Algo de errado não deu certo ao consultar o CEP");

                if (viaCepResult.erro)
                    throw new InvalidOperationException(message: "Algo de errado não deu certo ao consultar o CEP");

                Logradouro = viaCepResult.logradouro;
                Bairro = viaCepResult.bairro;
                Localidade = viaCepResult.localidade;
                UF = viaCepResult.uf;
                DDD = viaCepResult.ddd;
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}
