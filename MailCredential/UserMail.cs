using System.Collections.Generic;

namespace MailSender
{
    public class UserMail
    {
        
        #region Private Properties

        public string Email { get; }

        public string Password { get;}

        public MailTypeSelect Type { get; }

        #endregion

        #region Constructor

        public UserMail(string email,string password)
        {
            this.Email = email;
            this.Password = password;
            if (email.Contains("hotmail")) Type = MailTypeSelect.Hotmail;
            else if (email.Contains("gmail")) Type = MailTypeSelect.Gmail;
        }

        #endregion

        #region MailTypeHelper

        public string GetUserMailType()
        {
            return MailTypeSelector.MailType.GetMailType(this.Type);
        }

        #endregion

     
    }
}
