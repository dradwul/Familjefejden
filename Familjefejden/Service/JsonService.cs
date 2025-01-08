using Familjefejden.Klasser;
using Klasser;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

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
                if (objekt.ContainsKey(sektionNamn))
                {
                    return objekt[sektionNamn];
                }
            }
            return new object();
        }


        /*  /// /// ///  ------------  \\\ \\\ \\\
        // ||| ||| |||     NY SKIT     ||| ||| |||
        /  \\\ \\\ \\\  ------------  /// /// /// */

        // GRUPP-HANTERING
        // Just nu får gruppen id 1 iom att vi bara har en grupp i nuläget
        public async Task<bool> LaggTillNyGruppAsync(Grupp nyGrupp)
        {
            var gruppData = await HamtaSpecifikDataAsync("Grupp");

            if (gruppData is JObject gruppObjekt)
            {
                if (!string.IsNullOrEmpty((string)gruppObjekt["Namn"]))
                {
                    return false;
                }

                gruppObjekt["Id"] = 1;
                gruppObjekt["Namn"] = nyGrupp.Namn;
                gruppObjekt["Anvandare"] = JArray.FromObject(nyGrupp.Medlemmar);
                await SparaTillFilAsync("Grupp", gruppObjekt);
                return true;
            }
            return false;
        }

        public async Task<bool> KollaOmDetRedanFinnsEnGrupp()
        {
            var gruppData = await HamtaSpecifikDataAsync("Grupp");

            if (gruppData is JObject gruppObjekt)
            {
                if (!string.IsNullOrEmpty((string)gruppObjekt["Namn"]))
                {
                    return true;
                }
            }
            return false;
        }

        // LÄGG TILL ANVÄNDARE
        public async Task<bool> LaggTillAnvandareIGruppAsync(Anvandare nyAnvandare)
        {
            var gruppData = await HamtaSpecifikDataAsync("Grupp");
            if (gruppData is JObject gruppObjekt)
            {
                if (string.IsNullOrEmpty((string)gruppObjekt["Namn"]))
                {
                    return false;
                }

                var befintligaAnvandare = gruppObjekt["Anvandare"] as JArray ?? new JArray();

                int maxId = befintligaAnvandare
                    .Select(a => (int)a["Id"])
                    .DefaultIfEmpty(0)
                    .Max();

                nyAnvandare.Id = maxId + 1;

                var nyAnvandareJson = JObject.FromObject(nyAnvandare);
                if (!befintligaAnvandare.Any(m => JToken.DeepEquals(m, nyAnvandareJson)))
                {
                    befintligaAnvandare.Add(nyAnvandareJson);
                }
                else
                {
                    return false;
                }

                gruppObjekt["Anvandare"] = befintligaAnvandare;
                await SparaTillFilAsync("Grupp", gruppObjekt);
                return true;
            }
            return false;
        }


        // SKAPA TURNERING
        public async Task<bool> LaggaTillNyTurneringAsync(Turnering nyTurnering)
        {
            var turneringData = await HamtaSpecifikDataAsync("Turnering");

            if (turneringData is JObject turneringObjekt)
            {
                if (!string.IsNullOrEmpty((string)turneringObjekt["Namn"]))
                {
                    return false;
                }

                turneringObjekt["Id"] = 1;
                turneringObjekt["Namn"] = nyTurnering.Namn;
                turneringObjekt["Matcher"] = JArray.FromObject(nyTurnering.Matcher);
                turneringObjekt["Lag"] = JArray.FromObject(nyTurnering.Lag);

                await SparaTillFilAsync("Turnering", turneringObjekt);
                return true;
            }
            return false;
        }

        // LÄGG TILL Lag
        public async Task<bool> LaggTillLagAsync(Lag nyttLag)
        {
            var turneringData = await HamtaSpecifikDataAsync("Turnering");
            if (turneringData is JObject turneringObjekt)
            {
                if (string.IsNullOrEmpty((string)turneringObjekt["Namn"]))
                {
                    return false;
                }

                var befintligaLag = turneringObjekt["Lag"] as JArray ?? new JArray();

                int maxId = befintligaLag
                    .Select(a => (int)a["Id"])
                    .DefaultIfEmpty(0)
                    .Max();

                nyttLag.Id = maxId + 1;

                var nyttLagJson = JObject.FromObject(nyttLag);
                if (!befintligaLag.Any(m => JToken.DeepEquals(m, nyttLagJson)))
                {
                    befintligaLag.Add(nyttLagJson);
                }
                else
                {
                    return false;
                }

                turneringObjekt["Lag"] = befintligaLag;
                await SparaTillFilAsync("Turnering", turneringObjekt);
                return true;
            }
            return false;
        }

        // LÄGG TILL MATCH
        public async Task<bool> LaggTillMatchAsync(Match nyMatch)
        {
            var turneringData = await HamtaSpecifikDataAsync("Turnering");
            if (turneringData is JObject turneringObjekt)
            {
                if (string.IsNullOrEmpty((string)turneringObjekt["Namn"]))
                {
                    return false;
                }

                var befintligaMatcher = turneringObjekt["Matcher"] as JArray ?? new JArray();

                int maxId = befintligaMatcher
                    .Select(a => (int)a["Id"])
                    .DefaultIfEmpty(0)
                    .Max();

                nyMatch.Id = maxId + 1;

                var nyMatchJson = JObject.FromObject(nyMatch);
                if (!befintligaMatcher.Any(m => JToken.DeepEquals(m, nyMatchJson)))
                {
                    befintligaMatcher.Add(nyMatchJson);
                }
                else
                {
                    return false;
                }

                turneringObjekt["Matcher"] = befintligaMatcher;
                await SparaTillFilAsync("Turnering", turneringObjekt);
                return true;
            }
            return false;
        }

        public async Task<bool> LaggTillBetAsync(int anvandareId, Bet nyttBet)
        {
            var gruppData = await HamtaSpecifikDataAsync("Grupp");
            if (gruppData is JObject gruppObjekt)
            {
                var anvandareLista = gruppObjekt["Anvandare"] as JArray;
                if (anvandareLista != null)
                {
                    var anvandare = anvandareLista
                        .FirstOrDefault(a => (int)a["Id"] == anvandareId) as JObject;

                    if (anvandare != null)
                    {
                        var anvandarensBets = anvandare["Bets"] as JArray;

                        bool betFinns = anvandarensBets
                            .Any(bet => (int)bet["MatchId"] == nyttBet.MatchId);

                        if (betFinns)
                        {
                            Debug.WriteLine("Bet för detta matchId finns för detta användarId");
                            return false;
                        }

                        var nyttBetJson = JObject.FromObject(nyttBet);
                        anvandarensBets.Add(nyttBetJson);

                        anvandare["Bets"] = anvandarensBets;

                        await SparaTillFilAsync("Grupp", gruppObjekt);
                        return true;
                    }
                }
            }
            return false;
        }


        // UPPDATERA GRUPP
        // OBS OBS EJ TESTAD
        public async Task UppdateraGruppAsync(int gruppId, Grupp uppdateradGrupp)
        {
            var gruppData = await HamtaSpecifikDataAsync("Grupp");

            if (gruppData is JArray gruppLista)
            {
                foreach (var item in gruppLista)
                {
                    if (item is JObject grupp && (int)grupp["Id"] == gruppId)
                    {
                        grupp["Namn"] = uppdateradGrupp.Namn;
                        grupp["Medlemmar"] = JArray.FromObject(uppdateradGrupp.Medlemmar);

                        await SparaTillFilAsync("Grupp", gruppLista);
                        return;
                    }
                }
            }
        }

        // UPPDATERA POÄNG FÖR ANVÄNDARE
        public async Task UppdateraPoangForAnvandareAsync(int anvandareId, int poangAttLaggaTill)
        {
            var gruppData = await HamtaSpecifikDataAsync("Grupp");

            if (gruppData is JObject gruppObjekt)
            {
                var anvandareLista = gruppObjekt["Anvandare"] as JArray;

                if (anvandareLista != null)
                {
                    foreach (var anvandare in anvandareLista)
                    {
                        if ((int)anvandare["Id"] == anvandareId)
                        {
                            anvandare["TotalPoang"] = (int)anvandare["TotalPoang"] + poangAttLaggaTill;

                            gruppObjekt["Anvandare"] = anvandareLista;
                            await SparaTillFilAsync("Grupp", gruppObjekt);
                            return;
                        }
                    }
                }
            }

            throw new Exception("Användare med angivet ID hittades inte.");
        }


        public async Task<string> HamtaLagnamnFranLagId(int lagId)
        {
            var turneringData = await HamtaSpecifikDataAsync("Turnering");

            if (turneringData is JObject turneringObjekt)
            {
                var lagLista = turneringObjekt["Lag"] as JArray;

                if (lagLista != null)
                {
                    foreach (var lag in lagLista)
                    {
                        if ((int)lag["Id"] == lagId)
                        {
                            return (string)lag["Namn"];
                        }
                    }
                }
            }
            return null;
        }

        public async Task<int> HamtaLagIdFranNamn(string lagnamn)
        {
            var turneringData = await HamtaSpecifikDataAsync("Turnering");

            if (turneringData is JObject turneringObjekt)
            {
                var lagLista = turneringObjekt["Lag"] as JArray;

                if (lagLista != null)
                {
                    foreach (var lag in lagLista)
                    {
                        if ((string)lag["Namn"] == lagnamn)
                        {
                            return (int)lag["Id"];
                        }
                    }
                }
            }
            return -1;
        }

        public async Task<bool> KontrolleraOmLagFinns(string lagnamn)
        {
            var turneringData = await HamtaSpecifikDataAsync("Turnering");

            if (turneringData is JObject turneringObjekt)
            {
                var lagLista = turneringObjekt["Lag"] as JArray;
                if (lagLista != null)
                {
                    return lagLista.Any(lag => (string)lag["Namn"] == lagnamn);
                }
            }
            return false;
        }

        public async Task<string> HamtaTurneringsnamnFranId(int turneringId)
        {
            var turneringData = await HamtaSpecifikDataAsync("Turnering");

            if (turneringData is JObject turneringObjekt)
            {
                if ((int)turneringObjekt["Id"] == turneringId)
                {
                    return (string)turneringObjekt["Namn"];
                }
            }
            return null;
        }

        public async Task<string> HamtaGruppnamnFranId(int gruppId)
        {
            var gruppData = await HamtaSpecifikDataAsync("Grupp");

            if (gruppData is JObject gruppObjekt)
            {
                if ((int)gruppObjekt["Id"] == gruppId)
                {
                    return (string)gruppObjekt["Namn"];
                }
            }
            return null;
        }

        public async Task<List<Lag>> HamtaAllaLagAsync()
        {
            var turneringData = await HamtaSpecifikDataAsync("Turnering");
            if (turneringData is JObject turneringObjekt)
            {
                var lagLista = turneringObjekt["Lag"] as JArray;
                if (lagLista != null)
                {
                    return lagLista.Select(lag => lag.ToObject<Lag>()).ToList();
                }
            }
            return new List<Lag>();
        }

        public async Task<List<Anvandare>> HamtaAllaAnvandareAsync()
        {
            var gruppData = await HamtaSpecifikDataAsync("Grupp");
            if (gruppData is JObject gruppObjekt)
            {
                var anvandareLista = gruppObjekt["Anvandare"] as JArray;
                if (anvandareLista != null)
                {
                    return anvandareLista.Select(anvandare => anvandare.ToObject<Anvandare>()).ToList();
                }
            }
            return new List<Anvandare>();
        }
    }
}
