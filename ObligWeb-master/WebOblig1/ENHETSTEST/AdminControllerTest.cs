using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebOblig1.DAL;
using WebOblig1.BLL;
using WebOblig1.Model;
using WebOblig1.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using MvcContrib.TestHelper;

namespace ENHETSTEST {

    [TestClass]
    public class AdminControllerTest {

        [TestMethod]
        public void KundeAdministrasjon_ReturnsView() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            // Act
            var resultat = (ViewResult)controller.KundeAdministrasjon();

            // Assert
            Assert.AreEqual("", resultat.ViewName);
        }

        [TestMethod]
        public void KundeAdministrasjon_IkkeInnlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            // Act
            var resultat = controller.KundeAdministrasjon() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void KundeAdministrasjon_HttpPost_DBFEIL() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            controller.Session["pnr"] = "99999999999";

            var innKunde = new DomenePerson() {
                personnummer = "11111111111",
                fornavn = "Donald",
                etternavn = "Duck",
                adresse = "Apalveien",
                postnummer = "9999",
                poststed = "Andeby",
                telefonnummer = "31274933",
                epost = "don@duck.no",
                passord = "passord"
            };

            // Act
            var resultat = controller.KundeAdministrasjon(innKunde) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("KundeAdministrasjon", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void KundeAdministrasjon_HttpPost_FEIL() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            controller.Session["pnr"] = "99999999999";

            var innKunde = new DomenePerson() {
                personnummer = "11111111111",
                fornavn = "Donald",
                etternavn = "Duck",
                adresse = "Apalveien",
                postnummer = "9999",
                poststed = "Andeby",
                telefonnummer = "31274933",
                epost = "don@duck.no",
                passord = "passord"
            };

            // Act
            var resultat = controller.KundeAdministrasjon(innKunde) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("KundeAdministrasjon", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void KundeAdministrasjon_HttpPost_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            controller.Session["pnr"] = "99999999999";

            var innKunde = new DomenePerson() {
                personnummer = "11111111111",
                fornavn = "Donald",
                etternavn = "Duck",
                adresse = "Apalveien",
                postnummer = "9999",
                poststed = "Andeby",
                telefonnummer = "31274933",
                epost = "don@duck.no",
                passord = "passord"
            };

            // Act
            var resultat = controller.KundeAdministrasjon(innKunde) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("KundeAdministrasjon", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void KundeAdministrasjon_ModelState_Feil() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.ViewData.ModelState.AddModelError("fornavn", "Ikke oppgitt fornavn");
            controller.Session["LoggetInn"] = true;
            
            controller.Session["pnr"] = "99999999999";

            var innKunde = new DomenePerson() {
                personnummer = "11111111111",
                fornavn = "Donald",
                etternavn = "Duck",
                adresse = "Apalveien",
                postnummer = "9999",
                poststed = "Andeby",
                telefonnummer = "31274933",
                epost = "don@duck.no",
                passord = "passord"
            };

            // Act
            var resultat = controller.KundeAdministrasjon(innKunde) as ViewResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.IsTrue(resultat.ViewData.ModelState.Count == 1);
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void KundeAdministrasjon_ModelState_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            controller.Session["pnr"] = "99999999999";

            var innKunde = new DomenePerson() {
                personnummer = "11111111111",
                fornavn = "Donald",
                etternavn = "Duck",
                adresse = "Apalveien",
                postnummer = "9999",
                poststed = "Andeby",
                telefonnummer = "31274933",
                epost = "don@duck.no",
                passord = "passord"
            };

            // Act
            var resultat = controller.KundeAdministrasjon(innKunde) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("KundeAdministrasjon", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void KundeAdministrasjon_HttpPost_Ikke_Innlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            controller.Session["pnr"] = "99999999999";

            var innKunde = new DomenePerson() {
                personnummer = "11111111111",
                fornavn = "Donald",
                etternavn = "Duck",
                adresse = "Apalveien",
                postnummer = "9999",
                poststed = "Andeby",
                telefonnummer = "31274933",
                epost = "don@duck.no",
                passord = "passord"
            };

            // Act
            var resultat = controller.KundeAdministrasjon(innKunde) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void KontoAdministrasjon_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            // Act
            var resultat = (ViewResult)controller.KontoAdministrasjon();

            // Assert
            Assert.AreEqual("", resultat.ViewName);
        }

        [TestMethod]
        public void KontoAdministrasjon_IkkeInnlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            // Act
            var resultat = controller.KontoAdministrasjon() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void KontoAdministrasjon_HttpPost_DBFEIL() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            controller.Session["pnr"] = "99999999999";

            var testKonto = new DomeneKonto() {
                kontonummer = "11111111111",
                kontonavn = "BSU",
                personnummer = "22222222222",
                saldo = 500
            };

            // Act
            var resultat = controller.KontoAdministrasjon(testKonto) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("KontoAdministrasjon", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void KontoAdministrasjon_HttpPost_FEIL() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            controller.Session["pnr"] = "99999999999";

            var testKonto = new DomeneKonto() {
                kontonummer = "11111111111",
                kontonavn = "BSU",
                personnummer = "22222222222",
                saldo = 500
            };

            // Act
            var resultat = controller.KontoAdministrasjon(testKonto) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("KontoAdministrasjon", resultat.RouteValues["Action"]);
            
        }

        [TestMethod]
        public void KontoAdministrasjon_HttpPost_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            controller.Session["pnr"] = "99999999999";

            var testKonto = new DomeneKonto() {
                kontonummer = "11111111111",
                kontonavn = "BSU",
                personnummer = "22222222222",
                saldo = 500
            };

            // Act
            var resultat = controller.KontoAdministrasjon(testKonto) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("KontoAdministrasjon", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void KontoAdministrasjon_ModelState_Feil() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.ViewData.ModelState.AddModelError("kontonavn", "Ikke oppgitt kontonavn");
            controller.Session["LoggetInn"] = true;

            controller.Session["pnr"] = "99999999999";

            var testKonto = new DomeneKonto() {
                kontonummer = "11111111111",
                kontonavn = "BSU",
                personnummer = "22222222222",
                saldo = 500
            };

            // Act
            var resultat = controller.KontoAdministrasjon(testKonto) as ViewResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.IsTrue(resultat.ViewData.ModelState.Count == 1);
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void KontoAdministrasjon_ModelState_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            controller.Session["pnr"] = "99999999999";

            var testKonto = new DomeneKonto() {
                kontonummer = "11111111111",
                kontonavn = "BSU",
                personnummer = "22222222222",
                saldo = 500
            };

            // Act
            var resultat = controller.KontoAdministrasjon(testKonto) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("KontoAdministrasjon", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void KontoAdministrasjon_HttpPost_Ikke_Innlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;

            controller.Session["pnr"] = "99999999999";
            var testKonto = new DomeneKonto() {
                kontonummer = "11111111111",
                kontonavn = "BSU",
                personnummer = "22222222222",
                saldo = 500
            };


            // Act
            var resultat = controller.KontoAdministrasjon(testKonto) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void LoggInnAdmin_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;
            controller.Session["pnr"] = null;
            DomenePerson person = new DomenePerson() {
                personnummer = "01019012345",
                
            };

            // Act
            var resultat = controller.LoggInnAdmin(person) as JsonResult;

            // Assert
            Assert.IsTrue((bool)controller.Session["LoggetInn"]);
            Assert.AreEqual(person.personnummer, controller.Session["pnr"]);
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.AreEqual("OK", resultat.Data);
        }

        [TestMethod]
        public void LoggInnAdmin_FEIL() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;
            DomenePerson person = new DomenePerson() {
                personnummer = "02029012345",

            };

            // Act
            var resultat = controller.LoggInnAdmin(person) as JsonResult;

            // Assert
            Assert.IsFalse((bool)controller.Session["LoggetInn"]);
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.AreEqual("FEIL", resultat.Data);
        }

        [TestMethod]
        public void HentEnPerson_ReturnsView() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            // Act
            var resultat = controller.HentEnPerson() as ViewResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("", resultat.ViewName);
        }

        [TestMethod]
        public void HentEnPerson_IkkeInnlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;

            // Act
            var resultat = controller.HentEnPerson() as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void TotalOversikt_IkkeInnlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;
            string pnr = "01019012345";

            // Act
            var resultat = controller.Totaloversikt(pnr) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void TotalOversikt_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            string pnr = "01019012345";
            DomenePerson forventetPerson = new DomenePerson() {
                personnummer = "01019012345",
                fornavn = "Ole",
                etternavn = "Olsen",
                adresse = "Osloveien 2",
                epost = "ole@olsen.no",
                postnummer = "0864",
                poststed = "Oslo",
                telefonnummer = "12344321"
            };

            // Act
            var resultat = controller.Totaloversikt(pnr) as ViewResult;
            var resultatPerson = (DomenePerson)resultat.Model;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("", resultat.ViewName);
            Assert.AreEqual(forventetPerson.fornavn, resultatPerson.fornavn);
            Assert.AreEqual(forventetPerson.etternavn, resultatPerson.etternavn);
            Assert.AreEqual(forventetPerson.adresse, resultatPerson.adresse);
            Assert.AreEqual(forventetPerson.personnummer, resultatPerson.personnummer);
            Assert.AreEqual(forventetPerson.postnummer, resultatPerson.postnummer);
            Assert.AreEqual(forventetPerson.poststed, resultatPerson.poststed);
            Assert.AreEqual(forventetPerson.epost, resultatPerson.epost);
            Assert.AreEqual(forventetPerson.telefonnummer, resultatPerson.telefonnummer);
        }

        [TestMethod]
        public void TotalOversikt_FeilPersonnummer() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            string pnr = "02029012345";

            // Act
            var resultat = controller.Totaloversikt(pnr) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("HentEnPerson", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void Transaksjoner_Ikke_Innlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;

            // Act
            var resultat = controller.Transaksjoner("10000000000") as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void Transaksjoner_ReturnsView() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;

            var forventetResultat = new List<DomeneBetaling>();
            var b = new DomeneBetaling() {
                dato = "2016-12-12",
                betalingsId = 1,
                kid = "1233",
                belopKroner = 5,
                melding = "betaler",
                fraKonto = "10000000000",
                tilKonto = "10000000001"
            };
            forventetResultat.Add(b);
            forventetResultat.Add(b);
            forventetResultat.Add(b);

            // Act
            var resultat = controller.Transaksjoner("10000000000") as ViewResult;
            var resultatetList = (List<DomeneBetaling>)resultat.Model;
            for (var i = 0; i < resultatetList.Count; i++) {
                Assert.AreEqual(forventetResultat[i].betalingsId, resultatetList[i].betalingsId);
                Assert.AreEqual(forventetResultat[i].kid, resultatetList[i].kid);
                Assert.AreEqual(forventetResultat[i].fraKonto, resultatetList[i].fraKonto);
                Assert.AreEqual(forventetResultat[i].tilKonto, resultatetList[i].tilKonto);
                Assert.AreEqual(forventetResultat[i].dato, resultatetList[i].dato);
                Assert.AreEqual(forventetResultat[i].melding, resultatetList[i].melding);
                Assert.AreEqual(forventetResultat[i].belopKroner, resultatetList[i].belopKroner);
            }

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("", resultat.ViewName);
        }

        [TestMethod]
        public void SlettBetalingAdmin_Ikke_Innlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;
            int bId = 1;
            string knr = "12312312312";

            // Act
            var resultat = controller.SlettBetalingAdmin(bId, knr) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void SlettBetalingAdmin_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            string knr = "10000000000";
            int bId = 1;

            // Act
            var resultat = controller.SlettBetalingAdmin(bId, knr) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Transaksjoner", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void SlettBetalingAdmin_FEIL() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            string knr = "10000000000";
            int bId = 1;

            // Act
            var resultat = controller.SlettBetalingAdmin(bId, knr) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Transaksjoner", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void EndreBetalingAdmin_IkkeInnlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;

            // Act
            var resultat = controller.EndreBetalingAdmin(1) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void EndreBetalingAdmin_ReturnsView() {
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
        public void EndreBetalingAdmin_HttpPost_IkkeInnlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;

            // Act
            var resultat = controller.EndreBetalingAdmin(1) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void EndreBetalingAdmin_HttpPost_DBFEIL() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            int betalingsID = 2;

            var forventetResultat = new DomeneBetaling() {
                fraKonto = "10000000005",
                tilKonto = "11112244444",
                dato = "2016-11-09",
                kid = "12345",
                melding = "Husleie",
                betalingsId = betalingsID,
                belopKroner = 10,
                belopOrer = 50
            };

            var resultat = (ViewResult)controller.EndreBetalingAdmin(betalingsID);

            // Assert
            Assert.AreEqual("", resultat.ViewName);
        }

        [TestMethod]
        public void EndreBetalingAdmin_HttpPost_FEIL() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            int betalingsID = 2;

            var forventetResultat = new DomeneBetaling() {
                fraKonto = "10000000005",
                tilKonto = "11112244444",
                dato = "2016-11-09",
                kid = "12345",
                melding = "Husleie",
                betalingsId = betalingsID,
                belopKroner = 10,
                belopOrer = 50
            };

            var resultat = (ViewResult)controller.EndreBetalingAdmin(betalingsID);

            // Assert
            Assert.AreEqual("", resultat.ViewName);
        }

        [TestMethod]
        public void EndreBetalingAdmin_HttpPost_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            int betalingsID = 1;

            var forventetResultat = new DomeneBetaling() {
                fraKonto = "10000000000",
                tilKonto = "11112244444",
                dato = "2016-11-09",
                kid = "12345",
                melding = "Husleie",
                betalingsId = betalingsID,
                belopKroner = 10,
                belopOrer = 50
            };

            // Act
            var resultat = controller.EndreBetalingAdmin(forventetResultat) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Transaksjoner", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void EndreBetalingAdmin_ModelState_Feil() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.ViewData.ModelState.AddModelError("kid", "Ikke oppgitt kid");
            controller.Session["LoggetInn"] = true;
            int betalingsID = 2;

            var forventetResultat = new DomeneBetaling() {
                fraKonto = "10000000005",
                tilKonto = "11112244444",
                dato = "2016-11-09",
                kid = "12345",
                melding = "Husleie",
                betalingsId = betalingsID,
                belopKroner = 10,
                belopOrer = 50
            };

            // Act
            var resultat = controller.EndreBetalingAdmin(forventetResultat) as ViewResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.IsTrue(resultat.ViewData.ModelState.Count == 1);
            Assert.AreEqual(resultat.ViewName, "");
        }

        [TestMethod]
        public void EndreBetalingAdmin_ModelState_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            int betalingsID = 1;

            var forventetResultat = new DomeneBetaling() {
                fraKonto = "10000000000",
                tilKonto = "11112244444",
                dato = "2016-11-09",
                kid = "12345",
                melding = "Husleie",
                betalingsId = betalingsID,
                belopKroner = 10,
                belopOrer = 50
            };

            // Act
            var resultat = controller.EndreBetalingAdmin(forventetResultat) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("Transaksjoner", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void SlettKundeAdmin_IkkeInnlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;

            // Act
            var resultat = controller.SlettKundeAdmin("04049012345") as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void SlettKundeAdmin_Feil() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            string pnr = "06069012345";

            // Act
            var resultat = (ViewResult)controller.SlettKundeAdmin(pnr);

            // Assert
            Assert.AreEqual("", resultat.ViewName);
        }

        [TestMethod]
        public void SlettKundeAdmin_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            string pnr = "05059012345";

            // Act
            var resultat = controller.SlettKundeAdmin(pnr) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("KundeAdministrasjon", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void SlettKontoAdmin_IkkeInnlogget() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;

            // Act
            var resultat = controller.SlettKontoAdmin("12345678912") as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(resultat);
            Assert.AreEqual("Kunde", resultat.RouteValues["Controller"]);
            Assert.AreEqual("LoggInn", resultat.RouteValues["Action"]);
        }

        [TestMethod]
        public void SlettKontoAdmin_Feil() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            string knr = "10000000009";

            // Act
            var resultat = (ViewResult)controller.SlettKontoAdmin(knr);

            // Assert
            Assert.AreEqual("", resultat.ViewName);
        }

        [TestMethod]
        public void SlettKontoAdmin_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new AdminController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = true;
            string knr = "10000000000";

            // Act
            var resultat = controller.SlettKontoAdmin(knr) as RedirectToRouteResult;

            // Assert
            Assert.AreEqual("KontoAdministrasjon", resultat.RouteValues["Action"]);
        }

    }
}
