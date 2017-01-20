using System.Diagnostics;
using System.Web.Mvc;
using WebOblig1.BLL;
using WebOblig1.Model;

namespace WebOblig1.Controllers {
    public class AdminController : Controller {
        // Avgjøre dependency injection
        private IKundeLogikk _kundeBLL;

        public AdminController() {
            _kundeBLL = new BankDB_BLL();
        }

        public AdminController(IKundeLogikk stub) {
            _kundeBLL = stub;
        }

        public JsonResult LoggInnAdmin(DomenePerson person) {
            if (_kundeBLL.Admin_i_DB(person)) {
                Session["pnr"] = person.personnummer;
                Session["LoggetInn"] = true;
                return Json("OK", JsonRequestBehavior.AllowGet);
            } else {
                Session["LoggetInn"] = false;
                return Json("FEIL", JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult KundeAdministrasjon() {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    return View();
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KundeAdministrasjon(DomenePerson person) {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    if (ModelState.IsValid) {
                        string ok = _kundeBLL.NyPerson((string)Session["pnr"], person);
                        if (ok == ("OK")) {
                            TempData["NY"] = "En ny kunde er registrert!";
                            return RedirectToAction("KundeAdministrasjon");
                        } else if (_kundeBLL.EndreKunde((string)Session["pnr"], person)) {
                            TempData["NY"] = "Endring utført!";
                            return RedirectToAction("KundeAdministrasjon");
                        }
                    }
                    ViewData["FEIL"] = "Feil i registrering";
                    return View();
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }

        public ActionResult KontoAdministrasjon() {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    return View();
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KontoAdministrasjon(DomeneKonto konto) {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    if (ModelState.IsValid) {
                        if (_kundeBLL.NyKonto((string)Session["pnr"], konto) == "OK") {
                            TempData["KONTO"] = "En ny konto er registrert!";
                            return RedirectToAction("KontoAdministrasjon");
                        } else if (_kundeBLL.EndreKonto((string)Session["pnr"], konto)) {
                            TempData["KONTO"] = "Endring utført!";
                            return RedirectToAction("KontoAdministrasjon");
                        }
                    }
                    ViewData["FEIL"] = "Feil i registrering";
                    return View();
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }

        public ActionResult HentEnPerson() {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    return View();
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }

        public ActionResult Totaloversikt(string pnr) {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    if (pnr != null && Sikkerhet.validerPersonnummer(pnr)) {
                        if (_kundeBLL.hentKunden(pnr) != null) {
                            Session["kunden"] = pnr;
                            return View(_kundeBLL.hentKunden(pnr));
                        }
                    }
                    TempData["KUNDEN"] = "Ugyldig personnummer!";
                    return RedirectToAction("HentEnPerson");
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }

        public ActionResult Transaksjoner(string knr) {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    return View(_kundeBLL.hentTransaksjoner(knr));
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }

        public ActionResult SlettBetalingAdmin(int bID, string kontoNr) {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    if (_kundeBLL.SlettBetalingAdmin(bID, (string)Session["pnr"])) {
                        return RedirectToAction("Transaksjoner", "Admin", new { knr = kontoNr });
                    }
                    ViewData["FEIL"] = "Feil i sletting av betaling!";
                    return RedirectToAction("Transaksjoner");
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }

        public ActionResult EndreBetalingAdmin(int bID) {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    return View(_kundeBLL.HentBetaling(bID));
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EndreBetalingAdmin(DomeneBetaling betaling) {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    if (ModelState.IsValid) {
                        if (_kundeBLL.EndreBetalingAdmin(betaling, (string)Session["pnr"]).Equals("OK")) {
                            return RedirectToAction("Transaksjoner", "Admin", new { knr = betaling.fraKonto });
                        }
                    }
                    return View();
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }

        public ActionResult SlettKundeAdmin(string pnr) {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    if (_kundeBLL.SlettKundeAdmin(pnr, (string)Session["pnr"]) == ("OK")) {
                        TempData["NY"] = "Kunde slettet!";
                        return RedirectToAction("KundeAdministrasjon");
                    }
                    ViewData["FEIL"] = "Feil i registrering";
                    return View();
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }

        public ActionResult SlettKontoAdmin(string knr) {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    if (_kundeBLL.SlettKontoAdmin(knr, (string)Session["pnr"])) {
                        TempData["KONTO"] = "Konto slettet!";
                        return RedirectToAction("KontoAdministrasjon");
                    }
                    ViewData["FEIL"] = "Feil i registrering";
                    return View();
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }
    }
}
