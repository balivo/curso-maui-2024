using BuscaCep.Views;

namespace BuscaCep
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new MainPage();
            MainPage = new BuscaCepPage();
        }
    }
}
