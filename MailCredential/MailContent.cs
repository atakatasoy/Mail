using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MailSender
{
    public static class Mail
    {

        
        public static async Task<bool> Send(UserMail User,SingleMail userMail)
        {
            SmtpClient sc = new SmtpClient
            {
               Port = 587,
               Host =User.GetUserMailType(),
               EnableSsl = true,
               Timeout = 50000,
               Credentials = new NetworkCredential(User.Email, User.Password)//User section
            };
            MailMessage mail2 = new MailMessage()
            {
                From = new MailAddress(User.Email, "Intern"),//User section
                Subject = "Internship",
                IsBodyHtml = true,
                Body = "....",
            };
            mail2.To.Add("atakemalatasoy@hotmail.com.tr");//userMail section
            mail2.Attachments.Add(new Attachment(@"C:\Users\XD\Desktop\Staj\Resume.pdf"));
            try
            {
                //sc.Send(mail2);
                await Task.Delay(100);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
