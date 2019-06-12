using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENAREK.Models
{
    public class GetENAREKModel
    {
        //public GetENAREKModel()
        //{
        //    AMKA = "initAMKA";
        //    ENAREK = "initENAREK";
        //    Error = "initError";
        //}

        public string AMKA { get; set;  } 
        public string ENAREK { get; set; }

        public string SurName { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }

        [DataType(DataType.Date)]
       // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        public string Error { get; set; }
    }
}