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

namespace OktaMFA_ADFS
{
    class AuthenticationAdapter : IAuthenticationAdapter
    {
        public IAdapterPresentation BeginAuthentication(System.Security.Claims.Claim identityClaim, System.Net.HttpListenerRequest request, IAuthenticationContext context)
        {
            string upn = identityClaim.Value;


            return new AdapterPresentation(upn);

        }

        public bool IsAvailableForUser(System.Security.Claims.Claim identityClaim, IAuthenticationContext context)
        {
            return true;
        }

        public IAuthenticationAdapterMetadata Metadata
        {
            get { return new AuthenticationAdapterMetadata(); }
        }

        public void OnAuthenticationPipelineLoad(IAuthenticationMethodConfigData configData)
        {

        }

        public void OnAuthenticationPipelineUnload()
        {

        }

        public IAdapterPresentation OnError(System.Net.HttpListenerRequest request, ExternalAuthenticationException ex)
        {
            return new AdapterPresentation(ex.Message, true);
        }

        public IAdapterPresentation TryEndAuthentication(IAuthenticationContext context, IProofData proofData, System.Net.HttpListenerRequest request, out System.Security.Claims.Claim[] claims )
        {
            claims = null;
            IAdapterPresentation result = null;
            //AuthenticationAdapterMetadata Metadata = new AuthenticationAdapterMetadata();
            //string[] upn = Metadata.IdentityClaims;
            string userName = proofData.Properties["upn"].ToString();
            string pin = proofData.Properties["pin"].ToString();
            string tenantName = "marcjordan";
            string baseUrl = "https://" + tenantName + ".oktapreview.com/api/v1/";
            string authToken = "SSWS 009RUU8EeUvD-EpOEH1qHL0OZwmCTJK71kzFjsQufr";

            HttpWebRequest upnRequest = (HttpWebRequest)WebRequest.Create(baseUrl + "users/" + userName);
            upnRequest.Headers.Add("Authorization", authToken);
            upnRequest.Method = "GET";
            upnRequest.ContentType = "application/json";
            var upnResponse = (HttpWebResponse)upnRequest.GetResponse();
            var idReader = new StreamReader(upnResponse.GetResponseStream());
            var id = idReader.ReadToEnd();

            userProfile userProfile = JsonConvert.DeserializeObject<userProfile>(id);

            string userID = userProfile.id.ToString();


            HttpWebRequest httprequest = (HttpWebRequest)WebRequest.Create(baseUrl + "users/" + userID + "/factors/uft5ftmdz7pllPD3X0h7/verify");
            httprequest.Headers.Add("Authorization", authToken);
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
                 System.Security.Claims.Claim claim = new System.Security.Claims.Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2012/12/authmethod/otp");
                 claims = new System.Security.Claims.Claim[] { claim };

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
                result = new AdapterPresentation("Authentication failed.", false);
            }

            //
 //           var httpResponse = (HttpWebResponse)httprequest.GetResponse();
  //          using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
 //           {
 //               var factorResult = streamReader.ReadToEnd();
 //           }
//            
 //           if (pin == "12345")
//            {
//                System.Security.Claims.Claim claim = new System.Security.Claims.Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/authenticationmethod", "http://schemas.microsoft.com/ws/2012/12/authmethod/otp");
 //               claims = new System.Security.Claims.Claim[] { claim };
 //           }
 //           else
 //           {
 //               result = new AdapterPresentation("Authentication failed.", false);
 //           }
            return result;
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
}
