using System.Collections.Specialized;
using System.ComponentModel;

namespace MailSender
{
    public class SingleMail
    {

        #region Public Properties

        public int ID { get; set; }
        public string Email { get; set; }
        public bool Check { get; set; }
        public string Feedback { get; set; }

        #endregion

        #region Constructors

        public SingleMail(SingleMail another)
        {
            this.ID = another.ID;
            this.Email = another.Email;
            this.Check = another.Check;
            if (another.Feedback != null)
                this.Feedback = another.Feedback;
        }

        public SingleMail()
        {

        }

        #endregion

    }
}
