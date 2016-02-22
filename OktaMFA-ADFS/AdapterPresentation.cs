using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityServer.Web.Authentication.External;

namespace OktaMFA_ADFS
{
    class AdapterPresentation : IAdapterPresentation, IAdapterPresentationForm
    {
        private string message;
        private bool isPermanentFailure;
        private string upn;
        public string pollingEndpoint;

        public string GetPageTitle(int lcid)
        {
            return "Okta MFA Provider";
        }

        public string GetFormHtml(int lcid)
        {
            string result = "";
            if (!String.IsNullOrEmpty(this.message))
            {
                result += "<p>" + message + "</p>";
            }
            if (!this.isPermanentFailure)
            {
                result += "<form method=\"post\" id=\"loginForm\" autocomplete=\"off\">";
                result += "<p> Enter the Okta Verify code below. Alternatively, accept your push notification and then just hit Continue </p>";
                result += "PIN: <input id=\"pin\" name=\"pin\" type=\"password\" />";
                result += "<input id=\"context\" type=\"hidden\" name=\"Context\" value=\"%Context%\"/>";
                result += "<input id=\"authMethod\" type=\"hidden\" name=\"AuthMethod\" value=\"%AuthMethod%\"/>";
                result += "<input id=\"continueButton\" type=\"submit\" name=\"Continue\" value=\"Continue\" />";
                result += "<input id=\"upn\" type=\"hidden\" name=\"upn\" value=\"" + this.upn + "\"/>";
                result += "<input id=\"pollingEndpoint\" type=\"hidden\" name=\"pollingEndpoint\" value=\"" + this.pollingEndpoint + "\"/>";
                result += "</form>";
            }
            return result;
        }

        public string GetFormPreRenderHtml(int lcid)
        {
            return string.Empty;
        }
        public AdapterPresentation()
        {
            this.message = string.Empty;
            this.isPermanentFailure = false;
        }
        public AdapterPresentation(string message, bool isPermanentFailure)
        {
            this.message = message;
            this.isPermanentFailure = isPermanentFailure;
        }

        public AdapterPresentation(string upn)
        {
            this.message = string.Empty;
            this.isPermanentFailure = false;
            this.upn = upn;
        }

        public AdapterPresentation(string message, string upn, bool isPermanentFailure, string pollingEndpoint)
        {
            this.message = string.Empty;
            this.isPermanentFailure = false;
            this.upn = upn;
            this.pollingEndpoint = pollingEndpoint;
        }

        public AdapterPresentation(string message, string upn, bool isPermanentFailure)
        {
            this.message = message;
            this.isPermanentFailure = isPermanentFailure;
            this.upn = upn;
        }
    }
}
