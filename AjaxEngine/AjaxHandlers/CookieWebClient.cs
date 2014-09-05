using System;
using System.Net;

namespace AjaxEngine.AjaxHandlers
{
    public class CookieWebClient : WebClient
    {
        public static CookieContainer Cookies = new CookieContainer(int.MaxValue);

        protected override WebRequest GetWebRequest(Uri address)
        {
            var req = base.GetWebRequest(address);
            if (req is HttpWebRequest)
            {
                (req as HttpWebRequest).CookieContainer = Cookies;
            }
            return req;
        }
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            var rep = base.GetWebResponse(request);
            if (rep is HttpWebResponse)
            {
                Cookies.Add((rep as HttpWebResponse).Cookies);
            }
            return rep;
        }
    }
}
