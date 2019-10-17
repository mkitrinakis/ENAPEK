using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENAREK.Models
{
    public class TaxisAccountModel
    {
        //public GetENAREKModel()
        //{
        //    AMKA = "initAMKA";
        //    ENAREK = "initENAREK";
        //    Error = "initError";
        //}

            [Display(Name ="TaxisNet Όνομα Χρήστη")]
        [Required(ErrorMessage = "Παρακαλώ Συμπληρώστε Τον Λογαριασμό σας στο TaxisNet (User Name)")]
        public string LoginAccount { get; set;  }

        [Display(Name = "TaxisNet Password")]
        [Required(ErrorMessage = "Παρακαλώ Συμπληρώστε Password")]
        public string LoginPassword { get; set; }

    }
}