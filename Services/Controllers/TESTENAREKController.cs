using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ENAREK.Helpers;

namespace Services.Controllers
{
    public class TESTENAREKController : ApiController
    {
        public struct Request
        {
            public string amka; 
        }
        public HDIKACalls.StructENAREKResponse Post(Request request)
        {

            //string firstname = "Markos";
            //string fathername = "Angelis";
            //string mothername = "Efthalia";
            //string birthdate = "14/08/1972";
            HDIKACalls.StructENAREKResponse response = HDIKACalls.test_getENAREK(request.amka); 
            return response;
        }

    }



}
