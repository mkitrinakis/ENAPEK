using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENAPEK.Helpers
{
    public static class Core
    {
        public static string dateToString(object obj)
        {
            try
            {
                DateTime dt = (DateTime)obj;
                return dt.Day + "/" + dt.Month + "/" + dt.Year;
            }
            catch (Exception e) { return obj.ToString(); }
        }



        public static string canonicalize(string src)
        {
            string rs = src.Trim().ToUpper();
            rs = rs.Replace("Ά", "Α").Replace("Έ", "Ε").Replace("Ή", "Η").Replace("Ί", "Ι").Replace("Ό", "Ο").Replace("Ύ", "Υ").Replace("Ώ", "Ω").Replace("Ϊ", "Ι").Replace("Ϋ", "Υ");
            rs = rs.Replace("_", "-");
            while (rs.Contains(" -")) { rs = rs.Replace(" -", "-"); }
            while (rs.Contains("- ")) { rs = rs.Replace("- ", "-"); }
            while (rs.Contains("--")) { rs = rs.Replace("--", "-"); }
            rs = rs.Replace(" ", "-");
            return rs;
        }

    }
}