using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
 using ENAREK.Helpers; 

namespace Services.Controllers
{
    public class ENAREKController : ApiController
    {
        public HDIKACalls.StructENAREKResponse Get()
        {
            string amka = "14087200714";
            string firstname = "Markos";
            string surname = "Kitrinakis";
            string fathername = "Angelis";
            string mothername = "Eythalia";
            string birthdate = "14/08/1972";
            HDIKACalls.StructENAREKResponse response = HDIKACalls.getENAREK(true, amka, surname, firstname, fathername, mothername, birthdate);
            return response;
        }

        public struct RequestStruct
        {
            public string amka;
            public string surname;
            public string firstname;
            public string fathername;
            public string mothername;
            public string birthdate; 
        }

        public HDIKACalls.StructENAREKResponse Post(RequestStruct request)
        {
          
            //string firstname = "Markos";
            //string fathername = "Angelis";
            //string mothername = "Efthalia";
            //string birthdate = "14/08/1972";
            HDIKACalls.StructENAREKResponse response = HDIKACalls.getENAREK(true, request.amka, request.surname, request.firstname, request.fathername, request.mothername, request.birthdate);
            return response;
        }



            //GET api/HelloWorld/id
            //  [Route("WithParam")]

        }
}
