using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;  


namespace ENAPEK.Helpers
{
    public static class Log
    {
        static string path = "c:\\test\\log1.txt"; 
     public   static void write(string msg)
        {
            Console.WriteLine(msg);
            using (StreamWriter w = File.AppendText(path))
            {
                w.WriteLine(DateTime.Now.ToString() + "   " + msg);
                w.Close(); 
            }
        }
    }
}