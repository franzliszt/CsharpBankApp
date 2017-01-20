using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebOblig1.DAL;
using WebOblig1.BLL;
using WebOblig1.Model;
using WebOblig1.Controllers;
using MvcContrib.TestHelper;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Diagnostics;

namespace ENHETSTEST {
    [TestClass]
    public class MinSideControllerTest {

        [TestMethod]
        public void KontoOversikt_ReturnsView() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            controller.Session["pnr"] = "01019012345";

            var forventetResultat = new List<DomeneKonto>();
            DomeneKonto konto1 = new DomeneKonto() {
                kontonavn = "Sparekonto",
                personnummer = "01019012345",
                saldo = 5000,
                kontonummer = "11112233333"
            };
            DomeneKonto konto2 = new DomeneKonto() {
                kontonavn = "Brukskonto",
                personnummer = "01019012345",
                saldo = 3000,
                kontonummer = "11112244444"
            };
            forventetResultat.Add(konto1);
            forventetResultat.Add(konto2);

            // Act
            var resultat = (ViewResult)controller.KontoOversikt();
            var resultatPerson = (DomenePerson)resultat.Model;

            // Assert
            Assert.AreEqual("", resultat.ViewName);
            Assert.IsInstanceOfType(resultat.Model, typeof(DomenePerson));
            for(int i = 0; i < 2; i++) {
                Assert.AreEqual(forventetResultat[i].kontonavn, resultatPerson.konti[i].kontonavn);
                Assert.AreEqual(forventetResultat[i].personnummer, resultatPerson.konti[i].personnummer);
                Assert.AreEqual(forventetResultat[i].saldo, resultatPerson.konti[i].saldo);
                Assert.AreEqual(forventetResultat[i].kontonummer, resultatPerson.konti[i].kontonummer);
            }
        }

        [TestMethod]
        public void KontoOversikt_IkkeInnlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;
            controller.Session["pnr"] = "01019012345";

            // Act
            var resultat = controller.KontoOversikt() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void NyBetaling_ReturnsView() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            // Act
            var resultat = (ViewResult)controller.NyBetaling();

            // Assert
            Assert.AreEqual("", resultat.ViewName);
        }

        [TestMethod]
        public void NyBetaling_IkkeInnlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            // Act
            var resultat = controller.NyBetaling() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void VisBetalinger_ReturnsView() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            controller.Session["pnr"] = "01019012345";

            var forventetResultat = new List<DomeneBetaling>();
            for (int i = 0; i < 5; i++) {
                forventetResultat.Add(new DomeneBetaling() {
                    betalingsId = i,
                    fraKonto = "11112233333",
                    tilKonto = "11112244444",
                    melding = "Husleie",
                    belopKroner = 1000 * i,
                    dato = "2016-11-01",
                    kid = "12345",
                });
            }

            // Act
            var resultat = (ViewResult)controller.VisBetalinger();
            var resultatListe = (List<DomeneBetaling>)resultat.Model;

            // Assert
            Assert.AreEqual("", resultat.ViewName);
            Assert.IsInstanceOfType(resultat.Model, typeof(List<DomeneBetaling>));
            for (int i = 0; i < 5; i++) {
                Assert.AreEqual(forventetResultat[i].betalingsId, resultatListe[i].betalingsId);
                Assert.AreEqual(forventetResultat[i].fraKonto, resultatListe[i].fraKonto);
                Assert.AreEqual(forventetResultat[i].tilKonto, resultatListe[i].tilKonto);
                Assert.AreEqual(forventetResultat[i].melding, resultatListe[i].melding);
                Assert.AreEqual(forventetResultat[i].belopKroner, resultatListe[i].belopKroner);
                Assert.AreEqual(forventetResultat[i].dato, resultatListe[i].dato);
                Assert.AreEqual(forventetResultat[i].kid, resultatListe[i].kid);
            }
        }

        [TestMethod]
        public void VisBetalinger_FeilPersonnummer() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            controller.Session["pnr"] = "02029012345";

            // Act
            var resultat = (ViewResult)controller.VisBetalinger();
            var resultatListe = resultat.Model;

            // Assert
            Assert.AreEqual("", resultat.ViewName);
            Assert.IsNull(resultatListe);
        }

        [TestMethod]
        public void VisBetalinger_IkkeInnlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            // Act
            var resultat = controller.VisBetalinger() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void HentNavn_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["pnr"] = "01019012345";

            // Act
            var resultat = controller.hentNavn() as JsonResult;

            // Assert
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.AreEqual("Per Hansen", resultat.Data);
        }

        [TestMethod]
        public void HentNavn_FeilPersonnummer() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["pnr"] = "02029012345";

            // Act
            var resultat = controller.hentNavn() as JsonResult;

            // Assert
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.AreEqual(null, resultat.Data);
        }

        [TestMethod]
        public void VisFremtidigeBetalinger_ReturnsView() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            controller.Session["pnr"] = "01019012345";

            var forventetListe = new List<DomeneBetaling>();
            DomeneBetaling testBetaling = new DomeneBetaling() {
                betalingsId = 1,
                dato = "2016-11-09",
                kid = "12345",
                fraKonto = "11112233333",
                tilKonto = "11112244444",
                melding = "Husleie",
                belopKroner = 1000
            };
            forventetListe.Add(testBetaling);
            forventetListe.Add(testBetaling);
            forventetListe.Add(testBetaling);

            // Act
            var resultat = (ViewResult)controller.VisFremtidigeBetalinger();
            var resultatListe = (List<DomeneBetaling>)resultat.Model;

            // Assert
            Assert.AreEqual("", resultat.ViewName);
            for(int i = 0; i < 3; i++) {
                Assert.AreEqual(forventetListe[i].betalingsId, resultatListe[i].betalingsId);
                Assert.AreEqual(forventetListe[i].dato, resultatListe[i].dato);
                Assert.AreEqual(forventetListe[i].kid, resultatListe[i].kid);
                Assert.AreEqual(forventetListe[i].fraKonto, resultatListe[i].fraKonto);
                Assert.AreEqual(forventetListe[i].tilKonto, resultatListe[i].tilKonto);
                Assert.AreEqual(forventetListe[i].melding, resultatListe[i].melding);
                Assert.AreEqual(forventetListe[i].belopKroner, resultatListe[i].belopKroner);
            }
        }

        [TestMethod]
        public void VisFremtidigeBetalinger_FeilPersonnummer() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            controller.Session["pnr"] = "02029012345";

            // Act
            var resultat = (ViewResult)controller.VisFremtidigeBetalinger();
            var resultatListe = resultat.Model;

            // Assert
            Assert.AreEqual("", resultat.ViewName);
            Assert.IsNull(resultatListe);
        }

        [TestMethod]
        public void VisFremtidigeBetalinger_FeilTransaksjonsoppdatering() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            controller.Session["pnr"] = "03039012345";

            // Act
            var resultat = (ViewResult)controller.VisFremtidigeBetalinger();
            var resultatListe = resultat.Model;

            // Assert
            Assert.AreEqual("", resultat.ViewName);
            Assert.IsNull(resultatListe);
        }

        [TestMethod]
        public void VisFremtidigeBetalinger_IkkeLoggetInn() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            // Act
            var resultat = controller.VisFremtidigeBetalinger() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void EndreBetaling_ReturnsView() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            int betalingsID = 1;

            var forventetResultat = new DomeneBetaling() {
                fraKonto = "11112233333",
                tilKonto = "11112244444",
                dato = "2016-11-09",
                kid = "12345",
                melding = "Husleie",
                betalingsId = betalingsID,
                belopKroner = 1000,
                belopOrer = 50
            };

            // Act
            var resultat = (ViewResult)controller.EndreBetaling(betalingsID);
            var resultatBetaling = (DomeneBetaling)resultat.Model;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual(forventetResultat.betalingsId, resultatBetaling.betalingsId);
            Assert.AreEqual(forventetResultat.fraKonto, resultatBetaling.fraKonto);
            Assert.AreEqual(forventetResultat.tilKonto, resultatBetaling.tilKonto);
            Assert.AreEqual(forventetResultat.kid, resultatBetaling.kid);
            Assert.AreEqual(forventetResultat.melding, resultatBetaling.melding);
            Assert.AreEqual(forventetResultat.dato, resultatBetaling.dato);
            Assert.AreEqual(forventetResultat.belopKroner, resultatBetaling.belopKroner);
            Assert.AreEqual(forventetResultat.belopOrer, resultatBetaling.belopOrer);
        }

        [TestMethod]
        public void EndreBetaling_Feil() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            int betalingsID = 2;

            // Act
            var resultat = (ViewResult)controller.EndreBetaling(betalingsID);
            var resultatBetaling = resultat.Model;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.IsNull(resultatBetaling);
        }

        [TestMethod]
        public void EndreBetaling_IkkeInnlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;
            int betalingsID = 1;

            // Act
            var resultat = controller.EndreBetaling(betalingsID) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void HentKontoInfo_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["pnr"] = "01019012345";
            var kontonummer = "11112233333";

            // Act
            var resultat = controller.hentKontoInfo(kontonummer) as JsonResult;

            // Assert
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.IsInstanceOfType(resultat.Data, typeof(DomeneKonto));
            Assert.IsNotNull(resultat.Data);

        }

        [TestMethod]
        public void HentKontoInfo_FeilKontonummer() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["pnr"] = "01019012345";
            var kontonummer = "11112244444";

            // Act
            var resultat = controller.hentKontoInfo(kontonummer) as JsonResult;

            // Assert
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.IsNull(resultat.Data);
        }

        [TestMethod]
        public void HentAlleKonti_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["pnr"] = "01019012345";

            // Act
            var resultat = controller.hentAlleKonti() as JsonResult;

            // Assert
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.IsInstanceOfType(resultat.Data, typeof(List<DomeneKonto>));
            Assert.IsNotNull(resultat.Data);
        }

        [TestMethod]
        public void HentAlleKonti_FeilPersonnummer() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["pnr"] = "02029012345";

            // Act
            var resultat = controller.hentAlleKonti() as JsonResult;

            // Assert
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.IsNull(resultat.Data);
        }

        [TestMethod]
        public void NyAjaxBetaling_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["pnr"] = "01019012345";
            DomeneBetaling betaling = new DomeneBetaling() {
                tilKonto = "11112244444",
                fraKonto = "11112233333",
                belopKroner = 1000,
                dato = "2016-11-03",
                kid = "12345",
                melding = "Husleie",
                betalingsId = 1
            };

            // Act
            var resultat = controller.NyAjaxBetaling(betaling) as JsonResult;

            // Assert
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.IsNotNull(resultat.Data);
            Assert.AreEqual("OK", resultat.Data);
        }

        [TestMethod]
        public void NyAjaxBetaling_DBFeil() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["pnr"] = "03039012345";
            DomeneBetaling betaling = new DomeneBetaling() {
                tilKonto = "11112244444",
                fraKonto = "11112233333",
                belopKroner = 1000,
                dato = "2016-11-03",
                kid = "12345",
                melding = "Husleie",
                betalingsId = 1
            };

            // Act
            var resultat = controller.NyAjaxBetaling(betaling) as JsonResult;

            // Assert
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.IsNotNull(resultat.Data);
            Assert.AreEqual("FEIL", resultat.Data);
        }

        [TestMethod]
        public void NyAjaxBetaling_Datofeil() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["pnr"] = "01019012345";
            DomeneBetaling betaling = new DomeneBetaling() {
                tilKonto = "11112244444",
                fraKonto = "11112233333",
                belopKroner = 1000,
                dato = "2016-11-04",
                kid = "12345",
                melding = "Husleie",
                betalingsId = 1
            };

            // Act
            var resultat = controller.NyAjaxBetaling(betaling) as JsonResult;

            // Assert
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.IsNotNull(resultat.Data);
            Assert.AreEqual("DATOFEIL", resultat.Data);
        }

        [TestMethod]
        public void NyAjaxBetaling_FeilKunde() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["pnr"] = "02029012345";
            DomeneBetaling betaling = new DomeneBetaling() {
                tilKonto = "11112244444",
                fraKonto = "11112233333",
                belopKroner = 1000,
                dato = "2016-11-03",
                kid = "12345",
                melding = "Husleie",
                betalingsId = 1
            };

            // Act
            var resultat = controller.NyAjaxBetaling(betaling) as JsonResult;

            // Assert
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.IsNotNull(resultat.Data);
            Assert.AreEqual("FEIL", resultat.Data);
        }

        [TestMethod]
        public void NyAjaxBetaling_FeilTilKonto() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["pnr"] = "01019012345";
            DomeneBetaling betaling = new DomeneBetaling() {
                tilKonto = "11112244445",
                fraKonto = "11112233333",
                belopKroner = 1000,
                dato = "2016-11-03",
                kid = "12345",
                melding = "Husleie",
                betalingsId = 1
            };

            // Act
            var resultat = controller.NyAjaxBetaling(betaling) as JsonResult;

            // Assert
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.IsNotNull(resultat.Data);
            Assert.AreEqual("FEIL", resultat.Data);
        }

        [TestMethod]
        public void NyAjaxBetaling_FeilFraKonto() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["pnr"] = "01019012345";
            DomeneBetaling betaling = new DomeneBetaling() {
                tilKonto = "11112244444",
                fraKonto = "11112233334",
                belopKroner = 1000,
                dato = "2016-11-03",
                kid = "12345",
                melding = "Husleie",
                betalingsId = 1
            };

            // Act
            var resultat = controller.NyAjaxBetaling(betaling) as JsonResult;

            // Assert
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.IsNotNull(resultat.Data);
            Assert.AreEqual("FEIL", resultat.Data);
        }

        [TestMethod]
        public void NyAjaxBetaling_FeilSaldo() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["pnr"] = "01019012345";
            DomeneBetaling betaling = new DomeneBetaling() {
                tilKonto = "11112244444",
                fraKonto = "11112233333",
                belopKroner = 2000,
                dato = "2016-11-03",
                kid = "12345",
                melding = "Husleie",
                betalingsId = 1
            };

            // Act
            var resultat = controller.NyAjaxBetaling(betaling) as JsonResult;

            // Assert
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.IsNotNull(resultat.Data);
            Assert.AreEqual("FEIL", resultat.Data);
        }

        [TestMethod]
        public void EndreBetalingPOST_IkkeInnlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;
            DomeneBetaling betaling = new DomeneBetaling() {
                tilKonto = "11112244444",
                fraKonto = "11112233333",
                belopKroner = 2000,
                dato = "2016-11-03",
                kid = "12345",
                melding = "Husleie",
                betalingsId = 1
            };

            // Act
            var resultat = controller.EndreBetaling(betaling) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void EndreBetalingPOST_IkkeInnloggetSession() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;
            DomeneBetaling betaling = new DomeneBetaling() {
                tilKonto = "11112244444",
                fraKonto = "11112233333",
                belopKroner = 2000,
                dato = "2016-11-03",
                kid = "12345",
                melding = "Husleie",
                betalingsId = 1
            };

            // Act
            var resultat = controller.EndreBetaling(betaling) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void EndreBetalingPOST_ModelStateIkkeValid() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.ViewData.ModelState.AddModelError("kontonummer", "Ikke oppgitt kontonummer");
            controller.Session["LoggetInn"] = true;
            DomeneBetaling betaling = new DomeneBetaling() {
                tilKonto = "11112244444",
                fraKonto = "11112233333",
                belopKroner = 2000,
                dato = "2016-11-03",
                kid = "12345",
                melding = "Husleie",
                betalingsId = 1
            };

            // Act
            var resultat = controller.EndreBetaling(betaling) as ViewResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.IsTrue(resultat.ViewData.ModelState.Count == 1);
            Assert.AreEqual("", resultat.ViewName);
        }

        [TestMethod]
        public void EndreBetalingPOST_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new MinSideController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            DomeneBetaling betaling = new DomeneBetaling() {
                tilKonto = "11112244444",
                fraKonto = "11112233333",
                belopKroner = 2000,
                dato = "2016-11-03",
                kid = "12345",
                melding = "Husleie",
                betalingsId = 1
            };

            // Act
            var resultat = controller.EndreBetaling(betaling) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("VisFremtidigeBetalinger", resultat.RouteValues["Action"]);
        }
    }
}
