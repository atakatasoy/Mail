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

namespace WebRequest
{ 
    class WebRequestMain
    {
        static void Main(string[] args)
        {
            List<int> wrong = new List<int>();
            List<string> mails = new List<string>();
            mails.ReadSplitAdd(@"C:\Users\XD\Desktop\Mails.txt", ',');
            var SalesList = MultipleInsertSql.MultipleInsertSqlMain.CheckDBVsList(ownlist:true,list:mails);
            Regex last = new Regex(@"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
               @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase);
            foreach (var item in SalesList)
            {
                if (!last.IsMatch(item))
                {
                    wrong.Add(SalesList.IndexOf(item));
                }
            }
            foreach (var item in wrong)
            {
                var s = SalesList[item];
            }
            MultipleInsertSql.MultipleInsertSqlMain.AddDBmails(SalesList);
            List<CompanyWebSites> CompanyList = new List<CompanyWebSites>();
            MySqlConnection con = new MySqlConnection(ConfigurationManager.ConnectionStrings["base"].ConnectionString);
            string sorgu = "SELECT * FROM it_inf;";
            con.Open();
            using (MySqlDataAdapter reader = new MySqlDataAdapter(new MySqlCommand(sorgu, con)))
            using (DataTable x = new DataTable())
            {
                reader.Fill(x);
                con.Close();
                for(int i = 0; i < x.Rows.Count; i++)
                {
                    CompanyList.Add(new CompanyWebSites(x.Rows[i]["webSite"].ToString()));
                }
            }
            Task.WaitAll(CompanyList.Select(task=>task.Duty).ToArray());
        }
       
    }
}
