using System;
using System.Collections.Generic;

namespace WebRequest
{
    public static class DomainAlgorithm
    {
        public static List<string> Domains = new List<string>();

        static DomainAlgorithm()
        {
            Domains.ReadSplitAdd(@"C:\Users\XD\Desktop\Domain.txt", ',');
        }

        public static string MailDomainControlAlgorithm(string Text,string predict)
        {
            try
            {
                foreach (var domainExt in Domains)
                {
                    if (Text.Contains(domainExt))
                    {
                        var result = Text.Substring(Text.LastIndexOf(predict));
                        if (result.Contains(domainExt))
                        {
                            return result.Substring(0, result.IndexOf(domainExt) + domainExt.Length);
                        }
                        else
                        {
                            return Text.Substring(Text.IndexOf(predict), Text.IndexOf(domainExt) + domainExt.Length);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                var xd = e.Message;
                return string.Empty;
            }
            return string.Empty;
        }

    }
}
