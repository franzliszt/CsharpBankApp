using System;
using System.Collections.Generic;
using WebOblig1.Model;

namespace WebOblig1.DAL {
    public interface IRepository {
        string hentNavn(string pnr);
        List<DomeneKonto> VisMineKonti(string personnummer);
        List<DomeneBetaling> VisUtforteBetalinger(string personnummer);
        List<DomeneBetaling> VisFremtidigeBetalinger(string personnummer);
        DomenePerson KontoOversikt(string personnummer);
        string NyBetaling(DomeneBetaling betaling, String personnummer);
        bool SlettBetaling(int betalingsId);
        bool EndreBetaling(DomeneBetaling betalingsEndring);
        bool Bruker_i_DB(DomenePerson person);
        DomeneBetaling HentBetaling(int betalingsId);
        DomeneKonto HentMinKonto(string knr, string pnr);
        bool finnesBruker(string pnr);
        


        /*********** Metoder for Administrator ***********/
        string EndreBetalingAdmin(DomeneBetaling nyBetaling, string adminPnr);
        bool SlettBetalingAdmin(int betalingsId, string adminPnr);
        string SlettKundeAdmin(string pnr, string adminPnr);
        bool SlettKontoAdmin(string knr, string adminPnr);
        List<DomeneBetaling> hentTransaksjoner(string knr);
        bool Admin_i_DB(DomenePerson person);
        string RegistrerEndring(string adminPnr, string type, string kundePnr);
        string RegistrerEndring(string adminPnr, string type, string kundePnr, string kontonr, string nyttKontonr);
        string RegistrerFeil(string feilMelding);
        string NyPerson(string adminpnr, DomenePerson person);
        string NyKonto(string adminpnr, DomeneKonto konto);
        bool EndreKonto(string adminpnr, DomeneKonto konto);
        bool EndreKunde(string adminpnr, DomenePerson person);
        DomenePerson hentKunden(string pnr);
        List<DomeneBetaling> AdminVisAlleBetalinger();
    }
}
