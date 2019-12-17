using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Text ; 
using Newtonsoft.Json;

using ENAREK.Helpers;


namespace ENAREK.Helpers
{
    public class CallWebService
    {
        struct RequestStruct
        {
            public string amka;
            public string surname;
            public string firstname;
            public string fathername;
            public string mothername;
            public string birthdate;
        }





        public string pump(ENAREK.Models.GetENAREKModelTest m)
        {

            // string encoded = HelpUtils.return___PRODUCTION___Authorization();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // string q = "http://localhost:21801/api/ENAREK/";

            string q = System.Web.Configuration.WebConfigurationManager.AppSettings["WebServiceURL"];

            var request = WebRequest.Create(q);
            request.Method = "POST";
            request.ContentType = "application/json";
            // HDIKACalls.getENAREK(true, request.amka, request.surname, request.firstname, request.fathername, request.mothername, request.birthdate);
            RequestStruct rs = new Helpers.CallWebService.RequestStruct() { amka = m.AMKA, firstname = m.FirstName, surname = m.SurName, fathername = m.FatherName, mothername = m.MotherName, birthdate = myDate(m.BirthDate) };
            string postData = JsonConvert.SerializeObject(rs);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentLength = byteArray.Length;
            // Write the data to the request stream.  
            Stream dataStream = request.GetRequestStream();
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
                                return ("!Status:" + response.StatusCode + "-" + "-StatusDescription:" + response.StatusDescription + "-flag:" + json.flag + "-Error:" + (json.error ?? "NULL") + "-ENAREK:" +  json.ENAREK);
                            }
                            catch (Exception e) { return ("-->INNER SYSTEM ERROR :" + e.Message); }
                        }
                    }
                }
            }
            catch (Exception e) { return ("-->OUTER SYSTEM ERROR :" + e.Message); }
        }
        //    Console.WriteLine(content);


        private string myDate(object  val)
        {
            try
            {
                DateTime dval = (DateTime)val;
                return dval.Day.ToString().PadLeft(2, '0') + "/" + dval.Month.ToString().PadLeft(2, '0') + "/" + dval.Year;
            }
            catch { return val.ToString();  }
        }

    }
}