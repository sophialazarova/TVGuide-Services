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
    }
}