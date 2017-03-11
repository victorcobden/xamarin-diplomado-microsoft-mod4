using DemoAzureOfflineSync.ViewModels;
using Xamarin.Forms;

namespace DemoAzureOfflineSync.Views
{
    public partial class DemoAzureOfflineSyncPage : ContentPage
    {
        public DemoAzureOfflineSyncPage()
        {
            InitializeComponent();
            BindingContext = new DemoAzureOfflineSyncPageVM();
        }
    }
}