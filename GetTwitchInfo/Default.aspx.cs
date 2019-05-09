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

        protected void Page_Load(object sender, EventArgs e)
        {
            //string livestreamerArgs = "twitch.tv/BreaK/ audio_only";
            //string one = StartLivestreamer(livestreamerArgs);
            //string livestreamerArgs2 = "twitch.tv/vsnz/ 160p";
            //string onesix = StartLivestreamer(livestreamerArgs2);


            //string edgeUrl = Request.QueryString["edgeurl"];
            //string edgeUrl2 = "https://video-weaver.fra02.hls.ttvnw.net/v1/playlist/Cs4DJP-PfX6B7HLOgNpoxRlaubFsj5ZfBh6W7_jaAZhL3fV77-k8RL-M4xYcT0SCe9U9QfM9yNFDnjPP1dO17rBDKlegRTWmr_FQ9AZU-8mDZASU7J2_oJIN7MDdDOovPqbZEMajNusCwlGzt_-ySa8NRED06czcPTgeXJlVbCVBeT4fgRbfDoQtG9j6_9Twp3T5BRNLhutoOcKmMOFvgZwn2WbwrQlCbY38FbFTPD_P5Sr39kLZQI_xoCx_aUeX5oBOaEmA9LtXnrHjuRv6TvTXnHpYkztpu1EWWnXgIDUNF84G9_pe9WduABemf5dg5qgzh2A-hRnV1KcPd6sCHrdjZ-iTKu01sEfsqoX36-5HM8Kg39ma0GkwsnJ25QwvbiHxkh-xVdlanx1pcMyTEA-Xux7pHY7KousK4qjlNWHR35L1OfBSgxnRSJLbGCIc_ce4v3cB4QUTR5VkzprhEvqGYtgnPLCBZwQAbJSygkBaTBfrqEg14DJjDVB_2kWqEDHapoV5g8rhofGtOuRYCPPAegLPWeAkCeiwaE8XJC780mKcKHTJju6ZDc9hwA3eY7ew8QkyY6JJ8Y13E63GTq4Yv4_JUN70KvkKyxWWDEGJEhDbHtj3618BRiLz4bf8aipsGgxdwavsUukGgR3H9Xc.m3u8";

            //if (!string.IsNullOrWhiteSpace(edgeUrl))
            //    GetEdgeUrlAsync(edgeUrl);
            //else
            //    GetEdgeUrlAsync(edgeUrl2);
            //BuildTable();

            //testUrlStuff();

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



                foreach (var stream in data.streams)
                {
                    ListItem li = new ListItem();
                    li.Value = stream.channel.url;  //html goes here i.e.  xtab1.html
                    li.Text = "twitch.tv/" + stream.channel.display_name;  //text name goes i.e. here tab1
                    blTabs.Items.Add(li);
                }



                List<StreamerUrlData> weaverUrls = new List<StreamerUrlData>();
                List<StreamerUrlData.UrlInfo> weaverUrlInfos = new List<StreamerUrlData.UrlInfo>();
                int counter = 0;
                int qualityCounter = 0;
                //foreach (var stream in data.streams)
                //{
                //    if (stream.channel.display_name.ToLower() == "break")
                //    {
                //        StreamerUrlData sd = new StreamerUrlData();
                //        sd.Streamer = stream.channel.display_name;
                //        for (int i = 0; i < 2; i++)
                //        {
                //            weaverUrlInfos = new List<StreamerUrlData.UrlInfo>();
                //            StreamerUrlData.UrlInfo info = new StreamerUrlData.UrlInfo
                //            {
                //                Quality = "Audio Only",
                //                Url = StartLivestreamer("twitch.tv/" + stream.channel.display_name + "/ audio_only", filePath2)
                //            };
                //            weaverUrlInfos.Add(info);
                //            info = new StreamerUrlData.UrlInfo
                //            {
                //                Quality = "160p",
                //                Url = StartLivestreamer("twitch.tv/" + stream.channel.display_name + "/ 160p", filePath2)
                //            };
                //            weaverUrlInfos.Add(info);
                //            sd.urlInfo = weaverUrlInfos;
                //        }


                //        weaverUrls.Add(sd);
                //        break;
                //    }

                //}
                //var jsonOutput = JsonConvert.SerializeObject(weaverUrls);
                //hiddenUrlInfo.Value = jsonOutput;
                //var urlEdge = "video-edge-c2a758.fra02.abs.hls.ttvnw.net";
                //var server = urlEdge.ToIPAddress();

                //using (var w = new WebClient())
                //{
                //    w.Proxy = null;
                //    var jsonData = w.DownloadString(string.Format(apiUrl));
                //    var stream = JObject.Parse(jsonData.Replace(@"\r\n", ""));
                //    var chatLinks = stream.SelectToken(@"_links");
                //    var chatCount = stream.SelectToken("chatter_count");
                //    var getAdmins = stream.SelectToken("chatters").SelectToken("admins").Select(s => (string)s).ToList();
                //    var getStaff = stream.SelectToken("chatters").SelectToken("staff").Select(s => (string)s).ToList();
                //    var getModerators = stream.SelectToken("chatters").SelectToken("moderators").Select(s => (string)s).ToList();
                //    var getViewers = stream.SelectToken("chatters").SelectToken("viewers").Select(s => (string)s).ToList(); 
                //}
            }
        }

        protected void testUrlStuff()
        {
            string apiUrl = "https://api.twitch.tv/api/channels/drdisrespect/access_token?client_id=cayy6vdna64rpak248vna4kbqythej";

            Uri address = new Uri(apiUrl);

            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            request.Method = "GET";
            request.ContentType = "text/xml";
            var tokenSig = new TokenSig();
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string strOutputJson = FormatJson(reader.ReadToEnd());

                var data = JsonConvert.DeserializeObject<TokenSig>(strOutputJson);
                tokenSig = data;



                //foreach (var stream in data.streams)
                //{
                //    ListItem li = new ListItem();
                //    li.Value = stream.channel.url; //html goes here i.e.  xtab1.html
                //    li.Text = "twitch.tv/" + stream.channel.display_name; //text name goes i.e. here tab1
                //    blTabs.Items.Add(li);
                //}
            }

            apiUrl = "https://usher.ttvnw.net/api/channel/hls/drdisrespect.m3u8?allow_audio_only=true&sig=" + tokenSig.sig + "\\&token=" + Uri.EscapeUriString(tokenSig.token);

            address = new Uri(apiUrl);

            request = WebRequest.Create(address) as HttpWebRequest;

            request.Method = "GET";
            request.ContentType = "text/xml";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string strOutputJson = FormatJson(reader.ReadToEnd());

                var data = JsonConvert.DeserializeObject<TokenSig>(strOutputJson);



                //foreach (var stream in data.streams)
                //{
                //    ListItem li = new ListItem();
                //    li.Value = stream.channel.url; //html goes here i.e.  xtab1.html
                //    li.Text = "twitch.tv/" + stream.channel.display_name; //text name goes i.e. here tab1
                //    blTabs.Items.Add(li);
                //}
            }
        }

        protected void BuildTable()
        {
            HtmlTableRow row = new HtmlTableRow();
            HtmlTableCell cell = new HtmlTableCell();

            cell.ColSpan = 3;
            cell.InnerText = "Record 1";
            row.Cells.Add(cell);
            tableContent.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();

            cell.InnerText = "1";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.InnerText = "2";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.InnerText = "3";
            row.Cells.Add(cell);

            tableContent.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();

            cell.InnerText = "a";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.InnerText = "b";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.InnerText = "c";
            row.Cells.Add(cell);

            tableContent.Rows.Add(row);


            row = new HtmlTableRow();
            cell = new HtmlTableCell();
            cell.InnerText = "m";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.InnerText = "n";
            row.Cells.Add(cell);

            cell = new HtmlTableCell();
            cell.InnerText = "o";
            row.Cells.Add(cell);

            tableContent.Rows.Add(row);

            row = new HtmlTableRow();
            cell = new HtmlTableCell();

            //HtmlInputButton input = new HtmlInputButton();
            //input.ID = "Button1";
            //input.Value = "button";

            //cell.ColSpan = 3;
            //cell.Controls.Add(input);
            //row.Cells.Add(cell);
            //tableContent.Rows.Add(row);
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
                        if (lines[8].StartsWith("https"))
                        {
                            edgeUrl = lines[8];
                            edgeUrl = edgeUrl.Substring("https://".Length);
                            return edgeUrl = edgeUrl.Split('/')[0];
                        }

                    }
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

            //var server = url.ToIPAddress();
            //var blog = url.ToIPAddress();
            //var google = url.ToIPAddress();
            //var ipv6Google = url.ToIPAddress(true); // if available will be an IPV6
            return ipAddress + " " + url;
        }

        protected string FormatJson(string json)
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