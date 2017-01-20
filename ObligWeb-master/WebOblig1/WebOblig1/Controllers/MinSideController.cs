using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebOblig1.Model;
using WebOblig1.BLL;
using System.Web.Script.Serialization;
using System.Web.Security;
using System;

namespace WebOblig1.Controllers
{
    public class MinSideController : Controller {

        // Avgjøre dependency injection
        private IKundeLogikk _kundeBLL;

        public MinSideController() {
            _kundeBLL = new BankDB_BLL();
        }

        public MinSideController(IKundeLogikk stub) {
            _kundeBLL = stub;
        }

        public ActionResult KontoOversikt() {
            if (Session["LoggetInn"] != null) {
               if ((bool)Session["LoggetInn"]) {
                    string pnr = (string)Session["pnr"];
                    var funnetKunde = _kundeBLL.KontoOversikt(pnr);
                    return View(funnetKunde);
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }
        
        public ActionResult NyBetaling() {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    return View();
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }
        
        public ActionResult VisBetalinger() {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    return View(_kundeBLL.VisUtforteBetalinger((string)Session["pnr"]));
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }

        public JsonResult hentNavn() {
            return Json(_kundeBLL.hentNavn((string)Session["pnr"]), JsonRequestBehavior.AllowGet);
        } 
        
        public ActionResult VisFremtidigeBetalinger() {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    return View(_kundeBLL.VisFremtidigeBetalinger((string)Session["pnr"]));
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }

        public void SlettBetaling(int bID) {
            _kundeBLL.SlettBetaling(bID);
        }

        public ActionResult EndreBetaling(int bID) {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    var betaling = _kundeBLL.HentBetaling(bID);
                    if (betaling != null) {
                        return View(betaling);
                    }
                    return View(); // Burde vært en tilbakemelding her.
                }
            }
            return RedirectToAction("LoggInn", "Kunde");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EndreBetaling(DomeneBetaling betaling) {
            if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {
                    if (ModelState.IsValid) {
                        _kundeBLL.EndreBetaling(betaling);
                        return RedirectToAction("VisFremtidigeBetalinger");
                    } else {
                        // ModelState ikke valid
                        return View(); // Feilmelding?
                    }

                } else {
                    return RedirectToAction("LoggInn", "Kunde");
                }
            } else {
                return RedirectToAction("LoggInn", "Kunde");
            }
        }

        public ActionResult LoggUt() {
            Session["LoggetInn"] = false;
            Session.Abandon();
            Session.RemoveAll();

            FormsAuthentication.SignOut();

            this.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            this.Response.Cache.SetNoStore();
            return RedirectToAction("Index", "Kunde");
        }

        public JsonResult NyAjaxBetaling(DomeneBetaling b) {
            if (_kundeBLL.NyBetaling(b, (string)Session["pnr"]) == "OK") {
                return Json("OK", JsonRequestBehavior.AllowGet);
            } else if (_kundeBLL.NyBetaling(b, (string)Session["pnr"]) == "DATOFEIL") { 
                return Json("DATOFEIL", JsonRequestBehavior.AllowGet);
            } else {
                return Json("FEIL", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult hentAlleKonti() {
            return Json(_kundeBLL.VisMineKonti((string)Session["pnr"]), JsonRequestBehavior.AllowGet);
        }

        public JsonResult hentKontoInfo(string knr) {
            return Json(_kundeBLL.HentMinKonto(knr, (string)Session["pnr"]), JsonRequestBehavior.AllowGet);
        }
    }

    /* MAL FOR Å SPERRE FOR INNLOGGING. BRUK PÅ NYE SIDER SOM LAGES
     * if (Session["LoggetInn"] != null) {
                if ((bool)Session["LoggetInn"]) {

                }
            }
            return RedirectToAction("LoggInn", "Kunde");
     * */
}
