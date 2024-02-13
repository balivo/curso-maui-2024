using BuscaCep.ViewModels;
using System.Net.Http.Json;

namespace BuscaCep
{
    public partial class MainPage : ContentPage
    {
        BuscaCepViewModel ViewModel => (BuscaCepViewModel)BindingContext;

        public MainPage()
        {
            InitializeComponent();

            BindingContext = new BuscaCepViewModel();
        }

        private async void OnCounterClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ViewModel.CEP))
                    throw new InvalidOperationException(message: "Você precisa informar o CEP");

                lblLogradouro.Text =
                lblBairro.Text =
                lblLocalidade.Text =
                lblUf.Text =
                lblDDD.Text = "Carregando...";

                var viaCepResult = await new HttpClient()
                        .GetFromJsonAsync<ViaCepDto>(requestUri: $"https://viacep.com.br/ws/{ViewModel.CEP}/json/") ??
                        throw new InvalidOperationException(message: "Algo de errado não deu certo ao consultar o CEP");

                if (viaCepResult.erro)
                    throw new InvalidOperationException(message: "Algo de errado não deu certo ao consultar o CEP");

                lblLogradouro.Text = viaCepResult.logradouro;
                lblBairro.Text = viaCepResult.bairro;
                lblLocalidade.Text = viaCepResult.localidade;
                lblUf.Text = viaCepResult.uf;
                lblDDD.Text = viaCepResult.ddd;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }

    class ViaCepDto
    {
        public bool erro { get; set; }
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string localidade { get; set; }
        public string uf { get; set; }
        public string ibge { get; set; }
        public string gia { get; set; }
        public string ddd { get; set; }
        public string siafi { get; set; }
    }


}
