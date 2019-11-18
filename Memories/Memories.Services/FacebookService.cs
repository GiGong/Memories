using Facebook;
using Memories.Business.Facebook;
using Memories.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Memories.Services
{
    public class FacebookService : IFacebookService
    {
        private const string _facebookHead = "https://graph.facebook.com/";
        private List<FacebookPhoto> _facebookPhotos = null;
        private DateTime _photoUpdatedTime;

        public bool IsAuthorized { get => ConfigurationManager.AppSettings["token"] != null; }

        public string GetLoginUrl(object parameters)
        {
            var appSetting = ConfigurationManager.AppSettings;
            FacebookClient client = new FacebookClient()
            {
                Version = appSetting["Version"],
                AppId = appSetting["App_Id"]
            };

            return client.GetLoginUrl(parameters).AbsoluteUri;
        }

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
                    JObject data = JObject.Parse(sr.ReadToEnd()).Value<JObject>("data");

                    if (!data.Value<bool>("is_valid"))
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

        public IEnumerable<string> GetDeclinedList(string token)
        {
            FacebookClient client = new FacebookClient(token)
            {
                Version = ConfigurationManager.AppSettings["Version"]
            };

            string permissions = client.Get("me", new { fields = "permissions" }).ToString();
            var data = JObject.Parse(permissions).Value<JObject>("permissions").Value<JArray>("data");
            List<string> declined = new List<string>();

            foreach (JObject permission in data)
            {
                if (permission.Value<string>("status") == "declined")
                {
                    declined.Add(permission.Value<string>("permission"));
                }
            }

            return declined.Count > 0 ? declined : null;
        }

        public string GetReRequestUrl(IEnumerable<string> scopes, string token)
        {
            var appSetting = ConfigurationManager.AppSettings;
            FacebookClient client = new FacebookClient(token)
            {
                Version = appSetting["Version"],
                AppId = appSetting["App_Id"]
            };

            StringBuilder sb = new StringBuilder();
            foreach (var item in scopes)
            {
                sb.Append(item + ",");
            }

            return client.GetLoginUrl(new
            {
                redirect_uri = appSetting["RedirectUri"],
                response_type = "token",
                auth_type = "rerequest",
                scope = sb.ToString()
            }).AbsoluteUri;
        }

        public async Task<IEnumerable<FacebookPhoto>> GetPhotosAsync(bool isRefresh)
        {
            if (!isRefresh)
            {
                return _facebookPhotos;
            }

            var appSetting = ConfigurationManager.AppSettings;
            FacebookClient client = new FacebookClient(appSetting["token"])
            {
                Version = appSetting["Version"],
                AppId = appSetting["App_Id"]
            };
            client.SetJsonSerializers(JsonConvert.SerializeObject, JsonConvert.DeserializeObject);
            List<FacebookPhoto> list = new List<FacebookPhoto>(100);

            string result = (await client.GetTaskAsync("me", new { fields = "albums.limit(10){photos.limit(10){created_time,picture,images}}" })).ToString();
            _photoUpdatedTime = DateTime.Now;

            JObject resultObject = JObject.Parse(result);
            if (!resultObject.ContainsKey("albums"))
            {
                return null;
            }
            JObject albums = resultObject.Value<JObject>("albums");

            while (true)
            {
                foreach (JObject album in albums.Value<JArray>("data"))
                {
                    JObject photos = album.Value<JObject>("photos");

                    while (true)
                    {
                        foreach (JObject photo in photos.Value<JArray>("data"))
                        {
                            var fb = new FacebookPhoto
                            {
                                Id = photo.Value<string>("id"),
                                Created_Time = photo.Value<DateTime>("created_time"),
                                PreviewImage = photo.Value<string>("picture"),
                                SourceImage = photo.Value<JArray>("images")[0].Value<string>("source")
                            };
                            list.Add(fb);
                        }

                        if (!photos.Value<JObject>("paging").ContainsKey("next"))
                        {
                            break;
                        }
                        result = (await client.GetTaskAsync(photos.Value<JObject>("paging").Value<string>("next"))).ToString();
                        photos = JObject.Parse(result);
                    }
                }

                if (!albums.Value<JObject>("paging").ContainsKey("next"))
                {
                    break;
                }
                result = (await client.GetTaskAsync(albums.Value<JObject>("paging").Value<string>("next"))).ToString();
                albums = JObject.Parse(result);
            }

            list.Sort((x, y) => y.Created_Time.CompareTo(x.Created_Time));
            _facebookPhotos = list.ToList();
            return _facebookPhotos.Count > 0 ? _facebookPhotos : null;
        }

        public string GetPhotoUpdatedTime()
        {
            return _photoUpdatedTime.ToString("yyyy-MM-dd tt h:mm");
        }
    }

    internal class RecentComparer : IComparer<DateTime>
    {
        public int Compare(DateTime x, DateTime y)
        {
            return y.CompareTo(x);
        }
    }
}
