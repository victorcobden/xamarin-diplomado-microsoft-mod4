using DemoAzureOfflineSync.Models;
using DemoAzureOfflineSync.Services;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace DemoAzureOfflineSync.ViewModels
{
    public class DemoAzureOfflineSyncPageVM : ObservableBaseObject
    {
        #region private members
        private AzureClient _client = null;
        private bool isBusyProcessingData = false;
        private string pendingOperations = string.Empty;
        #endregion private members

        #region vm properties
        public ICommand RefreshDataCommand { get; set; }
        public ICommand GenerateDataCommand { get; set; }
        public ICommand CleanLocalDataCommand { get; set; }
        public ICommand SyncDataCommand { get; set; }
        public bool IsBusyProcessingData
        {
            get { return isBusyProcessingData; }
            set { isBusyProcessingData = value; OnPropertyChanged(); }
        }
        public ObservableCollection<Contact> Contacts { get; set; }
        public string PendingOperations
        {
            get { return pendingOperations; }
            set { pendingOperations = value; OnPropertyChanged(); }
        }
        #endregion vm properties

        #region constructor

        public DemoAzureOfflineSyncPageVM()
        {
            Contacts = new ObservableCollection<Contact>();
            _client = new AzureClient();
            _client.OnChangeEventCallback = new
           AzureClient.OnChangeEventDelegate(OnChangeEvent);
            GenerateDataCommand = new Command(() => GenerateData());
            RefreshDataCommand = new Command(() => RefreshData());
            SyncDataCommand = new Command(() => SyncData());
            CleanLocalDataCommand = new Command(() => CleanLocalData());
        }

        #endregion constructor

        public void OnChangeEvent(string value)
        {
            PendingOperations = value;
        }
        #region private methods
        private void GenerateData()

        {
            if (IsBusyProcessingData)
                return;
            IsBusyProcessingData = true;
            string[] names = { "José Luis", "Miguel Ángel", "José Francisco", "Jesús Antonio", "Jorge", "Alberto",
                                "Sofía", "Camila", "Valentina", "Isabella", "Ximena","Ana"};
            string[] lastNames = { "Hernández", "García", "Martínez", "López",
                                    "González", "Méndez", "Castillo", "Corona", "Cruz" };
            Random rnd = new Random(DateTime.Now.Millisecond);
            string name = $"{names[rnd.Next(0, 12)]} {lastNames[rnd.Next(0, 8)]}";
            string phone = string.Format($"{rnd.Next(55555, 55999)}-{rnd.Next(0000, 9999)}");
            var contact = new Contact()
            {
                Name = name,
                Phone = phone,
                IsLocal = true
            };
            _client.AddContactAsync(contact);
            IsBusyProcessingData = false;
            RefreshData();
        }
        private async void RefreshData()
        {
            Contacts.Clear();
            await _client.GetContactsAsync()
            .ContinueWith((b) =>
            {
                foreach (var item in b.Result)
                {
                    Contacts.Add(item);
                }
                IsBusyProcessingData = false;
            });
        }
        private async void SyncData()
        {
            IsBusyProcessingData = true;
            await _client.SyncAsync(false).ContinueWith((b) => { RefreshData(); });
        }
        private async void CleanLocalData()
        {
            await _client.CleanAsync()
            .ContinueWith((b) => { SyncData(); });
        }
        #endregion private methods
    }
}