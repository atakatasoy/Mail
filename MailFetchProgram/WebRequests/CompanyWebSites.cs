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

        public List<string> CompanyCountries { get; set; }

        public List<string> CompanyMail { get; set; } = new List<string>();

        public string CompanySite { get; set; }

        public Task Duty { get; set; }

        public CompanyWebSites(string URL)
        {
            CompanySite = URL;
            Duty=Task.Run(async() =>
            {
                await GetMail(this);
            });
        }
        public async Task GetMail(CompanyWebSites x)
        {
            try
            {
                string Content = "";
                
                HttpWebRequest Request = (HttpWebRequest)System.Net.WebRequest.Create(@CompanySite);
                using (HttpWebResponse response = (HttpWebResponse)Request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    Content = reader.ReadToEnd();
                }
                string[] MailSyntaxPredicts = new string[] { "jobs@","job@", "careers@", "career@","recruiter@","work@","hr@", "info@", "hello@","hi@","contact@","contactus@","support@","sales@"};
                for(int i = 0; i < MailSyntaxPredicts.Length; i++)
                {
                    if (Content.Contains(MailSyntaxPredicts[i]))
                    {
                        var tampon = Content;
                        var index = tampon.IndexOf(MailSyntaxPredicts[i]);
                        tampon = tampon.Substring(index, 50);
                        tampon = DomainAlhorithm.MailDomainControlAlgorithm(tampon, MailSyntaxPredicts[i]);
                        Regex deneme = new Regex(@"^.+@[^\.].*\.[a-z]{2,}$");
                        MatchCollection matches = deneme.Matches(tampon);
                        foreach (Match mailaddress in matches)
                        {
                            CompanyMail.Add(mailaddress.Value);
                            Helpers.WriteSplit(mailaddress.Value, ',');
                        }
                        
                    }
                }
                
                await Task.Delay(0);
               
            }
            catch(Exception e)
            {
                CompanyMail.Add(e.Message);
            }
        }
       
    }
}
