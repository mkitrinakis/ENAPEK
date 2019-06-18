using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ENAREK.Helpers;
using System.IO;
using System.Net;
using Newtonsoft.Json;


// Token 50  403 forbidden 


// 50 κλήσεις --> Throttled you Token Generation

namespace Tasks
{
    class TESTHDIKACalls
    {
        public void run()
        {
            Log.write("TESTHDIKACalls - starts");
            List<int> loops = new List<int>();
            for (int i = 0; i <= 100; i++) loops.Add(i);

            //Parallel.ForEach(loops, (loop) =>
            //{
            //    testLog("starting:" + loop.ToString());
            //    pump(loop); 
            //});


            for (int i=0; i<=100; i++)
            {
                testLog("starting:" + i.ToString());
                pump(i);
            }

        }


        struct RequestStruct
        {
            public string amka;
            public string surname;
            public string firstname;
            public string fathername;
            public string mothername;
            public string birthdate;
        }

        private RequestStruct initRequestStruct(bool correct)
        {
            RequestStruct rs = new Tasks.TESTHDIKACalls.RequestStruct();
            rs.amka = "14087200714"; 
            rs.birthdate = "14/08/1972";
            rs.fathername = "Angelis";
            rs.surname = "ΚΙΤΡΙΝΑΚΗΣ";
            rs.mothername = "ΕΥΘΑΛΙΑ";
            rs.firstname = correct ? "ΜΑΡΚΟΣ" : "AGAUHMOS";
            return rs; 


        }

        public void pump(int id)
        {
            
            // string encoded = HelpUtils.return___PRODUCTION___Authorization();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string q = "http://localhost:21801/api/ENAREK/";
            System.Threading.Thread.Sleep(id*100);
            var request = WebRequest.Create(q);
            request.Method = "POST";
            request.ContentType = "application/json";
            // HDIKACalls.getENAREK(true, request.amka, request.surname, request.firstname, request.fathername, request.mothername, request.birthdate);
            RequestStruct rs = ((id % 1).Equals(1)) ? initRequestStruct(true) : initRequestStruct(false);
            string postData = JsonConvert.SerializeObject(rs); 
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            // Write the data to the request stream.  
            dataStream.Write(byteArray, 0, byteArray.Length);
            // Close the Stream object.  
            dataStream.Close();
            //  request.Headers.Add("Authorization", "Basic " + encoded);
            
            try
            {
                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    using (var stream = response.GetResponseStream())
                    {
                        using (var sr = new StreamReader(stream))
                        {
                            try
                            {
                                string content = sr.ReadToEnd();
                                HDIKACalls.StructENAREKResponse json = JsonConvert.DeserializeObject<HDIKACalls.StructENAREKResponse>(content);
                                testLog("id:" + id + "!!-->Status:" + "-" + response.StatusCode + "-" + response.StatusDescription + "--flag--Error-ENAREK--?" + json.flag + "-" + (json.error ?? "-") + json.ENAREK);
                                //testLog("id:" + id + "-->flag:" + json.flag);
                                //testLog("id:" + id + "-->error:" + (json.error ?? "-"));
                                //testLog("id:" + id + "-->ENAREK:" + json.ENAREK);
                            }
                            catch (Exception e) { testLog("id:" + id + "-->INNER SYSTEM ERROR :" + e.Message); }
                        }
                    }
                }
            }
            catch (Exception e) { testLog("id:" + id + "-->OUTER SYSTEM ERROR :" + e.Message); }
        }
        //    Console.WriteLine(content);



        private string mystr(string src)
        {
            return src;
        }

        private void testLog(string src)
        {
            Console.WriteLine(System.DateTime.Now + ": " + src);
        }

    }
}

