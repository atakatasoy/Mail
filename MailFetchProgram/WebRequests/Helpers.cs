using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WebRequest
{
    public static class Helpers
    {
        
        public static void ReadSplitAdd(this List<string> s,string FilePath,char character)
        {
            using (FileStream stream = new FileStream(FilePath, FileMode.Open, FileAccess.Read))
            using (StreamReader oku = new StreamReader(stream))
            {
                var content = oku.ReadToEnd();
                var list = content.Split(character).ToList();
                for(int i = 0; i < list.Count; i++)
                {
                    list[i] = list[i].ToLower();
                    if (list[i].Contains("ı")) list[i] = list[i].Replace("ı", "i");
                }
                s.AddRange(list);
            }
        }
        public static void WriteSplit(string mail, char character,string FilePath= @"C:\Users\XD\Desktop\Mails.txt")
        {
            using (FileStream stream = new FileStream(FilePath, FileMode.Append, FileAccess.Write))
            using (StreamWriter oku = new StreamWriter(stream))
            {
                oku.Write(mail + character);
            }
        }
    }
}
