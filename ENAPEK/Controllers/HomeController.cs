using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ENAREK.Models;
using Newtonsoft.Json;

namespace ENAREK.Controllers
{
    [RequireHttps]
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
            ViewBag.Message = "Your contact page.";

            return View();
        }


        // NOT USED 
        /*
        public ActionResult GetENAREK()
        {
            ViewBag.Message = "Συμπληρώστε τα πεδία και επιλέξτε <<Αναζήτηση ΕΝΑΡΕΚ>>";
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
            string rs = getEMAREK.byAMKAQueryUpdate(m.AMKA).ENAREK;
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
        */ 

     

        public ActionResult TaxisLogin()
        {
            bool isAuthenticated = (bool)(System.Web.HttpContext.Current.Session["isAuthenticated"] ?? false);
            
            if (isAuthenticated)
            {
                ViewBag.Message = "Hello " + (System.Web.HttpContext.Current.Session["LoginAccount"] ?? "").ToString() +  ", you Are Authenticated as you can Get your ENAREK";
                return RedirectToAction("GetENAREKFull");
            }
            else
            {
                ViewBag.Message = "Authentication Page";
                return View("TaxisLogin");
            }
        }

        public ActionResult TaxisLogOff()
        {
            System.Web.HttpContext.Current.Session["isAuthenticated"] = false;
            System.Web.HttpContext.Current.Session["LoginAccount"] = "";
            return View("Index");
        }

        [HttpPost]
        public ActionResult TaxisLogin(TaxisAccountModel m)
        {
            System.Web.HttpContext.Current.Session["isAuthenticated"] = true;
            System.Web.HttpContext.Current.Session["LoginAccount"] = m.LoginAccount;
            return RedirectToAction("GetENAREKFull");
        }


        public ActionResult GetENAREKFull()
        {
            bool isAuthenticated = (bool)(System.Web.HttpContext.Current.Session["isAuthenticated"] ?? false);
            GetENAREKModel m = new GetENAREKModel();
            m.ENAREK = ""; 
                
            if (isAuthenticated)
            {
                ViewBag.Message = "Συμπληρώστε τα πεδία και επιλέξτε <<Αναζήτηση ΕΝΑΡΕΚ>>";
                return View(m);
            }
            else
            {
                ViewBag.Message = "You have to Authenticate First";
                return RedirectToAction("TaxisLogin");
            }
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
            string myResponse = "";
             HtmlHelper.ClientValidationEnabled = true;
            if (ModelState.IsValid)
            {


                Helpers.HDIKACalls.StructAMKADetailsResponse response1 = Helpers.HDIKACalls.getAMKADetails(true, AMKA, SurName);

                if (!response1.success)
                {
                    myResponse = "<b>ΠΡΟΣΟΧΗ: </b> Συστημικό Πρόβλημα καιά τον ελεγχο ΑΜΚΑ: " + response1.getError() + "(code:" + response1.code + ")";
                }
                else
                {
                    if (!response1.Result.match)
                    {
                        myResponse = "<b>ΠΡΟΒΛΗΜΑ: </b> Δεν βρέθηκε εγγραφή με αυτό το ζεύγος ΑΜΚΑ κ Επώνυμο";
                    }
                    else
                    {

                    }
                }

                Helpers.HDIKACalls.StructENAREKResponse response2 = Helpers.HDIKACalls.getENAREK(true, AMKA, SurName, FirstName, FatherName, MotherName, Helpers.Core.dateToString(BirthDate));
                if (response2.flag <= 0)
                {

                    if (response2.flag.Equals(-1))
                    {
                        myResponse = "<b>ΠΡΟΒΛΗΜΑ: </b> Συστημικό Πρόβλημα καtά τον ελεγχο ΑΜΚΑ: " + response1.getError() + "(system error:" + response2.error + ")";
                    }
                    if (response2.flag.Equals(-2))
                    {
                        myResponse = "<b>ΠΡΟΒΛΗΜΑ: </b> Δεν βρέθηκε εγγραφή με αυτό το ζεύγος ΑΜΚΑ κ Επώνυμο" + "(system error:" + response2.error + ")";
                    }
                    if (response2.flag.Equals(0))
                    {
                        myResponse = "<b>ΠΡΟΒΛΗΜΑ: </b> Συστημικό Πρόβλημα κατά την αναζήτηση ΕΝΑΡΕΚ" + "(system error:" + response2.error + ")";
                    }
                }
                else
                {
                    myResponse = "<b>ΠΡΟΒΛΗΜΑ: </b> " + response2.flag + "-- > " + response2.error;
                }
    
                    if (response2.flag.Equals(1))
                    {
                        m.ENAREK = response2.ENAREK;
                        myResponse = response2.ENAREK;
                    }
                    if (response2.flag.Equals(2))
                    {
                        m.ENAREK = response2.ENAREK;
                        myResponse = "<b>ΠΡΟΣΟΧΗ: </b> Το ΕΝΑΡΕΚ που βράθηκε βάσει των στοιχείων  είναι : <b>" + response2.ENAREK + "</b> αλλά επιβεβαιώστε τα στοιχεία γιατί δεν είναι τα ίδια στo(α) πεδίο(α) " + response2.error + ".";
                        myResponse += "<br>" + "Το ΕΝΑΡΕΚ δόθηκε βάσει των παρακάτων τιμών: " + showDetails(response2.AMKADetails);
                    }            
                
                ViewBag.Message = myResponse;
                Helpers.Log.write("the AKA is" + AMKA);
            }
            return View(m);
        }



        public ActionResult GetENAREKTest()
        {
            
            GetENAREKModelTest m = new GetENAREKModelTest();
            m.ENAREK = "";
            ViewBag.Message = "Συμπληρώστε τα πεδία και επιλέξτε <<Αναζήτηση ΕΝΑΡΕΚ>>";
            return View(m);
        }

        [HttpPost]
        public ActionResult GetENAREKTest(ENAREK.Models.GetENAREKModelTest m)
        {
            ENAREK.Helpers.CallWebService call = new ENAREK.Helpers.CallWebService();
            string myResponse = call.pump(m);
        ViewBag.Message = myResponse;
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