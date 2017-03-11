using DemoAzureOfflineSync.Models;
using DemoAzureOfflineSync.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoAzureOfflineSync.Tests
{
    [TestClass]
    public class AzureClientTest
    {
        [TestMethod]
        public async Task AddLocalContactAndPushToCloud()
        {
            //initialize elements.
            AzureClient _client = new AzureClient();
            _client.OnChangeEventCallback = new AzureClient.OnChangeEventDelegate((b)
           =>
            { });
            List<Contact> list = null;
            //get original data.
            await _client.GetContactsTableAsync()
            .ContinueWith((b) => { list = new List<Contact>(b.Result); });
            int original_count = list.Count;
            //create element.
            Contact c = new Contact()
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Guest",
                Phone = "23423",
                IsLocal = true
            };
            //add and update azure table.
            _client.AddContactAsync(c);
            await _client.SyncAsync(true);
            //expected result.
            int expected = original_count + 1;
            //actual result.
            await _client.GetContactsTableAsync()
            .ContinueWith((b) => { list = new List<Contact>(b.Result); });
            var actual = list.Count;
            //perform validation.
            Assert.AreEqual(expected, actual);
            //clean result.
            await _client.DeleteContactCloud(c.Id);
        }
    }
}
