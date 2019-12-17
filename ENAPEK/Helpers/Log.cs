using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;  


namespace ENAREK.Helpers
{
    public static class Log
    {
         // static string path = "c:\\test\\log1.txt"; 
       //  static string path = "log2.txt";
        public   static void write(string msg)
        {
            string path = System.Web.Configuration.WebConfigurationManager.AppSettings["LogFile"];

            //  Console.WriteLine(msg); 
            Console.WriteLine(msg);
            if (!(path ?? "").Trim().Equals(""))
            using (StreamWriter w = File.AppendText(path))
            {
                w.WriteLine(DateTime.Now.ToString() + "   " + msg);
                w.Close();
            }
        }
    }
}