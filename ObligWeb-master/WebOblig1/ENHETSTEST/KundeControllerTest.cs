using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebOblig1.DAL;
using WebOblig1.BLL;
using WebOblig1.Model;
using WebOblig1.Controllers;
using MvcContrib.TestHelper;
using System.Web.Mvc;

namespace ENHETSTEST {
    [TestClass]
    public class KundeControllerTest {

        [TestMethod]
        public void Index_ReturnsView() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new KundeController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;

            // Act
            var resultat = (ViewResult)controller.Index();

            // Assert
            Assert.IsFalse((bool)controller.Session["LoggetInn"]);
            Assert.AreEqual("", resultat.ViewName);
        }

        [TestMethod]
        public void LoggInn_ReturnsView() {
            // Arrange
            var controller = new KundeController(new BankDB_BLL(new BankDB_Stub()));

            // Act
            var resultat = (ViewResult)controller.LoggInn();

            // Assert
            Assert.AreEqual("", resultat.ViewName);
        }

        [TestMethod]
        public void SjekkDummyID_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new KundeController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;
            string id = "12345";

            // Act
            var resultat = controller.SjekkDummyID(id) as JsonResult;

            // Assert
            Assert.IsTrue((bool)controller.Session["LoggetInn"]);
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.AreEqual("OK", resultat.Data);
        }

        [TestMethod]
        public void SjekkDummyID_FeilID() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new KundeController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = false;
            string id = "54321";

            // Act
            var resultat = controller.SjekkDummyID(id) as JsonResult;

            // Assert
            Assert.IsFalse((bool)controller.Session["LoggetInn"]);
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.AreEqual("FEIL", resultat.Data);
        }

        [TestMethod]
        public void LoggInnAjax_OK() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new KundeController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["pnr"] = null;
            DomenePerson person = new DomenePerson() {
                fornavn = "Per",
                etternavn = "Hansen",
                personnummer = "01019012345",
                passord = "hei123"
            };

            // Act
            var resultat = controller.LoggInnAjax(person) as JsonResult;

            // Assert
            Assert.AreEqual(person.personnummer, controller.Session["pnr"]);
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.AreEqual("OK", resultat.Data);
        }

        [TestMethod]
        public void LoggInnAjax_BrukerFinnesIkke() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new KundeController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;
            DomenePerson person = new DomenePerson() {
                fornavn = "Per",
                etternavn = "Hansen",
                personnummer = "02029012345",
                passord = "hei123"
            };

            // Act
            var resultat = controller.LoggInnAjax(person) as JsonResult;

            // Assert
            Assert.IsFalse((bool)controller.Session["LoggetInn"]);
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.AreEqual("FEIL", resultat.Data);
        }

        [TestMethod]
        public void LoggInnAjax_FeilPassord() {
            // Arrange
            var SessionMock = new TestControllerBuilder();
            var controller = new KundeController(new BankDB_BLL(new BankDB_Stub()));
            SessionMock.InitializeController(controller);
            controller.Session["LoggetInn"] = null;
            DomenePerson person = new DomenePerson() {
                fornavn = "Per",
                etternavn = "Hansen",
                personnummer = "01019012345",
                passord = "passord"
            };

            // Act
            var resultat = controller.LoggInnAjax(person) as JsonResult;

            // Assert
            Assert.IsFalse((bool)controller.Session["LoggetInn"]);
            Assert.AreEqual(JsonRequestBehavior.AllowGet, resultat.JsonRequestBehavior);
            Assert.AreEqual("FEIL", resultat.Data);
        }
    }
}
