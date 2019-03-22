using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using System.Linq.Expressions;
using System.Linq;
using System;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace MailSender
{
   
    public class MailsLoad : INotifyPropertyChanged
    {
       
        #region Public Properties

        public static ObservableCollection<SingleMail> MailList { get; set; } = new ObservableCollection<SingleMail>();

        #endregion

        #region Commands

        /// <summary>
        /// Changes the state value of UI
        /// </summary>
        public static ICommand UIMailStateUpdateCommand { get; set; }

        /// <summary>
        /// Starts the Sending process
        /// </summary>
        public static ICommand Start { get; set; }

        #endregion

        #region Public Events

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        #endregion

        #region Constructor

        public MailsLoad()
        {
            MailList.FillList();

            UIMailStateUpdateCommand = new ParameterizedRelayCommand((singleMail) =>
            {

                var mSingleMail = singleMail as SingleMail;

                if (mSingleMail != null && mSingleMail.Check != true)
                {
                    var que = MailList.Where(sent => mSingleMail.ID == sent.ID).Single().ID - 1;
                    MailList[que].Check = true;
                    MailList[que] = new SingleMail(MailList[que]);
                }
            });

            Start = new RelayCommand(() =>
            {
                SendMails startProcess = new SendMails(new UserMail("atakemalatasoy@hotmail.com.tr","asdasd"));
            });
        }


        #endregion

    }
}
