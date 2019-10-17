using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ENAREK.Models
{
    public class GetENAREKModelTest
    {


        [Display(Name = "AMKA")]
        public string AMKA { get; set; }



        [Display(Name = "Το δικό μου ΕΝΑΡΕΚ ...")]
        public string ENAREK { get; set; }

        [Display(Name = "Επίθετο")]
        public string SurName { get; set; }

        [Display(Name = "Όνομα")]
        public string FirstName { get; set; }

        [Display(Name = "Πατρώνυμο")]
        public string FatherName { get; set; }

        [Display(Name = "Μητρώνυμο")]
        public string MotherName { get; set; }



      
        [Display(Name = "Ημ. Γέννησης")]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Σφάλμα")]
        public string Error { get; set; }
    }
}