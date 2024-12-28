using Klasser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;

namespace Familjefejden.Service
{
    public class TopplistaService
    {
        public async Task<Topplista> LaddaTopplistaAsync()
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFolder.GetFileAsync("topplista.json");

                string json = await FileIO.ReadTextAsync(file);
                Topplista topplista = JsonConvert.DeserializeObject<Topplista>(json);

                return topplista;
            }
            catch
            {
                return new Topplista { AnvandarePoang = new Dictionary<string, int>() };
            }
        }
    }
}
