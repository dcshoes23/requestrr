using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Requestrr.WebApi.RequestrrBot;

namespace Requestrr.WebApi
{
    public static class SettingsFileUpgrader
    {
        public static void Upgrade()
        {
            dynamic settingsJson = JObject.Parse(File.ReadAllText(SettingsFile.FilePath));

            if (settingsJson.Version.ToString().Equals("1.0.0", StringComparison.InvariantCultureIgnoreCase))
            {
                var botClientJson = settingsJson["BotClient"] as JObject;
                ((JObject)settingsJson["ChatClients"]["Discord"]).Add("MonitoredChannels", JToken.FromObject(botClientJson.GetValue("MonitoredChannels").ToString().Split(" ")));
                ((JObject)settingsJson["BotClient"]).Remove("MonitoredChannels");

                ((JObject)settingsJson["ChatClients"]["Discord"]).Add("TvShowRoles", JToken.FromObject(Array.Empty<string>()));
                ((JObject)settingsJson["ChatClients"]["Discord"]).Add("MovieRoles", JToken.FromObject(Array.Empty<string>()));

                settingsJson.ChatClients.Discord.EnableDirectMessageSupport = false;

                ((JObject)settingsJson["DownloadClients"]["Ombi"]).Add("BaseUrl", string.Empty);

                ((JObject)settingsJson["DownloadClients"]["Radarr"]).Add("BaseUrl", string.Empty);
                ((JObject)settingsJson["DownloadClients"]["Radarr"]).Add("SearchNewRequests", true);
                ((JObject)settingsJson["DownloadClients"]["Radarr"]).Add("MonitorNewRequests", true);

                ((JObject)settingsJson["DownloadClients"]["Sonarr"]).Add("BaseUrl", string.Empty);
                ((JObject)settingsJson["DownloadClients"]["Sonarr"]).Add("SearchNewRequests", true);
                ((JObject)settingsJson["DownloadClients"]["Sonarr"]).Add("MonitorNewRequests", true);

                settingsJson.Version = "1.0.1";
                File.WriteAllText(SettingsFile.FilePath, JsonConvert.SerializeObject(settingsJson));
            }
        }
    }
}