using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MultipleInsertSql;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Threading;
using System;

namespace WebRequest
{ 
    public class WebRequestMain
    {
        public class Trial
        {
            string mail { get; set; }
            bool state { get; set; }
        }
        static void Main(string[] args)
        {
            #region Useable
            //MultipleInsertSqlMain.CheckList(MailsList);
            //MailsList = MultipleInsertSqlMain.CheckDBVsList(ownlist: true, list: MailsList);
            //List<int> wrong = new List<int>();
            //List<string> mails = new List<string>();
            //Regex last = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            //   @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase);
            //foreach (var item in MailsList)
            //{
            //    if (!last.IsMatch(item))
            //    {
            //        wrong.Add(MailsList.IndexOf(item));
            //    }
            //}
            //foreach (var item in wrong)
            //{
            //    var s = MailsList[item];
            //} 
            #endregion
            List<Trial> keko = new List<Trial>();
            using (MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["base"].ConnectionString))
            {
                con.Open();
                string query = "select * from mails where state=0;";
                using (MySqlCommand com = new MySqlCommand(query, con))
                using (MySqlDataAdapter da = new MySqlDataAdapter(com))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Helpers.WriteSplit($"{dt.Rows[i]["address"]},{dt.Rows[i]["state"]}", '}', @"C:/Users/XD/Desktop/keko.txt");
                    }
                }
            }
        }
        public static void Fetch()
        {
            Dictionary<string, int> compAndPageCount = new Dictionary<string, int>();
            compAndPageCount.Add("https://www.goodfirms.co/big-data-analytics", 42);
            compAndPageCount.Add("https://www.goodfirms.co/artificial-intelligence", 20);
            compAndPageCount.Add("https://www.goodfirms.co/internet-of-things", 26);
            compAndPageCount.Add("https://www.goodfirms.co/augmented-virtual-reality", 10);
            compAndPageCount.Add("https://www.goodfirms.co/software-testing-companies", 46);
            compAndPageCount.Add("https://www.goodfirms.co/cloud-computing-companies", 50);
            compAndPageCount.Add("https://www.goodfirms.co/directory/platforms/top-web-design-companies", 178);
            compAndPageCount.Add("https://www.goodfirms.co/seo-agencies", 140);
            compAndPageCount.Add("https://www.goodfirms.co/directory/marketing-services/top-digital-marketing-companies", 170);
            compAndPageCount.Add("https://www.goodfirms.co/engineering", 4);
            compAndPageCount.Add("https://www.goodfirms.co/business-services", 15);
            compAndPageCount.Add("https://www.goodfirms.co/translation-services-companies", 21);
            compAndPageCount.Add("https://www.goodfirms.co/it-services", 53);
            compAndPageCount.Add("https://www.goodfirms.co/bpo", 28);
            compAndPageCount.Add("https://www.goodfirms.co/game-development-companies", 28);
            compAndPageCount.Add("https://www.goodfirms.co/writing", 11);
            compAndPageCount.Add("https://www.goodfirms.co/animation-multimedia", 6);
            compAndPageCount.Add("https://www.goodfirms.co/implementation-services", 8);
            compAndPageCount.Add("https://www.goodfirms.co/devops-companies", 19);
            compAndPageCount.Add("https://www.goodfirms.co/bot-development", 8);
            compAndPageCount.Add("https://www.goodfirms.co/robotic-process-automation", 2);
            compAndPageCount.Add("https://www.goodfirms.co/advertising-companies", 40);
            compAndPageCount.Add("https://www.goodfirms.co/hospital-management-software", 1);

            List<AddressConvertviaUri> list = new List<AddressConvertviaUri>();

            List<CompanyWebSites> cList = new List<CompanyWebSites>();

            foreach (var item in compAndPageCount)
            {
                int counter = 0;
                AddressConvertviaUri address = new AddressConvertviaUri(item.Key, item.Value);
                foreach (var eachItem in address.SiteAddresses[address.SiteAddresses.Count - 1])
                {
                    cList.Add(new CompanyWebSites(eachItem));
                }
            Aga:
                try
                {
                    while (address.NextPage())
                    {
                        counter = 0;
                        foreach (var eachItem in address.SiteAddresses[address.SiteAddresses.Count - 1])
                        {
                            cList.Add(new CompanyWebSites(eachItem));
                        }
                    }
                }
                catch
                {
                    if (counter == 0)
                    {
                        Thread.Sleep(15000);
                        counter++;
                    }
                    else if (counter == 1)
                    {
                        Thread.Sleep(20000);
                        counter++;
                    }
                    else
                    {
                        Thread.Sleep(30000);
                    }
                    goto Aga;
                }
                list.Add(address);
            }
            Task.WaitAll(cList.Select(e => e.Duty).ToArray());
        }
    }
}
