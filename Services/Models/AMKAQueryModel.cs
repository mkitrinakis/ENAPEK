using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services.Models
{
    public class AMKAQueryModel
    {
        [Required]
        [Display(Name = "Επώνυμο")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Όνομα")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Πατρώνυμο")]
        public string FatherName { get; set; }

        [Required]
        [Display(Name = "Μητρώνυμο")]
        public string MontherName { get; set; }

        [Required]
        [Display(Name = "Ημ. Γέννησης")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "Ιθαγένεια")]
        public string Dimos { get; set; }

     

    }
}