using DemoAzureOfflineSync.Views;
using Xamarin.Forms;
using XamarinDiplomado.Participants.Startup;

namespace DemoAzureOfflineSync
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Startup startup = new Startup("Victor Hugo Cobden Roque Acuña","vhcobden@gmail.com", 1, 4);
            startup.Init();
            MainPage = new NavigationPage(new DemoAzureOfflineSyncPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}