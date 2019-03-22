using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
namespace MailSender
{
    public class SendMails
    {
        #region Public Delegate

        public delegate void OnChanged();

        #endregion

        #region Public Events

        public event OnChanged PropertyChanged;

        #endregion

        #region CurrentUser

        private UserMail User { get; }

        #endregion

        #region Event Method

        /// <summary>
        /// Informs the UI that mail is sended successfully
        /// </summary>
        private void OnPropertyChanged()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (MailState)
                {
                    MailsLoad.UIMailStateUpdateCommand.Execute(NotSentMails[0]);
                    NotSentMails[0].SetEmailState();
                    NotSentMails.RemoveAt(0);
                    MailState = false;
                }
            });
        }

        #endregion
        
        #region Mail State

        private bool mMailState = false;

        public bool MailState
        {
            get
            {
                return mMailState;
            }
            set
            {
                
                mMailState = value;
                if (value)
                {
                    PropertyChanged();
                }
            }
        }

        #endregion

        #region Not Sent List

        List<SingleMail> NotSentMails = MailsLoad.MailList.Where(single => single.Check == false).ToList();

        #endregion

        #region DispatcherTimer and Mail Process

        DispatcherTimer Timer;


        private void Timer_Tick(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {

                MailState= await Mail.Send(User, NotSentMails[0]);

            });

        }
        #endregion

        #region Constructor

        /// <summary>
        /// Creates a class which has List of not sent emails and a timer to sent them.Default timer value is 3 mins.It starts to send emails as soon as it was initialized.
        /// </summary>
        public SendMails(UserMail forWho)
        {
            #region Timer Initialization

            Timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 10)///CHANGE THIS
            };
            Timer.Tick += Timer_Tick;

            #endregion

            #region Event Initialization
            
            PropertyChanged += new OnChanged(OnPropertyChanged);

            #endregion

            #region Process Assignment

            User = forWho;
        
            Timer.Start();

            #endregion

        }

        /// <summary>
        /// Creates a class which has List of not sent emails and a timer to sent them.It starts to send emails as soon as it was initialized.
        /// </summary>
        /// <param name="period">a TimeSpan value which is set to DispatcherTimer's Interval.</param>
        /// <param name="forWho">Who is sending</param>
        public SendMails(TimeSpan period,UserMail forWho)
        {

            Timer = new DispatcherTimer()
            {
                Interval = period,
            };
            Timer.Tick += Timer_Tick;

            PropertyChanged += new OnChanged(OnPropertyChanged);

            User = forWho;

            Timer.Start();

        }

        #endregion

    }
}
