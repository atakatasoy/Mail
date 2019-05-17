using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WebRequest
{
    public class AddressConvertviaUri
    {
        public string BaseURL { get; set; }

        public int PageOrder { get; set; } = 0;

        public string CurrentURL { get; set; }

        public HttpWebRequest Request { get; set; }

        public string HTML { get; set; }

        public List<string> SiteAddresses = new List<string>();

        public AddressConvertviaUri(string URL)
        {
            BaseURL = URL;
            Request= (HttpWebRequest)System.Net.WebRequest.Create(BaseURL);
            GetHTML();
            FillList();
        }
        private void GetHTML()
        {
            using (HttpWebResponse response = (HttpWebResponse)Request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                this.HTML = reader.ReadToEnd();
            }
        }
        private bool FillList()
        {
            var yeni = HTML;
            bool success = false;
            for (; yeni != "";)
            {
                var site = FetchWebSites(yeni, out yeni);

                if (site != "")
                {
                    SiteAddresses.Add(site);
                    success = true;
                }
            }
            if (success) return true;
            return false;
        }
        private string FetchWebSites(string html, out string tampon)
        {
            var s = html.IndexOf(">Go to website");

            if (s != -1)
            {
                tampon = html.Substring(s + 14);
                var qq = html.Substring(0, s);
                var href = qq.Substring(qq.LastIndexOf("href=\"") + 6);
                var last = href.Substring(0, href.Length - 1);
                return last;
            }
            tampon = "";
            return string.Empty;
        }
        public bool NextPage()
        {
            if (PageOrder == 0)
            {
                PageOrder = 2;
                CurrentURL = BaseURL + $"?page={PageOrder}";
                Request = (HttpWebRequest)System.Net.WebRequest.Create(CurrentURL);
                GetHTML();
                if (FillList()) return true;
                return false;
            }
            if (PageOrder == 111) return false;
            PageOrder++;
            CurrentURL = BaseURL + $"?page={PageOrder}";
            Request = (HttpWebRequest)System.Net.WebRequest.Create(CurrentURL);
            GetHTML();
            if (FillList()) return true;
            return false;
        }
    }
}
