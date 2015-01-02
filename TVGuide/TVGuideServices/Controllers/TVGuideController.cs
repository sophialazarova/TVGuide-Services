namespace TVGuideServices.Controllers
{
    using HtmlAgilityPack;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using TVGuideServices.Models;

    public class TVGuideController : ApiController
    {
        [HttpGet]
        public async Task<IHttpActionResult> GetScheduleForChannel(int channel, string date)
        {
            const string COMMON_URL = "http://www.start.bg/lenta/tv-programa//tv/show/channel/";
            const int TIME_LENGTH = 5;
            const int TIME_START_INDEX = 0;
            const int NAME_START_INDEX = 19;

            var urlForChannel = COMMON_URL + channel + "/" + date + "/" + "0";

            var client = new HttpClient();
            var request= new HttpRequestMessage();
            request.RequestUri = new Uri(urlForChannel);
            request.Method = HttpMethod.Get;

            var response = await client.SendAsync(request);
            var responseStream = await response.Content.ReadAsStreamAsync();
            using (StreamReader reader = new StreamReader(responseStream))
            {
                var html = reader.ReadToEnd();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var hours = htmlDoc.DocumentNode.SelectSingleNode("//ul[@class=\"tv-dlist\"]");
                
                var groups = hours.SelectNodes("li");
                var res = groups.Select(node => node.InnerText.Trim());
                var result = new List<TVProgramEntry>();
                foreach(var line in res){
                    var time = line.Substring(TIME_START_INDEX, TIME_LENGTH);
                    var name = line.Substring(NAME_START_INDEX);
                    TVProgramEntry entry = new TVProgramEntry(name,time);
                    result.Add(entry);
                }
                return Ok(result);
            }
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetTVSeriesSchedule(string date)
        {
            const string INITIAL_URL = "http://www.start.bg/lenta/tv-programa/tv/seriali";

            var url = INITIAL_URL + "/" + date;
            var client = new HttpClient();
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Get;
            var response = await client.SendAsync(request);
             var responseStream = await response.Content.ReadAsStreamAsync();
             using (StreamReader reader = new StreamReader(responseStream))
             {
                 var html = reader.ReadToEnd();
                 var htmlDoc = new HtmlDocument();
                 htmlDoc.LoadHtml(html);
                 var channels = htmlDoc.DocumentNode.SelectNodes("//div[@class=\"channel\"]");
                 var series = htmlDoc.DocumentNode.SelectNodes("//ul[@class=\"tv-dlist search\"]").ToList();
                 var resChannels = channels.Select(node => node.InnerText).ToList();
                 var dict = new List<TVChannelScheduleForSeries>();
                 for (int i = 0; i < series.Count; i++)
                 {
                     var entry = series[i];
                     var info = entry.SelectNodes("li").ToList();
                     var seriesOnChannel = new List<TVSeriesEntry>();
                     for (int j = 0; j < info.Count; j++)
                     {
                         var li = info[j];
                         var day = li.SelectSingleNode("div[@class=\"day\"]").InnerText;
                         var name = li.SelectSingleNode("div[@class=\"title\"]").InnerText.Replace("\r", string.Empty);
                         var time = li.SelectSingleNode("div[@class=\"time\"]").InnerText;
                         TVSeriesEntry s = new TVSeriesEntry(name, time, day);
                         seriesOnChannel.Add(s);
                     }
                     dict.Add(new TVChannelScheduleForSeries(resChannels[i], seriesOnChannel));
                 }
                 return Ok(dict);

             }
        }

    }
}