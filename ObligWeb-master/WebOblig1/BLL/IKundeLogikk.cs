using System;
using System.Collections.Generic;
using WebOblig1.Model;

namespace WebOblig1.BLL {
    public interface IKundeLogikk {
        string hentNavn(string pnr);

        List<DomeneKonto> VisMineKonti(string personnummer);

        List<DomeneBetaling> VisUtforteBetalinger(string personnummer);
        List<DomeneBetaling> VisFremtidigeBetalinger(string personnummer);

        DomenePerson KontoOversikt(string personnummer);

        string NyBetaling(DomeneBetaling betaling, String personnummer);

        bool SlettBetaling(int betalingsId);

        bool EndreBetaling(DomeneBetaling nyBetaling);

        bool Bruker_i_DB(DomenePerson person);

        

        /*********** Metoder for Administrator ***********/

        bool Admin_i_DB(DomenePerson person);
        string NyPerson(string adminpnr, DomenePerson person);
        bool EndreKunde(string adminpnr, DomenePerson person);
        string NyKonto(string adminpnr, DomeneKonto konto);
        bool EndreKonto(string adminpnr, DomeneKonto konto);
        DomenePerson hentKunden(string pnr);
        List<DomeneBetaling> hentTransaksjoner(string knr);
        bool SlettBetalingAdmin(int betalingsId, string adminPnr);
        DomeneBetaling HentBetaling(int betalingsId);
        string EndreBetalingAdmin(DomeneBetaling b, string adminPnr);
        string SlettKundeAdmin(string pnr, string adminPnr);
        bool SlettKontoAdmin(string knr, string adminPnr);
        DomeneKonto HentMinKonto(string knr, string pnr);
        string RegistrerEndring(string adminPnr, string type, string kundePnr);
        string RegistrerEndring(string adminPnr, string type, string kundePnr, string kontonr, string nyttKontonr);
        string RegistrerFeil(string feilMelding);
        List<DomeneBetaling> AdminVisAlleBetalinger();
    }
}
