using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    class NormalizeInput
    {
        string latins = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        string greeks = "ΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΡΣΤΥΦΧΨΩ";

        ArrayList matches = new ArrayList();

        public void run()
        {
            init();
            string line = "";
            System.IO.StreamReader fileInput = new System.IO.StreamReader(@"input.txt");
            System.IO.StreamWriter fileOutput = new System.IO.StreamWriter(@"output.txt");
            while ((line = fileInput.ReadLine()) != null)
            {
                string rs = processLine(line);
                fileOutput.WriteLine(rs);
            }
            fileInput.Close();
            fileOutput.Close();
        }

        private void init()
        {
            matches.Add("AΑ");
            matches.Add("BΒ");
            matches.Add("DΔ");
            matches.Add("EΕ");
            matches.Add("HΗ");
            matches.Add("IΙ");
            matches.Add("KΚ");
            matches.Add("MΜ");
            matches.Add("NΝ");
            matches.Add("OΟ");
            matches.Add("PΡ");
            matches.Add("SΣ");
            matches.Add("TΤ");
            matches.Add("XΧ");
            matches.Add("YΥ");
            matches.Add("ZΖ");
        }

        private  string processLine(string line)
        {
            string[] parts = line.Split(',');
            string[] rs = new string[8];
            for (int i = 0; i <= 7; i++)
            {
                try
                {
                    if (i <= 3 || i.Equals(5))
                    {
                         rs[i] = transform(parts[i]);
                        // rs[i] = parts[i];
                    }
                    else rs[i] = parts[i];
                }
                catch (Exception e) { Console.WriteLine("err on:" + line + "-->" + e.Message); }
            }
            return String.Join(";", rs);
        }

        private  string transform(string src)
        {
            string rs = canonicalize(src);
            if (isGreek(rs)) { rs = latinToGreek(rs); }
            else
            {
                rs = greekToLatin(rs);
            }
            return rs;
        }


        private string canonicalize(string src)
        {
            string rs = src.Trim().ToUpper();
            rs = rs.Replace("Ά", "Α").Replace("Έ", "Ε").Replace("Ή", "Η").Replace("Ί", "Ι").Replace("Ό", "Ο").Replace("Ώ", "Ω").Replace("Ύ", "Υ");
            rs = rs.Replace("_", "-");
            while (rs.Contains(" -")) { rs = rs.Replace(" -", "-"); }
            while (rs.Contains("- ")) { rs = rs.Replace("- ", "-"); }
            while (rs.Contains("--")) { rs = rs.Replace("--", "-"); }
            rs = rs.Replace(" ", "-"); 
            return rs;

        }


        bool isGreek(string src)
        {
            int latins = 0;
            int greeks = 0;
            bool lastCharIsGreek = false;
            foreach (char c in src)
            {
                if (isGreekChar(c)) { greeks++; lastCharIsGreek = true; }
                if (isLatinChar(c)) { latins++; lastCharIsGreek = false; }
            }
            if (greeks > latins + 2) return true;
            if (latins > greeks + 2) return false;
            return lastCharIsGreek;
        }

        bool isGreekChar(char c)
        { return greeks.Contains(c); }


        bool isLatinChar(char c)
        {
            return latins.Contains(c);
        }

        string greekToLatin(string src)
        {
            string rs = src;
            foreach (string match in matches)
            {
                rs = rs.Replace(match.Substring(1), match.Substring(0, 1));
            }
            return rs;
        }

        string latinToGreek(string src)
        {
            string rs = src;
            foreach (string match in matches)
            {
                rs = rs.Replace(match.Substring(0, 1), match.Substring(1));
            }
            return rs;
        }
    }
}
