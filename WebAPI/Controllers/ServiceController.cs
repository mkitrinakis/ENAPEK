using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{

    [RoutePrefix("api/Service")]
    public class ServiceController : ApiController
    {
        // GET: WebAPI
      //  [Route("Simple")]
        public string GetSimple()
        {
            return "Hello World";
        }

        // GET api/HelloWorld/id
     //   [Route("WithParam")]
        public string GetSpecial(string id)
        {
            return "Hello " + id;
        }


        public string Post(string id)
        {
            return "this is my fist Post"; 
        }

        

        //  public string PostSpecial( NameValueCollection formData)
        public string PostSpecial(FormDataCollection formData)
        {
            List<KeyValuePair<string, string>> l = formData.ToList<KeyValuePair<string, string>>(); 
            string rs = ""; 
            foreach (KeyValuePair<string,string> kvp in l )
            {
                rs += kvp.Key + "-->" + kvp.Value + ",   "; 
            }
            // return "this is my fist Post with value:" + formData["val"];
            return "this is my fist Post with value:" + rs;
        }
    }
}
