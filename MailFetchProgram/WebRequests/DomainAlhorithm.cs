using System.Collections.Generic;

namespace WebRequest
{
    public static class DomainAlhorithm
    {
        public static List<string> Domains = new List<string>();

        static DomainAlhorithm()
        {
            Domains.ReadSplitAdd(@"C:\Users\XD\Desktop\Domains.txt", ',');
        }

        public static string MailDomainControlAlgorithm(string Text,string predict)
        {
            foreach (var domainExt in Domains)
            {
                if (Text.Contains(domainExt))
                {
                    var result=Text.Substring(Text.LastIndexOf(predict), Text.IndexOf(domainExt) + domainExt.Length);
                    return result;
                }
            }
            return string.Empty;
        }

    }
}
