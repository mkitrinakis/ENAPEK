using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json; 


namespace ENAPEK.Helpers
{

    struct StructENAPEK
    {
        public string ENAREK; 
        public string IsNew ;  
    }

    public class GetENAPEK
    {
        public string byAMKAOnlyQuery(string AMKA)
        {
            string rs = getFromAMKATable(AMKA);
            return rs; 

        }

        public string byAMKAQueryUpdate(string AMKA)
        {
            StructENAPEK structENAPEK = new Helpers.StructENAPEK(); 
            
            string rs = getFromAMKATable(AMKA);
            if (!rs.Trim().Equals(""))
            {
                structENAPEK.ENAREK = rs;
                structENAPEK.IsNew = "0";
                return JsonConvert.SerializeObject(structENAPEK);
            }
            else
            {
                rs = insertToAMKATable(AMKA);
                structENAPEK.ENAREK = rs;
                structENAPEK.IsNew = "1";
                return JsonConvert.SerializeObject(structENAPEK);
            }

        }


        private string getFromAMKATable(string AMKA)
        {
            int milli = DateTime.Now.Millisecond;
            if (milli > 50) { return ""; }
            else { return "A" + (milli.ToString()); }
        }

        private string insertToAMKATable(string AMKA)
        {
            int milli = DateTime.Now.Millisecond;
            return "I_A" + milli.ToString();
        }

        }
    }

