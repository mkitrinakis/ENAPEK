using System.IO ;
using System; 

using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;
using ENAREK.Helpers; 



namespace ENAREK.Helpers
{
    public static class HDIKACalls
    {
        private static readonly HttpClient clientAuth = new HttpClient();
        private static readonly HttpClient client = new HttpClient(); 


        //public struct HDIKAEntry
        //{
        //    public bool Found;
        //    public string AMKA;
        //    public string FirstName;
        //    public string LastName;
        //    public string MotherName;
        //    public string FatherName;
        //    public string BirthDate;
        //    public string Id; 
        //}

        //struct Auth
        //{
        //    public string username;
        //    public string password;
        //    public string grant_type; 
        //}
        private static Dictionary<string, string> valuesAuth = new Dictionary<string, string>
            {
   { "username", "edutech" },
   { "password", "edUt3ch!555@!#$" },
   { "grant_type", "password" }
};
        

        // public static string token = getAuthToken(); 
        // public static string token = "6acb10b1-3c50-34bf-8976-9c7a2cac8cb9"; 
        public static string token = ""; 
      

        struct StructOAUTHResponse
        {
            public bool success;
            public string timestamp;
            public StructOAUTH OAUTH; 
        }

        struct StructOAUTH
        {
            public string access_token;
            public string scope;
            string token_type;
            public int expires_in; 
        }

        public class StructAMKADetailsResponse
        {
            public void IsENAREKError(string msg)
            {
                ServiceCallID = (ServiceCallID ?? "") + "#" + msg;
                success = false;
                code = -999;   // custome code
            }
            public string getError()
            {
                return ServiceCallID.Substring(ServiceCallID.IndexOf('#') + 1); 
            }
            public string ServiceCallID;
            public int code;
            public bool success;
            public StructAMKADetails Result;

        }


        public class StructAMKADetails
        {
            public StructAMKADetails()
            {
                
                birth_country = "";
                birth_country_code = ""; 
                birth_date = "";
                birth_municipality_greek_code = "";
                citizenship = "";
                citizenship_code = "";
                death_note = "";
                father_en = "";
                father_gr = "";
                id_type = "";
                last_mod_date = "";
                match = false;
                sex = "";
                ssn = "";
                surname_birth_en = "";
                surname_birth_gr = "";
                surname_cur_en = "";
                surname_cur_gr = "";
                tid = ""; 
            }

            public void initFromOtherAMKADetails(StructAMKADetails src)
            {
                surname_cur_en = src.surname_cur_en;
                surname_cur_gr = src.surname_cur_gr;
                father_en = src.father_en;
                father_gr = src.father_gr;
                name_en = src.name_en;
                name_gr = src.name_gr;
                mother_en = src.mother_en;
                mother_gr = src.mother_gr; 
                birth_date = src.birth_date;
            }
            public string birth_municipality_greek_code;
            public string birth_date;
            public string surname_cur_en;
            public string surname_cur_gr;
            public string surname_birth_en;
            public string surname_birth_gr;
            public string name_en;
            public string name_gr;
            public string father_en;
            public string father_gr;
            public string mother_en;
            public string mother_gr;
            public string tid;
            public string birth_country;
            public string ssn;
            public string birth_country_code;
            public string last_mod_date;
            public string id_type;
            public string death_note;
            public string citizenship;
            public string sex;
            public bool match; 
            public string citizenship_code;

        }

      


        // store: https://api.interoperability.gr/


       public struct StructENAREKResponse
        {
            public int flag;  // -3.x--> Wrong Input (Validation from Service) -2 --> HDIKA returned no match , -1 --> HDIKA returned system error error , 0 --> ENAREK returned system error ,1--> success 2 --> success but we find mismatches HDIKA Returned AMKA mismatch
            
            public string error;   // only for <=0 
            public string ENAREK;   // returns only on pure success (1) 
            public StructAMKADetails AMKADetails; // returns only on success (1 or 2) 

        }

       
        public static StructAMKADetailsResponse getAMKADetails(bool firstTry, string amka, string surname)
        {
            // returns a json representation of structAMKADetailsResponse 
            Dictionary<string, string> valuesAMKA = new Dictionary<string, string>
                {
                    // { "amka", "14087200714" },{ "surname", "ΚΙΤΡΙΝΑΚΗΣ" },
                    { "amka", amka },
                    { "surname", surname },
                    { "extendedinfo", "extended" },
   { "gdpruser", "edutech" },
   { "gdprpass", "edUt3ch!555@!#$" }
            };

            StructAMKADetailsResponse rs = new StructAMKADetailsResponse(); 
            string urlAMKA = "https://gateway.interoperability.gr/amkacheck/1.0.1/";
            Log.write("starting getAMKA");
         
            if (token.Equals("") || token.StartsWith("ERROR")) token = getAuthToken();
            if (token.Equals("") || token.StartsWith("ERROR"))
            {
                rs.IsENAREKError("ERROR ==> ENAREK.Helpers.HDIKACalls: Error on Authentication token: " + token);
               
                return rs; 
            }
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Log.write("token:" + token);
            urlAMKA += valuesAMKA["amka"] + "/";
            urlAMKA += HttpUtility.UrlEncode(valuesAMKA["surname"]) + "/";
            urlAMKA += "extended?";
            urlAMKA += "gdpruser=" + valuesAMKA["gdpruser"] + "&";
            urlAMKA += "gdprpass=" + HttpUtility.UrlEncode(valuesAMKA["gdprpass"]);
            Log.write("getting from url:" + urlAMKA);
            string err = "1";
            try
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, urlAMKA))
                {

                    // request.Content = httpContent;
                    using (HttpResponseMessage result = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead).Result)
                    {
                        err += "4";
                        
                        
                        if (result.StatusCode.ToString().Equals("Unauthorized"))
                        {

                            Log.write("unauthorized");
                            if (firstTry)
                            {
                                //   
                                token = getAuthToken();
               
                            }
                            else
                            {
                                Log.write("Ooops! not authenticated");
                                rs.IsENAREKError("ERROR ==> ENAREK.Helpsers.HDIKACalls: System Error: not authenticated");
                                return rs;
                            }
                        }
                        if (result.IsSuccessStatusCode)
                        {
                            var resultContent = result.Content;
                            string responseString = resultContent.ReadAsStringAsync().Result;
                            Log.write(responseString);
                            
                            try
                            {
                                rs = JsonConvert.DeserializeObject<StructAMKADetailsResponse>(responseString);
               
                                return rs; 
                            }
                            catch (Exception exc)
                            {
                                rs.IsENAREKError("ERROR ==> ENAREK.Helpsers.HDIKACalls: Cannot deserialize response: " + responseString);
                                return rs; 
                            }
                            
                        }
                        else
                        {
                            rs.IsENAREKError("ERROR ==> ENAREK.Helpsers.HDIKACalls: Call To HDIKA returned false with status code: " + result.StatusCode);
                            return rs;
                        }
                        }
                    }
                }
           
            catch (System.Exception exc)
            {
                rs.IsENAREKError("ERROR ==> ENAREK.Helpsers.HDIKACalls: " + exc.Message);
                
                return rs;
            }
        }
       

        public static StructENAREKResponse getENAREK(bool firstTry, string amka, string surname, string firstname, string fathername, string mothername, string birthdate)
        {
            //returns a json represenation of structENAREKResponse 
           
                StructENAREKResponse response = new StructENAREKResponse();
            amka = (amka ?? ""); 
            surname = (surname ?? "");
            firstname = (firstname ?? "");
            fathername = fathername ?? "";
            mothername = mothername ?? "";
            birthdate = birthdate ?? ""; 

            try
            {
                int isAMKA = validateAMKA(amka, surname); 
                    
                    if (isAMKA<0)
                    {
                    response.flag = isAMKA;
                    if (isAMKA.Equals(-31) || isAMKA.Equals(-32))
                    {
                        response.error = "Το ΑΜΚΑ πρέπει να αποτελείται από 11 ψηφία και μόνο";
                    }
                    if (isAMKA.Equals(-33)) { response.error = "Το επίθετο του πολίτη δεν μπορεί να είναι κενό"; }
                        response.ENAREK = "";
                        response.AMKADetails = new Helpers.HDIKACalls.StructAMKADetails();
                        Log.write("** getENAREK:" + JsonConvert.SerializeObject(response));
                        return response;
                    }
                
                

                StructAMKADetailsResponse amkaDetailsResponse = getAMKADetails(firstTry, amka, surname);
                if (!amkaDetailsResponse.success)
               
                {
                    response.flag = -1; // system Error from HDIKA
                    response.error = amkaDetailsResponse.ServiceCallID;
                    response.ENAREK = "";
                    response.AMKADetails = new Helpers.HDIKACalls.StructAMKADetails();
                    Log.write("** getENAREK:" + JsonConvert.SerializeObject(response)); 
                    return response; 
                }
              //  StructAMKADetailsResponse amkaDetailsResponse = JsonConvert.DeserializeObject<StructAMKADetailsResponse>(rs);
                if (amkaDetailsResponse.Result.match)
                {
                    bool correctMetadata = true;
                    List<string> correctMetadataError = new List<string>();
                    if (!myMatch(surname, amkaDetailsResponse.Result.surname_cur_gr) && !myMatch(surname, amkaDetailsResponse.Result.surname_cur_en)) { correctMetadata = false; correctMetadataError.Add("surname");  } 
                   
                        if (!myMatch(firstname, amkaDetailsResponse.Result.name_gr) && !myMatch(firstname, amkaDetailsResponse.Result.name_en)) { correctMetadata = false; correctMetadataError.Add("firstname"); }
                   
                   
                        if (!myMatch(fathername, amkaDetailsResponse.Result.father_gr) && !myMatch(fathername, amkaDetailsResponse.Result.father_en)) { correctMetadata = false; correctMetadataError.Add("fathername"); }
                   
                        
                            if (!myMatch(mothername, amkaDetailsResponse.Result.mother_gr) && !myMatch(mothername, amkaDetailsResponse.Result.mother_en)) { correctMetadata = false; correctMetadataError.Add("mothername"); }
                        
                    
                        if (!myMatchDate(birthdate, amkaDetailsResponse.Result.birth_date) ) { correctMetadata = false; correctMetadataError.Add("birthdate"); }
                    
                    Helpers.GetENAREK getENAREK = new GetENAREK();
                    Helpers.StructENAREK structEnarek = getENAREK.byAMKAQueryUpdate(amka);
                    if (!structEnarek.success || structEnarek.ENAREK.Trim().Equals(""))
                    
                    {
                        response.flag = 0;
                        response.error = "ERROR ==> ENAREK.Helpers.GetEnarek returned error:" + structEnarek.error; 
                        response.ENAREK = "";    
                        response.AMKADetails = new StructAMKADetails();
                        Log.write("** getENAREK:" + JsonConvert.SerializeObject(response));
                        return response; 
                    }
                    if (correctMetadata)
                    {
                        response.flag = 1;  // SUCCESS
                    }
                    else
                    {
                        response.flag = 2;  // SUCCESS But we find mismatches 
                        response.error = String.Join("#", correctMetadataError.ToArray()); 
                    }
                    response.ENAREK = structEnarek.ENAREK;
                    StructAMKADetails amkaDetails = new StructAMKADetails();
                    amkaDetails.initFromOtherAMKADetails(amkaDetailsResponse.Result);
                    response.AMKADetails = amkaDetails;
                    Log.write("** getENAREK:" + JsonConvert.SerializeObject(response));
                    return response; 
                }
                else
                {

                    response.flag = -2; // HDIKA RETURNED no match
                    response.error = "No Match!";
                    StructAMKADetails amkaDetails = new StructAMKADetails();
                    amkaDetails.initFromOtherAMKADetails(amkaDetailsResponse.Result);
                    response.AMKADetails = amkaDetails;
                    Log.write("** getENAREK:" + JsonConvert.SerializeObject(response));
                    return response; 
                }
            }
            catch (Exception e)
            {
                response.flag = 0; // system error of ENAREK
                response.error = e.Message;
                response.ENAREK = "";
                response.AMKADetails = new StructAMKADetails();
                Log.write("** getENAREK:" + JsonConvert.SerializeObject(response));
                return response; 
            }
          
            /*
            {
                "code": 400,
  "success": false,
  "message": "Service Call Parameters Error, amka id must be a 11 digit number",
  "timestamp": "2019-05-30T15:14:00+03:00"
}
            {
                "ServiceCallID": "384623bd-38f0-4faf-866c-4b4bfa8b8040",
  "code": 200,
  "success": true,
  "Result": {
                    "match": "false",
    "ssn": "14087200714"
  },
  "timestamp": "2019-05-30T15:14:00+03:00"
       }
            {
                "ServiceCallID": "30a9f279-c7d6-425d-acce-c2ee88de569c",
  "code": 200,
  "success": true,
  "Result": {
                    "birth_municipality_greek_code": "ΑΤΤΙ",
    "birth_date": "14/08/1972",
    "father_en": "ANGELIS",
    "father_gr": "ΑΓΓΕΛΗΣ",
    "tid": "00073811366",
    "birth_country": "ΕΛΛΑΔΑ",
    "ssn": "14087200714",
    "birth_country_code": "GR",
    "last_mod_date": "05/11/2015",
    "surname_cur_gr": "ΚΙΤΡΙΝΑΚΗΣ",
    "id_type": "Τ",
    "surname_cur_en": "KITRINAKIS",
    "surname_birth_gr": "ΚΙΤΡΙΝΑΚΗΣ",
    "death_note": "",
    "citizenship": "ΕΛΛΑΔΑ",
    "sex": "ΑΡΡΕΝ",
    "surname_birth_en": "KITRINAKIS",
    "match": "true",
    "citizenship_code": "GR",
    "bdate_istrue": "",
    "birth_municipality": "ΑΘΗΝΑ",
    "death_date": "",
    "amka_cur": "14087200714",
    "mother_en": "EFTHALIA",
    "mother_gr": "ΕΥΘΑΛΙΑ",
    "id_num": "ΑΚ548769",
    "name_gr": "ΜΑΡΚΟΣ",
    "id_in": "",
    "id_creation_year": "2012",
    "name_en": "MARKOS"
  },
  "timestamp": "2019-05-30T15:12:00+03:00"
          }

    */
        }


        private static  bool myMatch(string src, string dst)
        {
            if (dst.Trim().Equals("")) return true;  
            return Core.canonicalize(src).Equals(Core.canonicalize(dst)); 
        }

        private static bool myMatchDate(string src, string dst)
        {
            try
            {
                if (dst.Trim().Equals("")) return true;
                string[] srcParts = src.Split('/');
                string[] dstParts = dst.Split('/'); 
                for (int i = 0; i<=2; i++)
                {
                    if (!Convert.ToInt32(srcParts[i]).Equals(Convert.ToInt32(dstParts[i]))) return false; 
                }
                return true; 
            }
            catch (Exception e) { return false;  }
        }

        public static string getAuthToken()
        {
            string err = "1";
            string urlAuth = "https://validation.interoperability.gr:9443/OAUTH2Proxy_1.0.0/services/oauthproxy/authorization/token";
            Console.WriteLine("x4"); 
            Log.write("starting getAuthToken");
            try
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, urlAuth))
                {
                    using (HttpContent httpContent = CreateHttpContent(valuesAuth))
                    {
                        request.Content = httpContent;
                        // using (var response = clientAuth.SendAsync(request, HttpCompletionOption.ResponseContentRead))
                        // using (var response =  clientAuth.SendAsync(request))
                        using ( HttpResponseMessage result = clientAuth.SendAsync(request, HttpCompletionOption.ResponseContentRead).Result)
                        {
                            err += "4";
                         //   HttpResponseMessage result = response;
                            err += "5";
                            if (result.IsSuccessStatusCode)
                            {

                                var resultContent = result.Content;
                                string responseString = resultContent.ReadAsStringAsync().Result;
                                StructOAUTHResponse json = JsonConvert.DeserializeObject<StructOAUTHResponse>(responseString);
                                Log.write("returning token:" + json.OAUTH.access_token);
                                return json.OAUTH.access_token;
                            }
                            else
                            {
                                Log.write("ERROR:" + result.StatusCode);
                                return ("ERROR:" + result.StatusCode);
                            }
                        }
                    }
                }
            }
            catch (System.Exception exc)
            {
                err += "9";
                return ("ERROR" + err + "  " + exc.Message);
            }

        }
        

        private static void SerializeJsonIntoStream(object value, Stream stream)
        {
            using (var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true))
            using (var jtw = new JsonTextWriter(sw) { Formatting = Formatting.None })
            {
                var js = new JsonSerializer();
                js.Serialize(jtw, value);
                jtw.Flush();
            }
        }

        private static ByteArrayContent CreateHttpContentV2(object data)
        {
            string myContent = JsonConvert.SerializeObject(data);
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            ByteArrayContent byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return byteContent; 
        }


        private static HttpContent CreateHttpContent(object content)
        {
            HttpContent httpContent = null;

            if (content != null)
            {
                var ms = new MemoryStream();
                SerializeJsonIntoStream(content, ms);
                ms.Seek(0, SeekOrigin.Begin);
                httpContent = new StreamContent(ms);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }
            return httpContent;
        }


        private static int validateAMKA(string amka, string surname)
        {
            
            if (surname.Trim().Equals("")) return -33;
            string amkaNorm = amka.Trim(); 
            if (!amkaNorm.Length.Equals(11))
            {
                return -31; 
            }
            if (!Regex.IsMatch(amkaNorm, @"^\d+$")) return -32;
            return 1; 
        }


        /*
        public static string testAuthV2()
        {
            string err = "1";
            try
            {
                
                var values = new Dictionary<string, string>
            {
   { "username", "edutech" },
   { "password", "edUt3ch!555@!#$" },
   { "grant_type", "password" }
};
                string url = "https://validation.interoperability.gr:9443/OAUTH2Proxy_1.0.0/services/oauthproxy/authorization/token";

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url))
                {
                    err += "2"; 
                    string json = JsonConvert.SerializeObject(values);
                    
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    request.Content = content;
                    err += "3";
                    var response = client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                    err += "4";
                    HttpResponseMessage result = response.Result;
                    err += "5";
                    if (result.IsSuccessStatusCode)
                    {
                        err += "6";
                        var resultContent = result.Content;
                        // by calling .Result you are synchronously reading the result
                        string responseString = resultContent.ReadAsStringAsync().Result;
                        err += "7";
                        return "SUCCESS:" + responseString;
                    }

                    //Auth auth = new Auth() ;
                    //auth.username = "edutech";
                    //auth.password = "edUt3ch!555@!#$";
                    //auth.grant_type = "password"; 
                    else
                    {
                        err += "8";
                        return ("ERROR:" + result.StatusCode);
                    }
                }


                //    //    client.DefaultRequestHeaders.Add("Content-Type", "application/json"); 

                //    //   var response = client.PostAsync(, content).Result;
                //    if (response.IsSuccessStatusCode)
                //{
                //    var responseContent = response.Content;
                //    // by calling .Result you are synchronously reading the result
                //    string responseString = responseContent.ReadAsStringAsync().Result;
                //    return "SUCCESS:" + responseString;
                //}

                //Auth auth = new Auth() ;
                //auth.username = "edutech";
                //auth.password = "edUt3ch!555@!#$";
                //auth.grant_type = "password"; 
                //else
                //{
                //    return ("ERROR:" + response.StatusCode);
                //}
            }
            catch (System.Exception  exc)
            {
                err += "9";
                return ("ERROR" + err + "  "  + exc.Message); 
            }
            
} */
        //WebResponse resp = req.GetResponse();
        //    Stream respStream = resp.GetResponseStream();
        //    return respStream.ToString(); 
        //     return JsonConvert.SerializeObject(structENAREK);
    }


}
