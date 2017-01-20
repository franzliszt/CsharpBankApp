using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebOblig1.DAL;
using WebOblig1.Model;

namespace WebOblig1.BLL {
    public class BankDB_BLL : BLL.IKundeLogikk {
        private IRepository _repository;

        // *** Konstruktører. ***
        public BankDB_BLL() {
            _repository = new BankDB();
        }

        public BankDB_BLL(IRepository stub) {
            _repository = stub;
        }


        // *** Metoder. ***

        public string hentNavn(string pnr) {
            return _repository.hentNavn(pnr);
        }

        public List<DomeneKonto> VisMineKonti(string pnr) {
            return _repository.VisMineKonti(pnr);
        }

        public List<DomeneBetaling> VisUtforteBetalinger(string pnr) {
            return _repository.VisUtforteBetalinger(pnr);
        }

        public List<DomeneBetaling> VisFremtidigeBetalinger(string pnr) {
            return _repository.VisFremtidigeBetalinger(pnr);
        }

        public DomenePerson KontoOversikt(string pnr) {
            return _repository.KontoOversikt(pnr);
        }

        public string NyPerson(string adminpnr, DomenePerson person) {
            return _repository.NyPerson(adminpnr, person);
        }

        public string NyKonto(string adminpnr, DomeneKonto konto) {
            return _repository.NyKonto(adminpnr, konto);
        }

        public bool EndreKonto(string adminpnr, DomeneKonto konto) {
            return _repository.EndreKonto(adminpnr, konto);
        }

        public string NyBetaling(DomeneBetaling nyBetaliner, string pnr) {
            return _repository.NyBetaling(nyBetaliner, pnr);
        }

        public bool SlettBetaling(int betalingsID) {
            return _repository.SlettBetaling(betalingsID);
        }

        public bool EndreBetaling(DomeneBetaling endreBetaling) {
            return _repository.EndreBetaling(endreBetaling);
        }

        public bool Bruker_i_DB(DomenePerson person) {
            return _repository.Bruker_i_DB(person);
        }

        public bool Admin_i_DB(DomenePerson person) {
            return _repository.Admin_i_DB(person);
        }

        public DomeneBetaling HentBetaling(int betalingsID) {
            return _repository.HentBetaling(betalingsID);
        }

        public DomeneKonto HentMinKonto(string knr, string pnr) {
            return _repository.HentMinKonto(knr, pnr);
        }

        public List<DomeneBetaling> AdminVisAlleBetalinger() {
            return _repository.AdminVisAlleBetalinger();
        }

        public string RegistrerEndring(string adminPnr, string type, string kundePnr) {
            return _repository.RegistrerEndring(adminPnr, type, kundePnr);
        }

        public string RegistrerEndring(string adminPnr, string type, string kundePnr, string kontonr, string nyttKontonr) {
            return _repository.RegistrerEndring(adminPnr, type, kundePnr, kontonr, nyttKontonr);
        }

        public string RegistrerFeil(string feilMelding) {
            return _repository.RegistrerFeil(feilMelding);
        }

        public bool EndreKunde(string adminPnr, DomenePerson person) {
            return _repository.EndreKunde(adminPnr, person);
        }

        public DomenePerson hentKunden(string pnr) {
            return _repository.hentKunden(pnr);
        }

        public bool finnesBruker(string pnr) {
            return _repository.finnesBruker(pnr);
        }

        public List<DomeneBetaling> hentTransaksjoner(string knr) {
            return _repository.hentTransaksjoner(knr);
        }

        public string EndreBetalingAdmin(DomeneBetaling nyBetaling, string adminPnr) {
            return _repository.EndreBetalingAdmin(nyBetaling, adminPnr);
        }

        public bool SlettBetalingAdmin(int betalingsId, string adminPnr) {
            return _repository.SlettBetalingAdmin(betalingsId, adminPnr);
        }

        public string SlettKundeAdmin(string pnr, string adminPnr) {
            return _repository.SlettKundeAdmin(pnr, adminPnr);
        }

        public bool SlettKontoAdmin(string knr, string adminPnr) {
            return _repository.SlettKontoAdmin(knr, adminPnr);
        }
    }
}
