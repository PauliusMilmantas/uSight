using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace uSight_Web.Entities
{
    public class GeolocationAPI
    {
        private static Lazy<GeolocationAPI> instance = new Lazy<GeolocationAPI>(delegate () { return new GeolocationAPI(); });
        public static GeolocationAPI Instance { get { return instance.Value; } }

        private GeolocationAPI()
        {

        }

        public (string City, string Region, string Country, double Latitude, double Longitude) GetInfo(string ip)
        {
            string url;
            if (ip == "::1" || ip == "localhost" || ip == "127.0.0.1")
            {
                url = "http://ipinfo.io/json";
            }
            else
            {
                url = "http://ipinfo.io/" + ip + "/json";
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException e)
            {
                response = (HttpWebResponse)e.Response;
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    throw new Exception("Invalid GeolocationAPI request");
                }
                else if ((int)response.StatusCode == 429)
                {
                    throw new Exception("No more daily GeolocationAPI requests left");
                }
            }

            using (Stream responseStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                string json = reader.ReadToEnd();
                JObject responseJson = JObject.Parse(json);
                string city = responseJson["city"].ToString();
                string region = responseJson["region"].ToString();
                string country = responseJson["country"].ToString();
                string[] loc = responseJson["loc"].ToString().Split(',');
                double latitude = double.Parse(loc[0]);
                double longitude = double.Parse(loc[1]);
                return (city, region, country, latitude, longitude);
            }
        }
    }
}