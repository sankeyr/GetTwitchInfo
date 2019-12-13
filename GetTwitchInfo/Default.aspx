<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="GetTwitchInfo._Default" %>



<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(document).ready(function() {
            //getEdgeUrlOnLoad();            
            document.getElementById('<%= txbUrl.ClientID %>').value = "";
            document.getElementById("lblIpAndServer").value = "";
            $(".loader").addClass("hidden");
        });
        function getEdgeUrlOnLoad() {
            var hiddenValues = JSON.parse(document.getElementById('<%= hiddenUrlInfo.ClientID %>').value);
            var pageUrl = '<%= ResolveUrl("~/Default.aspx")%>';
            var edgeUrl = document.getElementById('<%= txbUrl.ClientID %>').value;
            //var testUrl = "https://video-weaver.fra02.hls.ttvnw.net/v1/playlist/CrwDwSaworbE6Je9fMDnD5pua2XyjrYlYQs2smMxlciWmNm2Ia9p_NvcMMGRHxwjiG-fsQihPrT5WeiA56dk_b2MeknjNQufwtZnI-wzj-gIYc0mZhSKDLHemezt1nkf7_SPjyMZK0-jvbgGUO23h-ZoN0dsZ_XhAhPHG-8JpWXqpz8o4QYWLSAzIBXM6ADyySHWtzmYJ3ZuM8GR1GOrve-7tDn3UdneAr7y27Wo5OaYNGW0Fbvff0Pf0li-oDxXCPPzRrhClZPNYT_XaiMmVdiqS5vXkjtk-cGIAwj-0LlNXuZXBscrLcEH4445QulC1pL5ucA6VP3BiK1Y3R9ayges42ruJAt6d7Z_3EzKQsSHZv8wB54PMIRwXoxTdOp89RE5GtuKnG23zyfE6MEt459iGYlseCcyf5z2QbuXPnng1xX_3L-Cm5RNvzNREmwpky4xEh5IyBQTDqYIYalpY-0rhPAaQKSRd_IwoeiFQdfZNJ23GmmmGaT5O1T9XMUeGfcERetWyoGxc9kJs4B0dwL6QXBJXWNycwRVIB5TzfmIcXjfXmyXQwO5px1IdKCc6PcGfvYtH8R6VTBntaobEhADC7RTGGBq_jAjhGY4WDk4GgycVF5WfW4kYFxOkTs.m3u8"
            for (var i = 0; i < hiddenValues.length; i++) {
                for (var j = 0; j < hiddenValues[i].urlInfo.length; j++) {
                    debugger;
                    $.ajax({
                        type: "POST",
                        url: pageUrl + '/GetEdgeUrlAsync',
                        data: "{'url':'" + JSON.stringify(hiddenValues[i].urlInfo[j].Url) + "'}",
                        contentType: "application/json;charset=utf-8;",
                        dataType: "json",
                        success: function (msg) {
                            debugger;
                            $.ajax({
                                type: "POST",
                                url: pageUrl + '/getIp',
                                async: false,
                                data: "{'url':'" + JSON.stringify(msg) + "'}",
                                contentType: "application/json;charset=utf-8;",
                                dataType: "json",
                                success: function (msg) {
                                    debugger;
                                    var tb = document.getElementById('lblIpAndServer');
                                    var myJSON = JSON.stringify(msg);
                                    document.getElementById("lblIpAndServer").value += hiddenValues[i].Streamer + ":" + "Quality: " +
                                        hiddenValues[i].urlInfo[j].Quality + ":" + msg.d;

                                },
                                error: function (e) {
                                    document.getElementById("errorMessage").innerHTML += e.statusText;
                                    debugger;
                                }
                            });
                            //var tb = document.getElementById('lblIpAndServer');
                            //var myJSON = JSON.stringify(msg);
                            //document.getElementById("lblIpAndServer").value = msg.d;

                        },
                        error: function (e) {
                            document.getElementById("errorMessage").innerHTML += e.statusText;
                            debugger;
                        }
                    });
                }
            }

        }
        function getEdgeUrl() {
            debugger;
            document.getElementById("lblIpAndServer").value = "";
            var pageUrl = '<%= ResolveUrl("~/Default.aspx")%>';
             var edgeUrl = document.getElementById('<%= txbUrl.ClientID %>').value;
            //var testUrl = "https://video-weaver.fra02.hls.ttvnw.net/v1/playlist/CrwDwSaworbE6Je9fMDnD5pua2XyjrYlYQs2smMxlciWmNm2Ia9p_NvcMMGRHxwjiG-fsQihPrT5WeiA56dk_b2MeknjNQufwtZnI-wzj-gIYc0mZhSKDLHemezt1nkf7_SPjyMZK0-jvbgGUO23h-ZoN0dsZ_XhAhPHG-8JpWXqpz8o4QYWLSAzIBXM6ADyySHWtzmYJ3ZuM8GR1GOrve-7tDn3UdneAr7y27Wo5OaYNGW0Fbvff0Pf0li-oDxXCPPzRrhClZPNYT_XaiMmVdiqS5vXkjtk-cGIAwj-0LlNXuZXBscrLcEH4445QulC1pL5ucA6VP3BiK1Y3R9ayges42ruJAt6d7Z_3EzKQsSHZv8wB54PMIRwXoxTdOp89RE5GtuKnG23zyfE6MEt459iGYlseCcyf5z2QbuXPnng1xX_3L-Cm5RNvzNREmwpky4xEh5IyBQTDqYIYalpY-0rhPAaQKSRd_IwoeiFQdfZNJ23GmmmGaT5O1T9XMUeGfcERetWyoGxc9kJs4B0dwL6QXBJXWNycwRVIB5TzfmIcXjfXmyXQwO5px1IdKCc6PcGfvYtH8R6VTBntaobEhADC7RTGGBq_jAjhGY4WDk4GgycVF5WfW4kYFxOkTs.m3u8"
            debugger;
            $.ajax({
                type: "POST",
                url: pageUrl + '/GetEdgeUrlAsync',
                data: "{'url':'" + JSON.stringify(edgeUrl) + "'}",
                contentType: "application/json;charset=utf-8;",
                dataType: "json",
                success: function (msg) {
                    debugger;
                    $.ajax({
                        type: "POST",
                        url: pageUrl + '/getIp',
                        async: false,
                        data: "{'url':'" + JSON.stringify(msg) + "'}",
                        contentType: "application/json;charset=utf-8;",
                        dataType: "json",
                        success: function (msg) {
                            debugger;
                            var tb = document.getElementById('lblIpAndServer');
                            var myJSON = JSON.stringify(msg);
                            document.getElementById("lblIpAndServer").value = msg.d;
                            $(".loader").addClass("hidden");
                        },
                        error: function (e) {
                            document.getElementById("errorMessage").innerHTML += e.statusText;
                            $(".loader").addClass("hidden");
                            debugger;
                        }
                    });
                    //var tb = document.getElementById('lblIpAndServer');
                    //var myJSON = JSON.stringify(msg);
                    //document.getElementById("lblIpAndServer").value = msg.d;

                },
                error: function (e) {
                    document.getElementById("errorMessage").innerHTML += e.statusText;
                    $(".loader").addClass("hidden");
                    debugger;
                }
            });

        }
        function loadResources() {
            debugger;
            var url = document.getElementById('<%= txbUrl.ClientID %>').value;//"https://video-weaver.fra02.hls.ttvnw.net/v1/playlist/Cs4DEOcH4affV8HXFXH29x6dLXmkBatO6cxTGEphzNnJe16QF9gJ_NG0Uz-s6ouiD0jpKhSgzYL7MeqpCEIXA5H6si5kMwodEEjEY2OhuwnuO3WU5sQO43tz-pFAEX8xfUpNbIXHViioHG20oGrg77h9egkHNN4RBkfI6d01zRi1SurQT3tqBMY292gTLAipnvOgO37UHXB4iinls57UV-vHkaFeOSZg2LmVlcdPMuRIKXsUzAAv6UgZaxX7PSZoscVvKYIszh5ZlxWWkGA62pQhNxLNfiwZ55zsVjG7Zj-g843JgW2PK3x9WMFeGXrp6nWgZSxgmJ1NUEKLLnNNI07Xs0dOAH_JOfbxxnrV8cQbKPfwiWDdg1O9Ejmgjp7XGnuRA9rJZbcbRHK3BA5PaURmH8M5d0XLeOGES1SPyLHcuDa2HqEIJ0h9QlG_avM9HAAwSU9E2T6ENbs-DFYPtAsWOR_TahROiArBI5zH-xDi6sWvnLDwcdPa5pdH9IUp8r-g6x-yxlK92GvJ-zousYUs6lqYolSu2HnGFFL3C2VZWMyzkUX2vm4oOSYVdRZRgthb2cMMrfyDTS1gnhE5tuaLyFj2xdDH9QlvwM0W0_EqEhAlHhnM0Xgx5n32nps56ZC5Ggyz55BUcSny6VCQkpw.m3u8";
            var tmp = url.lastIndexOf("/");
            if (tmp != -1) {
                debugger;
                var videoEdge = "";
                var proxyurl = "https://cors-anywhere.herokuapp.com/";
                var m3u8 = proxyurl + url;
                $.ajax({
                    type: "GET",
                    headers: {
                        'Access-Control-Allow-Origin': '*',
                        'X-Requested-With': 'XMLHttpRequest'
                    },
                    url: m3u8,
                    success: function (urlData) {
                        debugger;
                        var lines = urlData.trim().split(/\s*[\r\n]+\s*/g);
                        var len = lines.length;

                        for (var i = 0; i < len; ++i) {
                            if (lines[i].includes("video-edge")) {
                                videoEdge = lines[i];
                                break;
                            }

                        }
                        videoEdge = videoEdge.replace(/(^\w+:|^)\/\//, '');
                        videoEdge = videoEdge.split('/')[0];
                        // videoEdge = Sys.Serialization.JavaScriptSerializer.serialize(videoEdge);
                        debugger;
                        var pageUrl = '<%= ResolveUrl("~/Default.aspx")%>';
                        //document.getElementById('txtArea').innerHTML = videoEdge;
                        $.ajax({
                            type: "POST",
                            url: pageUrl + '/getIp',
                            data: "{'url':'" + JSON.stringify(videoEdge) + "'}",
                            contentType: "application/json;charset=utf-8;",
                            dataType: "json",
                            success: function (msg) {
                                debugger;
                                var tb = document.getElementById('lblIpAndServer');
                                var myJSON = JSON.stringify(msg);
                                document.getElementById("lblIpAndServer").value = msg.d;

                            },
                            error: function (e) {
                                document.getElementById("errorMessage").innerHTML += e.statusText;
                                debugger;
                            }
                        });
                    },
                    error: function (request, status, error) {
                        document.getElementById("errorMessage").innerHTML += "Status: " + status + "-- Error: " + error;
                    },
                    complete: function (data) {
                        console.log(data.statusText);
                    }
                });

            }
        }
        function openInTab() {
            var url = document.getElementById('<%= txbUrl.ClientID %>').value;
            var win = window.open(url, '_blank');
            win.focus();
        }
        function fillBoxFromApi(streamer) {
            debugger;
            $(".loader").removeClass("hidden");
            var pageUrl = '<%= ResolveUrl("~/Default.aspx")%>';
            $.ajax({
                type: "POST",
                url: pageUrl + '/GetStreamerLink',
                data: "{'streamer':'" + JSON.stringify(streamer) + "'}",
                contentType: "application/json;charset=utf-8;",
                dataType: "json",
                success: function (msg) {
                    debugger;
                    document.getElementById('<%= txbUrl.ClientID %>').value = msg.d;
                    getEdgeUrl();
                },
                error: function (e) {
                    $(".loader").addClass("hidden");
                    document.getElementById("errorMessage").innerHTML += e.statusText;
                    debugger;
                }
            });
        }
    </script>
    <div class="row">
        <div class="col-md-12">
            <h2>URL Generator</h2>
            <div>
                <asp:TextBox runat="server" ID="txbUrl" Width="500px"></asp:TextBox>
                <a class="btn btn-default" onclick="getEdgeUrl()">Generate</a>
                <a class="btn btn-default" onclick="openInTab()">Open In New Tab</a>
                <br />
                <div style="float: left">
                    <textarea id="lblIpAndServer" style="max-width: 500px; width: 500px"></textarea>
                </div>                
                <div class="loader" style="float: left; padding-top: 10px; padding-left: 10px;">
                    <div class="loader-wheel"></div>
                    <div class="loader-text"></div>
                </div>
                <br />
            </div>
            <br/>
            <br/>
            <br/>
            <p>
                <asp:Label runat="server" Text="Streamers Online"></asp:Label>
                <asp:Literal ID="ltrInfo" runat="server"></asp:Literal>
                <asp:BulletedList ID="blTabs"
                    BulletStyle="Disc"
                    DisplayMode="LinkButton"
                    runat="server">
                </asp:BulletedList>
                <table id="tableContent" border="1" runat="server"></table>
            </p>
            <p>
                <span id="errorMessage"></span>
                <asp:Label runat="server" ID="lblError"></asp:Label>
            </p>
        </div>
        <%--        <div class="col-md-4">
            <h2>Get more libraries</h2>
            <p>
                NuGet is a free Visual Studio extension that makes it easy to add, remove, and update libraries and tools in Visual Studio projects.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301949">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Web Hosting</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>
            </p>
        </div>
        <div class="col-md-4">
            <h2>Web Hosting</h2>
            <p>
                You can easily find a web hosting company that offers the right mix of features and price for your applications.
            </p>
            <p>
                <a class="btn btn-default" href="https://go.microsoft.com/fwlink/?LinkId=301950">Learn more &raquo;</a>
            </p>
        </div>--%>
    </div>
<%--    <div class="row">
        <iframe src="https://pwn.sh/tools/getstream.html" width="1000" height="300"></iframe>
    </div>--%>
    <asp:HiddenField ID="hiddenUrlInfo" runat="server" />
</asp:Content>
