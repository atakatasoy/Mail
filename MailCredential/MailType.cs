using System.Collections.Generic;

namespace MailSender
{
  public static class MailTypeSelector
    {
        #region Helper


        public static readonly Dictionary<MailTypeSelect, string> MailType = new Dictionary<MailTypeSelect, string> { { MailTypeSelect.Gmail, "smtp.gmail.com" }, { MailTypeSelect.Hotmail, "smtp.live.com" } };

        public static string GetMailType(this Dictionary<MailTypeSelect, string> List, MailTypeSelect type)
        {
            return MailType[type];
        }


        #endregion
    }
    public enum MailTypeSelect
    {
        Hotmail,
        Gmail,
    }
}
