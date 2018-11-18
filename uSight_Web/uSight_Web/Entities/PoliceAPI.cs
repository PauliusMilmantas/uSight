using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace uSight_Web.Entities
{
    public class PoliceAPI
    {
        public bool IsStolen(string plateNumber)
        {
            if (plateNumber.Length < 3 || plateNumber.Length > 10)
            {
                throw new Exception("Incorrect length of plateNumber");
            }
            var parameters = "vnr=" + plateNumber + "&knr=&varnr=&opt=1";

            var url = "https://www.epolicija.lt/itpr_paieska/transportas_en.php";
            var req = System.Net.WebRequest.Create(url);

            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length);
            os.Close();
            HttpWebResponse resp = null;
            HttpStatusCode code;
            try
            {
                resp = (HttpWebResponse)req.GetResponse();
                if (resp == null)
                {
                    return false;
                }
                code = resp.StatusCode;
            }
            catch (WebException e)
            {
                code = ((HttpWebResponse)e.Response).StatusCode;
            }
            
            if (code != HttpStatusCode.OK)
            {
                return false;
            }

            var sr = new StreamReader(resp.GetResponseStream());
            var responseString = sr.ReadToEnd().Trim();
            return !responseString.Contains("There is no information about this transport vehicle in the Register of Wanted Motor Vehicles.");
        }
    }
}