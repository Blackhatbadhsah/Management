using Blazored.LocalStorage;
using System.Text.Json;
using System.Xml.Linq;

namespace EmployeeManagement.Client.Services
{
    public class ObjectStore
    {
        private readonly ILocalStorageService LocalStorage;
        public ObjectStore(ILocalStorageService localStorage)
        {
            LocalStorage = localStorage;
        }
        // A method to store data in local storage
        public async Task StoreDataAsync<T>(string key, T data)
        {
            // Convert the data to a JSON string
            string jsonData = JsonSerializer.Serialize(data);

            // Store the data in local storage
            await LocalStorage.SetItemAsync(key, jsonData);
        }

        // A method to retrieve data from local storage
        public async Task<T> GetDataAsync<T>(string key)
        {
            // Retrieve the data from local storage
            string jsonData = await LocalStorage.GetItemAsync<string>(key);
            T data = default(T);
            // Convert the JSON string to the desired type
            if (jsonData != null)
               data = JsonSerializer.Deserialize<T>(jsonData);
          
               
            return data;
        }
    }
}
