using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebRequest
{
    class FromtxtToDb
    {
        public void Process()
        {
            List<string> webSites = new List<string>();
            webSites.ReadSplitAdd(@"C:\Users\XD\Desktop\Websites.txt", ',');
            webSites.ReadSplitAdd(@"C:\Users\XD\Desktop\AppDevelopment.txt", ',');
            webSites.ReadSplitAdd(@"C:\Users\XD\Desktop\WebSiteDevelopment.txt", ',');
            webSites.ReadSplitAdd(@"C:\Users\XD\Desktop\E-Commerce.txt", ',');
            MultipleInsertSql.MultipleInsertSqlMain.CheckList(webSites);
            MultipleInsertSql.MultipleInsertSqlMain.AddDb(webSites.ToArray());
        }
    }
}
