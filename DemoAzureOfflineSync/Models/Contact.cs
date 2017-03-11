using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoAzureOfflineSync.Models
{
    [DataTable("Contacts")]
    public class Contact
    {
        [JsonProperty("Id")]
        public string Id { get; set; }
        [JsonProperty("Name")]
        public string Name { get; set; }
        [Version]
        public string Version { get; set; }
        [JsonProperty("IsLocal")]
        public bool IsLocal { get; set; }
        [JsonProperty("Phone")]
        public string Phone { get; set; }
    }
}
