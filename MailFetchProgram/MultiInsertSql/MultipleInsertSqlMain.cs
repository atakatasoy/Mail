using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;

namespace MultipleInsertSql
{
    public class MultipleInsertSqlMain
    {
        public static void Main(string[] args)
        {
            //txtden oku string[] halde AddDBmailse yolla;(old)
        }
        public static void AddDBmails(List<string> ayrilmis)
        {
            MySqlConnection qq = new MySqlConnection(ConfigurationManager.ConnectionStrings["base"].ConnectionString);
            string sorgu = "INSERT INTO mails (address,state) VALUES(";
            for (int i = 0; i < ayrilmis.Count; i++)
            {
                
                if (i == 0)
                {
                    sorgu += $"'{ayrilmis[i]}',{false})";
                    if (ayrilmis.Count == 1) sorgu += ";";
                    else sorgu += ",";
                    continue;
                }
                else
                {
                    if (i == ayrilmis.Count - 1)
                    {
                        sorgu += $"('{ayrilmis[i]}',{false});";
                        continue;
                    }
                    sorgu += $"('{ayrilmis[i]}',{false}),";
                }

            }
            using (qq)
            {
                if (ayrilmis.Count > 0)
                {
                    qq.Open();

                    using (MySqlCommand comd = new MySqlCommand(sorgu, qq))
                    {
                        comd.ExecuteNonQuery();
                    }

                    qq.Close();
                }

            }
        }
        public static void AddDb(string[] ayrilmis)
        {
            MySqlConnection qq = new MySqlConnection(ConfigurationManager.ConnectionStrings["base"].ConnectionString);
            string sorgu = "INSERT INTO it_inf (webSite,jobs_mail) VALUES(";
            for (int i = 0; i < ayrilmis.Length; i++)
            {
                if (i == 0)
                {
                    sorgu += $"'{ayrilmis[i]}','{string.Empty}')";
                    if (ayrilmis.Length == 1) sorgu += ";";
                    else sorgu += ",";
                    continue;
                }
                else
                {
                    if (i == ayrilmis.Length - 1)
                    {
                        sorgu += $"('{ayrilmis[i]}','{string.Empty}');";
                        continue;
                    }
                    sorgu += $"('{ayrilmis[i]}','{string.Empty}'),";
                }

            }
            using (qq)
            {
                if (ayrilmis.Length > 0)
                {
                    qq.Open();

                    using (MySqlCommand comd = new MySqlCommand(sorgu, qq))
                    {
                        comd.ExecuteNonQuery();
                    }

                    qq.Close();
                }

            }
        }
        public static List<string> CheckDBVsList(string FilePath=@"C:\Users\XD\Desktop\Mails.txt", bool ownlist=false,List<string> list=null,bool SelfCheck=true,char txtsplitcharacter=',')
        {
            MySqlConnection Connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["base"].ConnectionString);
            List<string> ayrilmis;
            if (!ownlist)
            {
                string mailler;
                using (FileStream dosya = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
                using (StreamReader oku = new StreamReader(dosya))
                {
                    mailler = oku.ReadToEnd();
                }
                ayrilmis = mailler.Split(txtsplitcharacter).ToList();
            }
            else
            {
                ayrilmis = list;
            }
            //Checks list if there are repeated values
            if (SelfCheck) CheckList(ayrilmis);

            //Checks List with Db if there are same values 
            using (MySqlDataAdapter com=new MySqlDataAdapter(new MySqlCommand("SELECT * FROM mails", Connection)))
            {
                using(DataTable mails = new DataTable())
                {
                    com.Fill(mails);
                    for (int i = 0; i < mails.Rows.Count; i++)
                    {
                        mails.Rows[i]["address"] = mails.Rows[i]["address"].ToString().Trim();
                        mails.Rows[i]["address"] = mails.Rows[i]["address"].ToString().ToLower();
                        if (mails.Rows[i]["address"].ToString().Contains("ı")) mails.Rows[i]["address"] = mails.Rows[i]["address"].ToString().Replace("ı", "i");
                        for (int q = 0; q < ayrilmis.Count; q++)
                        {
                            ayrilmis[q] = ayrilmis[q].Trim();
                            ayrilmis[q] = ayrilmis[q].ToLower();
                            if (mails.Rows[i]["address"].ToString()==ayrilmis[q])
                            {
                                ayrilmis.RemoveAt(q);
                                q--;
                                continue;
                            }
                        }
                       
                    }
                }
            }
            return ayrilmis;
        }
        public static void CheckList(List<string> ayrilmis)
        {
            for (int i = 0; i < ayrilmis.Count; i++)
            {
                ayrilmis[i] = ayrilmis[i].Trim();
                for (int q = 0; q < ayrilmis.Count; q++)
                {
                    ayrilmis[q] = ayrilmis[q].Trim();
                    if (ayrilmis[i] == ayrilmis[q] && i != q)
                    {
                        ayrilmis.RemoveAt(q);
                        q = 0;
                    }
                }
            }
        }
    }
}
