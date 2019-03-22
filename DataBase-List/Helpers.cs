using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;

namespace MailSender
{
    
    public static class Helpers
    {

        #region Private Connection

        private static MySqlConnection Connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["Root"].ConnectionString);

        #endregion

        #region Private Properties

        private static int TryDataBaseCounter { get; set; } = 0;

        private static int TryMailCounter { get; set; } = 0;

        #endregion

        #region List Processes

        /// <summary>
        /// Gets the information from the server.
        /// </summary>
        /// <returns>Returns as a DataTable</returns>
        public static DataTable GetMails()
        {
            try
            {
                string sorgu = "SELECT * FROM mails;";
                using (DataTable data = new DataTable())
                using (MySqlDataAdapter comd = new MySqlDataAdapter(new MySqlCommand(sorgu, Connection)))
                {
                    Connection.Open();
                    comd.Fill(data);
                    Connection.Close();
                    return data;
                }
            }
            catch
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
                if (TryDataBaseCounter < 3)
                {
                    TryDataBaseCounter++;
                    return GetMails();
                }
                TryDataBaseCounter = 0;
                return null;
            }
          
        }

        /// <summary>
        /// Defined List processes
        /// </summary>
        /// <param name="MailList">Fills the specified list with email addresses in server.</param>
        public static void FillList(this ObservableCollection<SingleMail> MailList)
        {
            using (var data = GetMails())
            {
                if (data != null)
                {
                    for (int i = 0; i < data.Rows.Count; i++)
                    {
                        MailList.Add(new SingleMail()
                        {
                            ID = Convert.ToInt32(data.Rows[i]["ID"]),
                            Email = data.Rows[i]["address"].ToString(),
                            Check = Convert.ToBoolean(data.Rows[i]["state"])
                        });
                    }
                }
            }

        }

        #endregion

        #region Mail Processes

        /// <summary>
        /// Changing Mail state of database value
        /// </summary>
        /// <param name="value">SingleMail which is marked as sent</param>
        /// <returns>true-false</returns>
        public static bool SetEmailState(this SingleMail value)
        {
            try
            {
                string sorgu = "UPDATE mails SET state=@state WHERE ID=@id;";
                using(MySqlCommand comd=new MySqlCommand(sorgu, Connection))
                {
                    Connection.Open();
                    comd.Parameters.AddWithValue("@state", true);
                    comd.Parameters.AddWithValue("@id", value.ID);
                    comd.ExecuteNonQuery();
                    Connection.Close();
                    return true;
                }
            }
            catch
            {
                if (Connection.State == ConnectionState.Open) Connection.Close();
                if (TryMailCounter < 3)
                {
                    TryMailCounter++;
                    return SetEmailState(value);
                }
                TryMailCounter = 0;
                return false;
            }
        }

        #endregion
    }
}
