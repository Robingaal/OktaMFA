using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityServer.Web.Authentication.External;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace OktaMFA_Console
{
    class Program
    {
        static void Main(string[] args)
        {
          //  claims = null;
            IAdapterPresentation result = null;
            // string pin = proofData.Properties["pin"].ToString();
            string pin = Console.ReadLine();
            HttpWebRequest httprequest = (HttpWebRequest)WebRequest.Create("https://marcjordan.oktapreview.com/api/v1/users/00u5bjwu5kN4HCRvb0h7/factors/uft5ftmdz7pllPD3X0h7/verify");
            httprequest.Headers.Add("Authorization", "SSWS 009RUU8EeUvD-EpOEH1qHL0OZwmCTJK71kzFjsQufr");
            httprequest.Method = "POST";
            httprequest.ContentType = "application/json";
            otpCode otpCode = new otpCode
            { passCode = pin };
            string otpString = JsonConvert.SerializeObject(otpCode);
            using (var streamWriter = new StreamWriter(httprequest.GetRequestStream()))
            {

                streamWriter.Write(otpString);
            }
            try
            {
                var httpResponse = (HttpWebResponse)httprequest.GetResponse();
                if (httpResponse.StatusCode.ToString() == "OK")
                {
                    Console.WriteLine("Worked");
                    Console.ReadLine();

                }
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var factorResult = streamReader.ReadToEnd();
                }

            }
            catch (WebException we)
            {
                var failResponse = we.Response as HttpWebResponse;
                if (failResponse == null)
                    throw;
                Console.WriteLine("Nope");
                Console.ReadLine();
            }

            
           
       //     return result;
        }
    }
    public class otpCode
    {
        public string passCode { get; set; }


    }
}
