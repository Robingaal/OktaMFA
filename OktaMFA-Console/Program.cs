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
            //claims = null;
            IAdapterPresentation result = null;
            //string pin = proofData.Properties["pin"].ToString();
            string tenantName = "marcjordan";
            string baseUrl = "https://" + tenantName + ".oktapreview.com/api/v1/";
            string userName = "marc.jordan@okta.com";
            string authToken = "SSWS 009RUU8EeUvD-EpOEH1qHL0OZwmCTJK71kzFjsQufr";

            HttpWebRequest upnRequest = (HttpWebRequest)WebRequest.Create(baseUrl + "users/" + userName);
            upnRequest.Headers.Add("Authorization", authToken);
            upnRequest.Method = "GET";
            upnRequest.ContentType = "application/json";
            var upnResponse = (HttpWebResponse)upnRequest.GetResponse();
            var streamReader = new StreamReader(upnResponse.GetResponseStream());
            var id = streamReader.ReadToEnd();
                
                userProfile userProfile = JsonConvert.DeserializeObject<userProfile>(id);
                Console.WriteLine(userProfile.id);


            HttpWebRequest factorRequest = (HttpWebRequest)WebRequest.Create(baseUrl + "users/" + userProfile.id + "/factors");
            factorRequest.Headers.Add("Authorization", authToken);
            factorRequest.Method = "GET";
            factorRequest.ContentType = "application/json";
            factorRequest.Accept = "application/json";
            var factorResponse = (HttpWebResponse)factorRequest.GetResponse();
            var factorReader = new StreamReader(factorResponse.GetResponseStream());

            Console.ReadLine();
            //     return result;
        }

    }

    public class otpCode
    {
        public string passCode { get; set; }


    }

    public class userProfile
    {
        public string id { get; set; }
        public string status { get; set; }
        public string lastLogin { get; set; }
        public string lastUpdated { get; set; }
        public string passwordChanged { get; set; }
        public string created { get; set; }

    }


}
