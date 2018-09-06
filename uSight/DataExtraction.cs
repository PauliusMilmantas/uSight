using System.Net;
using Newtonsoft.Json.Linq;

namespace uSight
{
    class DataExtraction
    {
        public dynamic getJson()
        {
            var html = new HtmlAgilityPack.HtmlDocument();
            html.LoadHtml(new WebClient().DownloadString("http://www.pauliusmilmantas.esy.es/uSight/service.php?request=2")); //Sioje vietoje saugomas JSON failas

            dynamic obj = JObject.Parse(html.Text);

            return obj;
        }
    }
}
