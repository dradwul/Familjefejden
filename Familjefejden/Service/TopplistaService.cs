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
        public async Task<List<Topplista>> LaddaTopplistaAsync()
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFolder.GetFileAsync("topplista.json");

                string json = await FileIO.ReadTextAsync(file);
                List<Topplista> topplistor = JsonConvert.DeserializeObject<List<Topplista>>(json);

                return topplistor;
            }
            catch
            {
                return new List<Topplista>
                {
                    new Topplista { AnvandareIdPoang = new Dictionary<string, int>() }
                };
            }
        }

        public async Task KopieraFilTillLokalMappAsync(string filnamn)
        {
            StorageFolder installationFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

            StorageFile originalFile = await installationFolder.GetFileAsync(filnamn);
            await originalFile.CopyAsync(storageFolder, filnamn, NameCollisionOption.ReplaceExisting);
        }

        public async Task LaggTillNyTopplistaIJsonFilenAsync(Topplista nyTopplista)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.GetFileAsync("topplista.json");

            string json = await FileIO.ReadTextAsync(file);

            List<Topplista> topplistor = JsonConvert.DeserializeObject<List<Topplista>>(json) ?? new List<Topplista>();
            topplistor.Add(nyTopplista);

            string uppdateradJson = JsonConvert.SerializeObject(topplistor, Formatting.Indented);

            await FileIO.WriteTextAsync(file, uppdateradJson);
        }
    }
}
