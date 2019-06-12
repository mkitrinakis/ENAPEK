using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json; 


namespace ENAREK.Helpers
{

    struct StructENAREK
    {
        public string ENAREK; 
        public string IsNew ;  
    }

    public class GetENAREK
    {
        public string byAMKAOnlyQuery(string AMKA)
        {
            string rs = getFromAMKATable(AMKA);
            return rs; 

        }

        public string byAMKAQueryUpdate(string AMKA)
        {
            StructENAREK structENAREK = new Helpers.StructENAREK(); 
            
            string rs = getFromAMKATable(AMKA);
            if (!rs.Trim().Equals(""))
            {
                structENAREK.ENAREK = rs;
                structENAREK.IsNew = "0";
                return JsonConvert.SerializeObject(structENAREK);
            }
            else
            {
                rs = insertToAMKATable(AMKA);
                structENAREK.ENAREK = rs;
                structENAREK.IsNew = "1";
                return JsonConvert.SerializeObject(structENAREK);
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

