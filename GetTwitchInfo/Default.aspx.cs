using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace GetTwitchInfo
{
    public static class NetworkingExtensions
    {
        /// <summary>
        /// Converts a string representing a host name or address to its <see cref="IPAddress"/> representation, 
        /// optionally opting to return a IpV6 address (defaults to IpV4)
        /// </summary>
        /// <param name="hostNameOrAddress">Host name or address to convert into an <see cref="IPAddress"/></param>
        /// <param name="favorIpV6">When <code>true</code> will return an IpV6 address whenever available, otherwise 
        /// returns an IpV4 address instead.</param>
        /// <returns>The <see cref="IPAddress"/> represented by <paramref name="hostNameOrAddress"/> in either IpV4 or
        /// IpV6 (when available) format depending on <paramref name="favorIpV6"/></returns>
        public static IPAddress ToIPAddress(this string hostNameOrAddress, bool favorIpV6 = false)
        {
            AddressFamily favoredFamily;
            if (favorIpV6)
                favoredFamily = AddressFamily.InterNetworkV6;
            else
                favoredFamily = AddressFamily.InterNetwork;
            var addrs = Dns.GetHostAddresses(hostNameOrAddress);
            if (addrs.FirstOrDefault(addr => addr.AddressFamily == favoredFamily) != null)
                return addrs.FirstOrDefault(addr => addr.AddressFamily == favoredFamily);
            else
                return addrs.FirstOrDefault();
        }
    }
    public partial class _Default : Page
    {
        public class Data
        {
            public int _total { get; set; }

            public Links _links { get; set; }

            public List<Streams> streams { get; set; }

            public class Streams
            {
                public string _id { get; set; }

                public Channel channel { get; set; }
                //public Channel channel { get; set; }
                //public List<string> moderators { get; set; }
                //public List<string> staff { get; set; }
                //public List<string> viewers { get; set; }
                //public List<string> admins { get; set; }
            }

            public class Links
            {
                public string self { get; set; }
                public string next { get; set; }
            }

            public class Channel
            {
                public string display_name { get; set; }
                public string game { get; set; }
                public string name { get; set; }
                public string url { get; set; }
            }
        }

        public class StreamerUrlData
        {
            public string Streamer { get; set; }

            public List<UrlInfo> urlInfo { get; set; }

            public class UrlInfo
            {
                public string Quality { get; set; }

                public string Url { get; set; }
            }

        }

        public class TokenSig
        {
            public string token { get; set; }

            public string sig { get; set; }
        }

        public class StreamerApiData
        {

            public Urls urls { get; set; }
            public bool success { get; set; }

            public class Urls
            {
                [JsonProperty("480p")]
                public string _480p { get; set; }

                public string audio_only { get; set; }

                [JsonProperty("360p")]
                public string _360p { get; set; }

                [JsonProperty("1080p60")]
                public string _1080p60 { get; set; }

                [JsonProperty("720p60")]
                public string _720p60 { get; set; }

                [JsonProperty("160p")]
                public string _160p { get; set; }

                [JsonProperty("720p")]
                public string _720p { get; set; }
            }

        }

        protected void Page_Load(object sender, EventArgs e)
        {

            string url = "https://api.twitch.tv/kraken/streams/followed?oauth_token=xsmfc5wupj9nkjyyrkayh933b0kayk";
            string apiUrl = "https://api.twitch.tv/kraken/streams/followed?oauth_token=xsmfc5wupj9nkjyyrkayh933b0kayk";


            Uri address = new Uri(apiUrl);

            // Create the web request 
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST 
            request.Method = "GET";
            request.ContentType = "text/xml";

            string subfoldername = "Streamlink\\bin";
            //Your fileName
            string filename = "streamlink.exe";
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, subfoldername, filename);
            string filePath2 = "C:\\Program Files (x86)\\Streamlink\\bin\\streamlink.exe";

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                // Get the response stream 
                StreamReader reader = new StreamReader(response.GetResponseStream());

                // Console application output 
                string strOutputJson = FormatJson(reader.ReadToEnd());

                //dynamic jsonObj = JsonConvert.DeserializeObject(strOutputJson);
                var data = JsonConvert.DeserializeObject<Data>(strOutputJson);


                ltrInfo.Text = "<ul>";
                foreach (var stream in data.streams)
                {
                    //ltrInfo.Text += "<li><a onclick=\"fillBoxFromApi('" + stream.channel.display_name.ToString() + "')\">" + "twitch.tv/" + stream.channel.display_name + "</a></li>";
                    ltrInfo.Text += 
                    "<li>"+
                    "<span style=\"font-weight:bold;\">" + "twitch.tv/" + stream.channel.display_name + "</span><br/>" +
                    "<a onclick=\"fillBoxFromApi('" + stream.channel.display_name.ToString() + ":_160p" + "')\">" + "160p" + "</a>&nbsp;" +
                    "<a onclick=\"fillBoxFromApi('" + stream.channel.display_name.ToString() + ":_360p" + "')\">" + "360p" + "</a>&nbsp;" +
                    "<a onclick=\"fillBoxFromApi('" + stream.channel.display_name.ToString() + ":_480p" + "')\">" + "480p" + "</a>&nbsp;" +
                    "<a onclick=\"fillBoxFromApi('" + stream.channel.display_name.ToString() + ":_760p" + "')\">" + "760p" + "</a>&nbsp;" +
                    "<a onclick=\"fillBoxFromApi('" + stream.channel.display_name.ToString() + ":audio_only" + "')\">" + "AudioOnly" + "</a>&nbsp;" +
                    "</li>";
                }
                ltrInfo.Text += "</ul>";


                List<StreamerUrlData> weaverUrls = new List<StreamerUrlData>();
                List<StreamerUrlData.UrlInfo> weaverUrlInfos = new List<StreamerUrlData.UrlInfo>();
                int counter = 0;
                int qualityCounter = 0;
            }
        }

        [WebMethod]
        public static string GetEdgeUrlAsync(string url)
        {
            using (var client = new HttpClient())
            {
                url = url.Replace("\"", string.Empty);
                var response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content;

                    // by calling .Result you are synchronously reading the result
                    string responseString = responseContent.ReadAsStringAsync().Result;
                    var lines = responseString.Split('\n');
                    if (lines.Any())
                    {
                        string edgeUrl = null;
                        foreach (string line in lines)
                        {
                            if (line.StartsWith("https"))
                            {
                                edgeUrl = line;
                                edgeUrl = edgeUrl.Substring("https://".Length);
                                return edgeUrl = edgeUrl.Split('/')[0];
                            }
                        }
                    }
                }
            }
            return null;
        }

        [WebMethod]
        public static string GetStreamerLink(string streamer)
        {
            streamer = streamer.Replace("\"", "");
            string[] streamerInfo = streamer.Split(':');
            string streamerName = streamerInfo[0];
            string streamerQuality = streamerInfo[1];
            string apiUrl = "https://pwn.sh/tools/streamapi.py?url=twitch.tv/" + streamerName;

            Uri address = new Uri(apiUrl);

            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            request.Method = "GET";
            request.ContentType = "text/xml";
            StreamerApiData apd = new StreamerApiData();
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());


                string strOutputJson = FormatJson(reader.ReadToEnd());

                var data = JsonConvert.DeserializeObject<StreamerApiData>(strOutputJson);

                switch (streamerQuality)
                {
                    case "_160p":
                        return data.urls._160p;
                    case "_360p":
                        return data.urls._360p;
                    case "_480p":
                        return data.urls._480p;
                    case "_760p":
                        return data.urls._720p;
                    case "audio_only":
                        return data.urls.audio_only;
                }
            }
            return null;
        }

        [WebMethod]
        public static string getIp(string url)
        {
            url = url.Replace("\"", string.Empty);
            url = url.Replace("d:", string.Empty);
            url = url.Replace("{", string.Empty);
            url = url.Replace("}", string.Empty);
            IPAddress[] localIPAddress = Dns.GetHostAddresses(url);
            var ipAddress = localIPAddress.FirstOrDefault();
            return ipAddress + " " + url;
        }

        protected static string FormatJson(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }

        public string StartLivestreamer(string livestreamerArgs, string filePath, Action onClose = null)
        {
            string streamWeaver = string.Empty;
            StringBuilder sb = new StringBuilder();
            // the process needs to be launched from its own thread so it doesn't lockup the UI

            Task responseTask = Task.Run(() =>
            {
                var proc = new Process
                {
                    StartInfo =
                    {
                        FileName = filePath,
                        Arguments = livestreamerArgs,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true,
                        UseShellExecute = false
                    },
                    EnableRaisingEvents = true
                };

                bool preventClose = false;

                // see below for output handler
                proc.ErrorDataReceived +=
                    (sender, args) =>
                    {
                        if (args.Data == null) return;
                        sb.Append(args.Data);
                        sb.Append(Environment.NewLine);
                        preventClose = true;
                        //messageBoxViewModel.MessageText += Environment.NewLine + args.Data;
                    };
                proc.OutputDataReceived +=
                    (sender, args) =>
                    {
                        if (args.Data == null) return;
                        sb.Append(args.Data);
                        sb.Append(Environment.NewLine);
                        if (args.Data.Contains("video-weaver"))
                        {
                            streamWeaver = args.Data;

                        }

                        //if (args.Data.Contains("Starting player") &&
                        //    settingsHandler.Settings.HideStreamOutputMessageBoxOnLoad)
                        //{
                        //    messageBoxViewModel.TryClose();
                        //    // can continue adding messages, the view model still exists so it doesn't really matter
                        //}

                        //messageBoxViewModel.MessageText += Environment.NewLine + args.Data;
                    };

                try
                {
                    proc.Start();
                    //sb.Append("Start");
                    //sb.Append(Environment.NewLine);
                    proc.BeginErrorReadLine();
                    //sb.Append("BeginErrorReadLine");
                    //sb.Append(Environment.NewLine);
                    proc.BeginOutputReadLine();
                    //sb.Append("BeginOutputReadLine");
                    //sb.Append(Environment.NewLine);
                    proc.WaitForExit();

                    if (proc.ExitCode != 0)
                    {
                        preventClose = true;
                    }

                    onClose?.Invoke();
                    //sb.Append("Close");
                    //sb.Append(Environment.NewLine);
                }
                catch (Exception ex)
                {
                    preventClose = true;
                    sb.Append("In Catch error");
                    sb.Append(ex.Message);
                    lblError.Text = "In Catch Error:" + ex.Message;
                    //messageBoxViewModel.MessageText += Environment.NewLine + ex;
                }

                if (preventClose)
                {
                    //messageBoxViewModel.MessageText += Environment.NewLine + Environment.NewLine +
                    //                                   $"ERROR occured in {settingsHandler.Settings.LivestreamExeDisplayName}: " +
                    //                                   $"Manually close this window when you've finished reading the {settingsHandler.Settings.LivestreamExeDisplayName} output.";

                    // open the message box if it was somehow closed prior to the error being displayed
                    //if (!messageBoxViewModel.IsActive)
                    //    windowManager.ShowWindow(messageBoxViewModel, null, new WindowSettingsBuilder().SizeToContent().NoResizeBorderless().Create());
                }
                //else
                //    messageBoxViewModel.TryClose();
            });
            //sb.Append("Wait");
            //sb.Append(Environment.NewLine);
            responseTask.Wait();
            string sbString = sb.ToString();
            if (streamWeaver.Length > 0)
                streamWeaver = TrimStreamWeaver(streamWeaver);
            lblError.Text = sb.ToString();
            return streamWeaver;
        }

        protected string TrimStreamWeaver(string streamWeaver)
        {
            streamWeaver = streamWeaver.Substring(streamWeaver.IndexOf("'", StringComparison.Ordinal) + 1);
            streamWeaver = streamWeaver.Substring(0, streamWeaver.LastIndexOf("'", StringComparison.Ordinal));
            return streamWeaver;
        }
    }
}