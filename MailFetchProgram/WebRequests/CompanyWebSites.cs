using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebRequest
{
    public class CompanyWebSites
    {
        public List<string> ErrorList { get; set; } = new List<string>();

        public List<string> CompanyCountries { get; set; }

        public List<string> CompanyMail { get; set; } = new List<string>();

        public string CompanySite { get; set; }

        public Task Duty { get; set; }

        string[] PrimaryPagePredicts = new string[]
        {
            "careers",
            "career",
            "current-openings",
            "works",
            "work",
            "case-studies",
            "contact",
            "contact-us",
            "about",
            "about-us",
        };
        public CompanyWebSites(string URL)
        {
            CompanySite = URL;

            Duty=Task.Run(async() =>
            {
                await GetMail(this);
                if (ShouldDoMore()) PredictPageAndSearchIn();
            });
        }
        public string GetHtml(string uri)
        {
            HttpWebRequest Request = (HttpWebRequest)System.Net.WebRequest.Create(uri);

            using (HttpWebResponse response = (HttpWebResponse)Request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        public async Task GetMail(CompanyWebSites x)
        {
            try
            {
                var Content = GetHtml(CompanySite);
                bool found = false;
                for(int i = 0; i < Helpers.PrimaryMailSyntaxPredicts.Length; i++)
                {
                    if (Content.Contains(Helpers.PrimaryMailSyntaxPredicts[i]))
                    {
                        FindAndAddMail(Helpers.PrimaryMailSyntaxPredicts[i], Content, out found);
                    }
                }
                if (!found)
                {
                    for (int i = 0; i < Helpers.SecondaryMailSyntaxPredicts.Length; i++)
                    {
                        if (Content.Contains(Helpers.SecondaryMailSyntaxPredicts[i]))
                        {
                            bool useless = false;
                            FindAndAddMail(Helpers.SecondaryMailSyntaxPredicts[i], Content, out useless);
                        }
                    }
                }
                await Task.Delay(0);
            }
            catch(Exception e)
            {
                ErrorList.Add(e.Message);
            }
        }
        public void FindAndAddMail(string foundPredict,string Content,out bool found)
        {
            var tampon = Content;

            var index = tampon.IndexOf(foundPredict);

            if (tampon.Substring(index).Length > 50) tampon = tampon.Substring(index, 50);
            else tampon = tampon.Substring(index, tampon.Substring(index).Length);

            tampon = DomainAlgorithm.MailDomainControlAlgorithm(tampon, foundPredict);

            Regex deneme = new Regex(@"^.+@[^\.].*\.[a-z]{2,}$");

            MatchCollection matches = deneme.Matches(tampon);
            bool bufferFound = false;
            foreach (Match mailaddress in matches)
            {
                var mailValue = mailaddress.Value;
                if (!CompanyMail.Contains(mailValue))
                {
                    bufferFound = true;
                    CompanyMail.Add(mailValue);
                    Helpers.WriteSplit(mailValue, ',');
                }
            }
            found = bufferFound;
        }
        bool ShouldDoMore()
        {
            if (ErrorList.Count != 0) return false;
            if (CompanyMail.Count != 0)
            {
                foreach (var each in CompanyMail)
                {
                    for (int i = 0; i < Helpers.PrimaryMailSyntaxPredicts.Length; i++)
                    {
                        if (each.Contains(Helpers.PrimaryMailSyntaxPredicts[i])) return false;
                    }
                }
                return true;
            }
            else return true;
        }
        void PredictPageAndSearchIn()
        {
            string[] webExtensions = new string[] { ".html", ".aspx", ".php" };
            for(int i = 0; i < PrimaryPagePredicts.Length; i++)
            {
                string bufferContent = null;
                for (int j = 0; j < webExtensions.Length; j++)
                {
                    try
                    {
                        bufferContent = GetHtml($"{CompanySite}/{PrimaryPagePredicts[i]}{webExtensions[j]}");
                    }
                    catch { }
                }
                if (bufferContent != null)
                {
                    bool found = false;
                    for (int j = 0; j < Helpers.PrimaryMailSyntaxPredicts.Length; j++)
                    {
                        if (bufferContent.Contains(Helpers.PrimaryMailSyntaxPredicts[j]))
                        {
                            FindAndAddMail(Helpers.PrimaryMailSyntaxPredicts[i], bufferContent, out found);
                            if (found)
                            {
                                return;
                            }
                        }
                    }
                    for (int j = 0; j < Helpers.SecondaryMailSyntaxPredicts.Length; j++)
                    {
                        if (bufferContent.Contains(Helpers.SecondaryMailSyntaxPredicts[j]))
                        {
                            bool useless = false;
                            FindAndAddMail(Helpers.SecondaryMailSyntaxPredicts[j], bufferContent, out useless);
                        }
                    }
                }
            }
        }
    }
}
