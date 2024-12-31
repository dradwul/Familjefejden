using Klasser;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Documents;

namespace Familjefejden.Service
{
    public class JsonService
    {

//////////////////// Filhantering
        private async Task SparaTillFilAsync(string sektionNamn, object data)
        {
            var allData = await LaddaAllDataFranFilAsync();

            // Ersätter sektion:
            for (int i = 0; i < allData.Count; i++)
            {
                if (allData[i].ContainsKey(sektionNamn))
                {
                    allData[i][sektionNamn] = data;
                    break;
                }
            }

            StorageFolder dataMapp = ApplicationData.Current.LocalFolder;
            StorageFile dataFil = await dataMapp.GetFileAsync("dataFil.json");
            string json = JsonConvert.SerializeObject(allData, Formatting.Indented);
            await FileIO.WriteTextAsync(dataFil, json);
        }

        /// <summary>
        /// Laddar all data från dataFilen. Inklusive Anvandare, topplistor osv.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Dictionary<string, object>>> LaddaAllDataFranFilAsync()
        {
            try
            {
                StorageFolder dataMapp = ApplicationData.Current.LocalFolder;
                StorageFile dataFil = await dataMapp.GetFileAsync("dataFil.json");
                string json = await FileIO.ReadTextAsync(dataFil);

                // Läs in JSON som en lista av dictionaries
                return JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(json);
            }
            catch
            {
                return new List<Dictionary<string, object>>();
            }
        }

        /// <summary>
        /// Kopierar "dataFil.json" i projektmappen till AppMappens ..\LocalState
        /// </summary>
        /// <param name="filnamn">Namnet på originalfilen som ska kopieras</param>
        /// <returns></returns>
        public async Task KopieraFilTillLokalMappAsync(string filnamn)
        {
            StorageFolder installationsMapp = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder dataMapp = ApplicationData.Current.LocalFolder;
            try
            {
                await dataMapp.GetFileAsync(filnamn);
            }
            catch (FileNotFoundException)
            {
                StorageFile originalFil = await installationsMapp.GetFileAsync(filnamn);
                await originalFil.CopyAsync(dataMapp, filnamn, NameCollisionOption.ReplaceExisting);
            }
        }

        /// <summary>
        /// Hämtar data från vald "sektion"
        /// </summary>
        /// <param name="sektionNamn">Parameter för att välja sektion. T.ex. "Topplista"</param>
        /// <returns></returns>
        public async Task<object> HamtaSpecifikDataAsync(string sektionNamn)
        {
            var data = await LaddaAllDataFranFilAsync();

            // Hitta rätt sektion
            foreach (var objekt in data)
            {
                if(objekt.ContainsKey(sektionNamn))
                {
                    return objekt[sektionNamn];
                }
            }
            return new object();
        }

//////////////////// Hantering av Topplista
        // UPPDATERA TOPPLISTA
        public async Task UppdateraTopplistaAsync(Dictionary<string, int> anvandareIdPoang)
        {
            var topplistaData = await HamtaSpecifikDataAsync("Topplista");

            if (topplistaData is Newtonsoft.Json.Linq.JObject topplista)
            {
                topplista["AnvandareIdPoang"] = Newtonsoft.Json.Linq.JObject.FromObject(anvandareIdPoang);

                await SparaTillFilAsync("Topplista", topplista);
            }
        }

        public async Task<Topplista> LaddaTopplistaAsync()
        {
            try
            {
                var topplistaData = await HamtaSpecifikDataAsync("Topplista");

                if (topplistaData is Newtonsoft.Json.Linq.JObject topplistaJson)
                {
                    return topplistaJson.ToObject<Topplista>();
                }
            }
            catch
            {
                Debug.WriteLine("Kunde inte ladda topplistan.");
            }

            return new Topplista { AnvandareIdPoang = new Dictionary<string, int>() };
        }

        //////////////////// Hantering av Användare
        // BETS
        public async Task UppdateraAnvandaresBetsAsync(int anvandareId, List<Bet> bets)
        {
            var anvandareData = await HamtaSpecifikDataAsync("Anvandare");

            if(anvandareData is Newtonsoft.Json.Linq.JArray anvandareLista)
            {
                foreach(var item in anvandareLista)
                {
                    if(item is Newtonsoft.Json.Linq.JObject anvandare && (int)anvandare["Id"] == anvandareId)
                    {
                        anvandare["Bets"] = Newtonsoft.Json.Linq.JArray.FromObject(bets);

                        await SparaTillFilAsync("Anvandare", anvandareLista);
                        return;
                    }
                }
            }
        }

        // LÄGG TILL ANVÄNDARE
        public async Task LaggaTillNyAnvandareAsync(string namn)
        {
            var anvandareData = await HamtaSpecifikDataAsync("Anvandare");

            if(anvandareData is Newtonsoft.Json.Linq.JArray anvandareLista)
            {
                var nyAnvandare = new
                {
                    Id = anvandareLista.Count > 0 ? (int)anvandareLista.Last["Id"] + 1 : 1,
                    Namn = namn,
                    Bets = new List<Bet>()
                };

                anvandareLista.Add(Newtonsoft.Json.Linq.JObject.FromObject(nyAnvandare));

                await SparaTillFilAsync("Anvandare", anvandareLista);
            }
        }

        
    }
}
