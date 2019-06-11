using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENAPEK.Helpers
{
    public static class RandomGenerator
    {

        static  Random random = new Random(); 
        public static int counter = 0;

        public static string getID()
        {
            int num = random.Next(100000000);
           
            string sNum = num.ToString().PadLeft(8);

            int checksum = getChecksum(sNum);
            if (checksum != -1)
            {
                return num.ToString() + checksum.ToString();
            }
            else
            {
                return ""; 
            }
        }


        private static int getChecksum(string sNum)
        {
            if (sNum.Length != 8) return -1;
            int multFactor = 1;
            double sum = 0;
            for (int i = 7; i >= 0; i--)
            {
                multFactor *= 2;
                char c = sNum[i];
                int cInt = Convert.ToInt32(c);
                int cMult = cInt * multFactor;
                sum += cMult;

            }
            int checksum = (int)((sum / 11) % 1 * 10); // divide by 11, and get the first digit of the decimal part. 
            return checksum; 
        }


        public static bool checkID(string src)
        {
            if (src.Length != 9) return false;
            string srcOriginal = src.Substring(0, src.Length - 1);
            int checksum = getChecksum(srcOriginal);
            string c = src[8].ToString();
            return (c.Equals(checksum.ToString())); 

        }


    }

}