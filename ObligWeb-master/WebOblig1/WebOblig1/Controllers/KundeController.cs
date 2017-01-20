using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebOblig1.Model;
using WebOblig1.BLL;

namespace WebOblig1.Controllers {
    public class KundeController : Controller {
        // Avgjøre dependency injection
        private IKundeLogikk _kundeBLL;

        public KundeController() {
            _kundeBLL = new BankDB_BLL();
        }

        public KundeController(IKundeLogikk stub) {
            _kundeBLL = stub;
        }

        public ActionResult Index()  {
            Session["LoggetInn"] = false;
            return View();
        }

        public ActionResult LoggInn() {
            return View();
        }


        public JsonResult LoggInnAjax(DomenePerson person) {
            // sjekk database for passord
            if (_kundeBLL.Bruker_i_DB(person)) {
                Session["pnr"] = person.personnummer;
                return Json( "OK", JsonRequestBehavior.AllowGet);
            } else {
                Session["LoggetInn"] = false;
                return Json("FEIL", JsonRequestBehavior.AllowGet);
             }
        }

        public JsonResult SjekkDummyID(string id) {
            if (Sikkerhet.validerdummyID(id)) {
                Session["LoggetInn"] = true;
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            return Json("FEIL", JsonRequestBehavior.AllowGet);
        }

    }
}
