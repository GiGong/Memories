using Memories.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace Memories.Services
{
    public class FacebookService : IFacebookService
    {
        private const string _facebookHead = "https://graph.facebook.com/";

        public bool IsAuthorized { get => ConfigurationManager.AppSettings["token"] != null; }

        public void Authorize(string token)
        {
            var appSetting = ConfigurationManager.AppSettings;

            StringBuilder param = new StringBuilder();
            param.Append("debug_token?");
            param.Append($"input_token={token}");
            param.Append($"&access_token={appSetting["App_Id"]}|{appSetting["key"]}");

            string target = _facebookHead + param;

            HttpWebRequest http = (HttpWebRequest)WebRequest.Create(target);
            http.Method = "GET";
            http.Timeout = 3 * 1000;

            using (HttpWebResponse resp = (HttpWebResponse)http.GetResponse())
            {
                HttpStatusCode status = resp.StatusCode;
                if (status != HttpStatusCode.OK)
                {
                    throw new Exception("Facebook Server return " + status + Environment.NewLine + "In debuging token");
                }

                Stream respStream = resp.GetResponseStream();
                using (StreamReader sr = new StreamReader(respStream))
                {
                    JToken data = JObject.Parse(sr.ReadToEnd()).SelectToken("data");

                    if (!bool.TryParse(data.SelectToken("is_valid").ToString(), out bool is_valid))
                    {
                        //log json.SelectToken("data")
                        throw new Exception("Debug access token fail!");
                    }

                    if (!is_valid)
                    {
                        throw new Exception("Access token is invalid" + Environment.NewLine + "Facebook error");
                    }
                }
            }

            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings["token"] == null)
            {
                settings.Add("token", token);
            }
            else
            {
                settings["token"].Value = token;
            }
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }

        public void ClearAuthorize()
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;
            if (settings["token"] != null)
            {
                settings.Remove("token");
            }
            configFile.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
        }
    }
}
