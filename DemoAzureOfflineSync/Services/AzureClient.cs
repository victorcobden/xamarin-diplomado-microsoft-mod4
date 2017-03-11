using DemoAzureOfflineSync.Models;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DemoAzureOfflineSync.Services
{
    public class AzureClient
    {
        private IMobileServiceClient _client;
        private IMobileServiceSyncTable<Contact> _table;
        private const string dbPath = "ContactsDB";
        private const string serviceUri = "http://intermedio-Xamarin-mod4mobiledevops.azurewebsites.net/";
        #region public delegates
        public delegate void OnChangeEventDelegate(string value);
        public OnChangeEventDelegate OnChangeEventCallback;
        #endregion public delegates
        public AzureClient()
        {
            Setup();
        }
        #region private methods
        private async void Setup()
        {
            _client = new MobileServiceClient(serviceUri);
            var store = new MobileServiceSQLiteStore(dbPath);
            store.DefineTable<Contact>();
            await _client.SyncContext.InitializeAsync(store).ContinueWith((b) =>
            {
                SetNotification(GetPendingOperations());
            });
            _table = _client.GetSyncTable<Contact>();
        }
        private void SetNotification(string value)
        {
            OnChangeEventCallback(value);
        }
        #endregion private methods
        #region public methods
        public async Task<IEnumerable<Contact>> GetContactsAsync()
        {
            var empty = new Contact[0];
            try
            {
                return await _table.ToEnumerableAsync();
            }
            catch
            {
                return empty;
            }
        }
        public async Task<IEnumerable<Contact>> GetContactsTableAsync()
        {
            return await _client.GetTable<Contact>().ToListAsync();
        }

        public async void AddContactAsync(Contact contact)
        {
            await _table.InsertAsync(contact)
            .ContinueWith((b) => { SetNotification(GetPendingOperations()); });
        }
        public async Task SyncAsync(bool ignoreConnectivity)
        {
            if (ignoreConnectivity)
            {
                await ProcessSync();
            }
            else
            {
                if (!Plugin.Connectivity.CrossConnectivity.Current.IsConnected)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await
                       App.Current.MainPage.DisplayAlert("DemoAzureOfflineSync", "Es necesario que actives tu red para sincronizar información.", "Aceptar");
                    });
                }
                else
                {
                    await ProcessSync();
                }
            }
        }
        private async Task ProcessSync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            try
            {
                await _table.Where(t => t.IsLocal).ToListAsync().ContinueWith(async
               (b) =>
                {
                    foreach (Contact c in b.Result)
                    {
                        c.IsLocal = false;
                        await _table.UpdateAsync(c);
                    }
                });
                await _client.SyncContext.PushAsync()
                .ContinueWith((b) =>
                {
                    SetNotification(GetPendingOperations());
                });
                await _table.PullAsync("allContacts", _table.CreateQuery());
            }
            catch (MobileServicePushFailedException pushEx)
            {
                if (pushEx.PushResult != null)
                    syncErrors = pushEx.PushResult.Errors;
            }
        }
        public async Task DeleteContactCloud(string id)
        {
            Contact c = await _client.GetTable<Contact>().LookupAsync(id);
            await _client.GetTable<Contact>().DeleteAsync(c);
        }
        public async Task CleanAsync()
        {
            await _table.PurgeAsync("allContacts", string.Empty, true, new
           System.Threading.CancellationToken())
            .ContinueWith((b) => { SetNotification(GetPendingOperations()); });
        }
        public string GetPendingOperations()
        {
            return string.Format("Operaciones Pendientes Por Enviar: {0}",
           _client.SyncContext.PendingOperations.ToString());
        }
        #endregion public methods
    }
}
