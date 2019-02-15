using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");

            //string edgeUrl = Request.QueryString["edgeurl"];
            //string edgeUrl2 = "https://video-weaver.fra02.hls.ttvnw.net/v1/playlist/Cs4DJP-PfX6B7HLOgNpoxRlaubFsj5ZfBh6W7_jaAZhL3fV77-k8RL-M4xYcT0SCe9U9QfM9yNFDnjPP1dO17rBDKlegRTWmr_FQ9AZU-8mDZASU7J2_oJIN7MDdDOovPqbZEMajNusCwlGzt_-ySa8NRED06czcPTgeXJlVbCVBeT4fgRbfDoQtG9j6_9Twp3T5BRNLhutoOcKmMOFvgZwn2WbwrQlCbY38FbFTPD_P5Sr39kLZQI_xoCx_aUeX5oBOaEmA9LtXnrHjuRv6TvTXnHpYkztpu1EWWnXgIDUNF84G9_pe9WduABemf5dg5qgzh2A-hRnV1KcPd6sCHrdjZ-iTKu01sEfsqoX36-5HM8Kg39ma0GkwsnJ25QwvbiHxkh-xVdlanx1pcMyTEA-Xux7pHY7KousK4qjlNWHR35L1OfBSgxnRSJLbGCIc_ce4v3cB4QUTR5VkzprhEvqGYtgnPLCBZwQAbJSygkBaTBfrqEg14DJjDVB_2kWqEDHapoV5g8rhofGtOuRYCPPAegLPWeAkCeiwaE8XJC780mKcKHTJju6ZDc9hwA3eY7ew8QkyY6JJ8Y13E63GTq4Yv4_JUN70KvkKyxWWDEGJEhDbHtj3618BRiLz4bf8aipsGgxdwavsUukGgR3H9Xc.m3u8";

            //if (!string.IsNullOrWhiteSpace(edgeUrl))
            //    GetEdgeUrlAsync(edgeUrl);
            //else
            //    GetEdgeUrlAsync(edgeUrl2);

            string url = "https://api.twitch.tv/kraken/streams/followed?oauth_token=xsmfc5wupj9nkjyyrkayh933b0kayk";
            string apiUrl = "https://api.twitch.tv/kraken/streams/followed?oauth_token=xsmfc5wupj9nkjyyrkayh933b0kayk";
            

            Uri address = new Uri(apiUrl);

            // Create the web request 
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;

            // Set type to POST 
            request.Method = "GET";
            request.ContentType = "text/xml";

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
                var urlEdge = "video-edge-c2a758.fra02.abs.hls.ttvnw.net";
                var server = urlEdge.ToIPAddress();


                //var objt = JsonConvert.DeserializeObject<dynamic>(strOutputJson);

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

        [WebMethod]                                 //Default.aspx.cs
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
    }
}