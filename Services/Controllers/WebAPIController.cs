using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Web.Http;

namespace Services.Controllers
{
    public class WebAPIController : ApiController
    {

        // GET: WebAPI
        //  [Route("Simple")]

        // url : http://localhost:21801/api/WebAPI/Simple


        public string GetSimple()
        {
            return "Hello World";
        }

        // GET api/HelloWorld/id
        //   [Route("WithParam")]
        //public string GetSpecial(string id)
        //{
        //    return "Hello " + id;
        //}


        public string GetSpecial(string id1, string id2)
        {
            return "Hello from " + id1 + "  to " + id2;
        }

        public string Post(string id)
        {
            return "this is my fist Post";
        }

       public  struct TempStruct
        {
            public string username;
            public string password;
            public string grant_type; 
        }

        public string Post(TempStruct src)
        {
            string rs = src.username + "." + src.password + "." + src.grant_type;
            return "result is:" + rs; 
        }
        
        //public string PostSpecial(FormDataCollection formData)
        //{
        // List<KeyValuePair<string, string>> l = formData.ToList<KeyValuePair<string, string>>();
        //    string rs = "";
        //    foreach (KeyValuePair<string, string> kvp in l)
        //    {
        //        rs += kvp.Key + "-->" + kvp.Value + ",   ";
        //    }
          
        //    return "this is my fist Post with value:" + rs;
        //}


    }
}
