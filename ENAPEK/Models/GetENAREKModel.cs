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

            [Display(Name ="ΑΜΚΑ")]
            
            
        [Required(ErrorMessage = "Παρακαλώ Συμπληρώστε ΑΜΚΑ")]
        [RegularExpression(@"^[0-9]{9,10}$", ErrorMessage ="Το ΑΜΚΑ πρέπει να είναι 9-10 αριθμητικά ψηφία")]
        public string AMKA { get; set;  }

        

        [Display(Name = "Το δικό μου ΕΝΑΡΕΚ ...")]
        public string ENAREK { get; set; }

        [Display(Name = "Επίθετο")]
        [Required(ErrorMessage = "Το πεδίο 'Επίθετο' είναι Υποχρεωτικό")]
        
        public string SurName { get; set; }

        [Display(Name = "Όνομα")]
        [Required(ErrorMessage = "Το πεδίο 'Όνομα' είναι Υποχρεωτικό")]
        public string FirstName { get; set; }

        [Display(Name = "Πατρώνυμο")]
        [Required(ErrorMessage = "Το πεδίο 'Πατρώνυμο' είναι Υποχρεωτικό")]
        public string FatherName { get; set; }

        [Display(Name = "Μητρώνυμο")]
        [Required(ErrorMessage = "Το πεδίο 'Μητρώνυμο' είναι Υποχρεωτικό")]
        
        public string MotherName { get; set; }



        // το type='date' σε Firefox κ Chrome γίνεται translate σωστά σε DataType.Data ανεξάρτητως format, και το placeholder δείχνει το φορματ του Browser
        // στον IE όμως το translation σε Date γίνεται βάση browser format και δεν ξέρουμε τι placeholder να βάλουμε. 
        [DataType(DataType.Date, ErrorMessage = "To πεδίο Ημ. Γέννησης' πρέπει να είναι ημερομηνία της μορφής μμ/ηη/εεεε" )]
       // [DisplayFormat(DataFormatString = "dd/MM/yyyy}")]
        [Required(ErrorMessage = "Το πεδίο 'Ημ. Γέννησης' είναι Υποχρεωτικό")]
        [Display(Name = "Ημ. Γέννησης")]
        
        // [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Σφάλμα")]
        public string Error { get; set; }
    }
}