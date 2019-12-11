using System.Collections.Generic;
using System.IO;
using System.Linq;
using static WebRequest.WebRequestMain;

namespace WebRequest
{
    public static class Helpers
    {
        static object obje { get; set; } = new object();
        public static void ReadSplitAdd(this List<string> s, string FilePath, char character)
        {
            using (FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            using (StreamReader oku = new StreamReader(stream))
            {
                var content = oku.ReadToEnd();
                var list = content.Split(character).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = list[i].ToLower();
                    if (list[i].Contains("ı")) list[i] = list[i].Replace("ı", "i");
                }
                s.AddRange(list);
            }
        }
        public static void ReadSplitAdd(this List<Trial> s, string FilePath, char character)
        {
            using (FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            using (StreamReader oku = new StreamReader(stream))
            {
                var content = oku.ReadToEnd();
                var list = content.Split(character).ToList();
                for (int i = 0; i < list.Count; i++)
                {
                    list[i] = list[i].ToLower();
                    if (list[i].Contains("ı")) list[i] = list[i].Replace("ı", "i");
                }
                //s.AddRange(list);
            }
        }
        public static void WriteSplit(string mail, char character,string FilePath= @"C:\Users\XD\Desktop\kemal.txt")
        {
            lock (obje)
            {
                using (FileStream stream = new FileStream(FilePath, FileMode.Append, FileAccess.Write))
                using (StreamWriter oku = new StreamWriter(stream))
                {
                    oku.Write(mail + character);
                } 
            }
        }
        public static void FindPrimaryMails(List<string> mailList)
        {
            for (int i = 0; i < mailList.Count; i++)
            {
                var AtChar = mailList[i].IndexOf('@');
                var bufferMail = mailList[i].Substring(AtChar);
                List<string> sameMailDomains = new List<string>() { mailList[i] };
                for (int j = i + 1; j < mailList.Count; j++)
                {
                    var AtChar2 = mailList[j].IndexOf('@');
                    var bufferMail2 = mailList[j].Substring(AtChar2);
                    if (bufferMail2.Contains(bufferMail)) sameMailDomains.Add(mailList[j]);
                }
                if (sameMailDomains.Count > 1)
                {
                    for(int j = 0; j < sameMailDomains.Count; j++)
                    {
                        bool success = false;
                        for(int k = 0; k < PrimaryMailSyntaxPredicts.Length; k++)
                        {
                            if (sameMailDomains[j].Contains(PrimaryMailSyntaxPredicts[k]))
                            {
                                sameMailDomains.Remove(sameMailDomains[j]);
                                for(int q = 0; q < sameMailDomains.Count; q++)
                                {
                                    mailList.Remove(sameMailDomains[q]);
                                    i = 0;
                                }
                                success = true;
                                break;
                            }
                        }
                        if (success) break;
                    }
                }
            }
        }
        public static string[] SecondaryMailSyntaxPredicts = new string[]
        {
            "info@",
            "hello@",
            "hi@",
            "contact@",
            "contactus@",
            "support@",
            "sales@"
        };
        public static string[] PrimaryMailSyntaxPredicts = new string[]
        {
            "jobs@",
            "job@",
            "careers@",
            "career@",
            "recruiter@",
            "work@",
            "hr@",
        };
    }
}
