using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENAREK.Models;
using Newtonsoft.Json;

namespace ENAREK.Controllers
{
    public class HomeController : Controller
    {
        
        // Helpers.RandomGenerator rg; 
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your cvontact page.";

            return View();
        }

        public ActionResult GetENAREK()
        {
            ViewBag.Message = "GetENAREK";
            return View();
        }

        [HttpPost]
        public ActionResult GetENAREK(GetENAREKModel m)
        {
            //   if (rg == null) { rg = new Helpers.RandomGenerator();  }
            
            string AMKA = m.AMKA; 
            Helpers.GetENAREK getEMAREK = new Helpers.GetENAREK();

            ViewBag.Message = "GetENAREKFull Post"; 
            Helpers.Log.write("the AKA is" + AMKA); 
            string rs = getEMAREK.byAMKAOnlyQuery(m.AMKA).ENAREK;
            Helpers.RandomGenerator.counter++;
            string ID = Helpers.RandomGenerator.getID(); 
            if (!rs.StartsWith("ERROR"))
            {
                m.ENAREK = rs;
                
                m.Error = "random" + ID  + " ---counter:" + Helpers.RandomGenerator.counter ; 
            }
            else
            {
                m.ENAREK = "";
                
                m.Error = "random" + ID + " ---counter:" + Helpers.RandomGenerator.counter;
            }

            if (Helpers.RandomGenerator.checkID(ID))
            {
                m.Error += " SUCCESS"; 
            }
            else
            {
                m.Error += " --FAIL --";
            }
            return View(m);
        }

        public ActionResult GetENAREKFull()
        {
            ViewBag.Message = "GetENAREKFull";
            return View();
        }

        [HttpPost]
        public ActionResult GetENAREKFull(GetENAREKModel m)
        {
            string AMKA = m.AMKA;
            string SurName = m.SurName;
            string FirstName = m.FirstName;
            string FatherName = m.FatherName;
            string MotherName = m.MotherName;
            DateTime BirthDate = m.BirthDate; 
             Helpers.GetENAREK getENAREK = new Helpers.GetENAREK();
            
            Helpers.HDIKACalls.StructAMKADetailsResponse response1 = Helpers.HDIKACalls.getAMKADetails(true, AMKA, SurName);
            string myResponse = ""; 
            if (!response1.success)
            {
                myResponse = "Συστημικό Πρόβλημα καιά τον ελεγχο ΑΜΚΑ: " + response1.getError() + "(code:" + response1.code + ")"; 
            }
            else
            {
                if (!response1.Result.match)
                {
                    myResponse = "Δεν βρέθηκε εγγραφή με αυτό το ζεύγος ΑΜΚΑ κ Επώνυμο"; 
                }
                else
                {
                   
                }
            }

            Helpers.HDIKACalls.StructENAREKResponse response2 = Helpers.HDIKACalls.getENAREK(true, AMKA, SurName, FirstName, FatherName, MotherName, Helpers.Core.dateToString(BirthDate));
            //if (response2.flag.Equals(-1))
            //{
            //    myResponse = "Συστημικό Πρόβλημα καtά τον ελεγχο ΑΜΚΑ: " + response1.getError() + "(system error:" + response2.error+ ")";
            //}
            //if (response2.flag.Equals(-2))
            //{
            //    myResponse = "Δεν βρέθηκε εγγραφή με αυτό το ζεύγος ΑΜΚΑ κ Επώνυμο" + "(system error:" + response2.error + ")";
            //}
            //if (response2.flag.Equals(0))
            //{
            //    myResponse = "Συστημικό Πρόβλημα κατά την αναζήτηση ΕΝΑΡΕΚ" + "(system error:" + response2.error + ")";
            //}
            if (response2.flag <= 0)
            {
                myResponse = "f:" + response2.flag + " --> " + response2.error;
            }
            else
            {
                if (response2.flag.Equals(1))
                {
                    myResponse = "Το ΕΝΑΡΕΚ είναι : " + response2.ENAREK;
                }
                if (response2.flag.Equals(2))
                {
                    myResponse = "Το ΕΝΑΡΕΚ είναι : " + response2.ENAREK + " αλλά επιβεβαιώστε τα στοιχεία γιατί δεν είναι τα ίδια στα πεδία " + response2.error + ":";
                    myResponse += showDetails(response2.AMKADetails);
                }
            }

            // ViewBag.Message = "getAMKADetails Post for:" + Newtonsoft.Json.JsonConvert.SerializeObject(response1) + ", ENAREK Response:" + Newtonsoft.Json.JsonConvert.SerializeObject(response2);
            ViewBag.Message = myResponse; 

            Helpers.Log.write("the AKA is" + AMKA);
            

            return View(m);
        }

        private string showDetails(Helpers.HDIKACalls.StructAMKADetails amkaDetails)
        {
            string rs = "Επωνυμο: " + amkaDetails.surname_cur_gr + "(" + amkaDetails.surname_cur_en + ")" + Environment.NewLine;
            rs += "Όνομα: " + amkaDetails.name_gr + "(" + amkaDetails.name_en + ")" + Environment.NewLine;
            rs += "Πατρώνυμο: " + amkaDetails.father_gr+ "(" + amkaDetails.father_en+ ")" + Environment.NewLine;
            rs += "Μητρώνυμο: " + amkaDetails.mother_gr + "(" + amkaDetails.mother_en+ ")" + Environment.NewLine;
            rs += "Ημ. Γενν: " + amkaDetails.birth_date;
            return rs; 
        }
    }
}