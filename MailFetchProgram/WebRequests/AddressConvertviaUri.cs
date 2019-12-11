using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;

namespace WebRequest
{
    public class AddressConvertviaUri
    {
        public string BaseURL { get; set; }

        public int PageOrder { get; set; } = 0;

        public string CurrentURL { get; set; }

        public HttpWebRequest Request { get; set; }

        public string HTML { get; set; }

        public int PageCount { get; set; }

        public List<List<string>> SiteAddresses = new List<List<string>>();

        public AddressConvertviaUri(string URL,int count)
        {
            BaseURL = URL;

            PageCount = count;

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
            SiteAddresses.Add(new List<string>());
            var yeni = HTML;

            bool success = false;

            for (; yeni != "";)
            {
                var site = FetchWebSites(yeni, out yeni);

                if (site != "")
                {
                    SiteAddresses[SiteAddresses.Count - 1].Add(site);

                    success = true;
                }
            }
            if (success == true) return true;

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
            if (PageCount == 1) return false;

            if (PageOrder == 0)
            {
                PageOrder = 2;

                CurrentURL = BaseURL + $"?page={PageOrder}";

                Request = (HttpWebRequest)System.Net.WebRequest.Create(CurrentURL);

                GetHTML();

                return FillList();
            }
            if (PageOrder == PageCount) return false;

            CurrentURL = BaseURL + $"?page={PageOrder}";

            Request = (HttpWebRequest)System.Net.WebRequest.Create(CurrentURL);

            GetHTML();

            PageOrder++;

            return FillList();
        }
    }
}
