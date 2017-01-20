using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebOblig1.DAL;
using WebOblig1.Model;

namespace WebOblig1.DAL {
    public class BankDB_Stub : DAL.IRepository {

        public List<DomeneBetaling> AdminVisAlleBetalinger() {
            var forventetResultat = new List<DomeneBetaling>();

            var betaling = new DomeneBetaling() {
                betalingsId = 1,
                belopKroner = 12,
                belopOrer = 50,
                melding = "Betaling 1",
                dato = "17/12/2016",
                fraKonto = "05205012345",
                tilKonto = "40809012345",
                kid = "12345678910"
            };

            forventetResultat.Add(betaling);
            forventetResultat.Add(betaling);
            forventetResultat.Add(betaling);

            return forventetResultat;
        }

        public bool Admin_i_DB(DomenePerson person) {
            // Admin finnes ikke i databasen eller passord feil
            if (!person.personnummer.Equals("01019012345")) {
                return false;
            } else {
                // Alt OK
                return true;
            }
        }

        public bool Bruker_i_DB(DomenePerson person) {
            // finner ikke bruker i DB
            if (!person.personnummer.Equals("01019012345")) {
                return false;
            }
            // feil passord
            if (!person.passord.Equals("hei123")) {
                return false;
            }
            // OK
            return true;
        }

        public bool EndreBetaling(DomeneBetaling betalingsEndring) {
            // Kunden blir bare sendt til VisFremtidigeBetalinger
            // Metoden burde egentlig vært void.
            return true;
        }

        public DomeneBetaling HentBetaling(int betalingsId) {
            // Hvis betaling ikke finnes i databasen
            if (!betalingsId.Equals(1)) {
                return null;
            }
                DomeneBetaling betaling = new DomeneBetaling {
                    fraKonto = "11112233333",
                    tilKonto = "11112244444",
                    dato = "2016-11-09",
                    kid = "12345",
                    melding = "Husleie",
                    betalingsId = betalingsId,
                    belopKroner = 1000,
                    belopOrer = 50
                };
                return betaling;
        }

        public DomeneKonto HentMinKonto(string knr, string pnr) {
            // Hvis konto ikke finnes i database
            if (!knr.Equals("11112233333")) {
                return null;
            }
            // Alt OK
            DomeneKonto konto = new DomeneKonto() {
                kontonavn = "Sparekonto",
                kontonummer = knr,
                personnummer = pnr,
                saldo = 1000
            };
            return konto;
        }

        public string NyKonto(string adminPnr, DomeneKonto konto) {
            if (konto.kontonummer.Equals("11111111111")) {
                return "KUNDEN EKSISTERER";
            } else if (konto.kontonummer.Equals("16111212121")) {
                return "DBFEIL";
            } else {
                return "OK";
            }
        }

        public bool EndreKonto(string adminPnr, DomeneKonto konto) {
            if (konto.kontonummer.Equals("11111211111")) {
                return false;
            }
            return true;
        }

        public string hentNavn(string pnr) {
            // Fant ikke kunden i databasen
            if (!pnr.Equals("01019012345")) {
                return null;
            }
            return "Per Hansen";
        }

        public DomenePerson KontoOversikt(string personnummer) {
            DomenePerson person = new DomenePerson {
                personnummer = personnummer,
                konti = new List<DomeneKonto>()
            };
            DomeneKonto konto1 = new DomeneKonto() {
                kontonavn = "Sparekonto",
                personnummer = personnummer,
                saldo = 5000,
                kontonummer = "11112233333"
            };
            DomeneKonto konto2 = new DomeneKonto() {
                kontonavn = "Brukskonto",
                personnummer = personnummer,
                saldo = 3000,
                kontonummer = "11112244444"
            };
            person.konti.Add(konto1);
            person.konti.Add(konto2);
            return person;
        }

        public string NyPerson(string adminpnr, DomenePerson person) {
            if (person.personnummer.Equals("11111111111")) {
                return "KUNDEN EKSISTERER";
            } else if (person.personnummer.Equals("16111212121")) {
                return "DBFEIL";
            } else {
                return "OK";
            }
        }

        public bool EndreKunde(DomenePerson person) {
            throw new NotImplementedException();
        }

        public string NyBetaling(DomeneBetaling betaling, string personnummer) {
            // Hvis kunden ikke finnes i databasen
            if (personnummer.Equals("02029012345")) {
                return "UKJENT";
            }
            // Hvis til-konto ikke finnes i databasen
            if (!betaling.tilKonto.Equals("11112244444")) {
                return "UKJENT";
            }
            // Hvis fra-konto ikke finnes i databasen
            if (!betaling.fraKonto.Equals("11112233333")) {
                return "UKJENT";
            }
            // Hvis ikke nok penger på konto
            if (!betaling.belopKroner.Equals(1000)) {
                return "SALDOFEIL";
            }
            // Hvis feil dato, feks "i fortid"
            if (!betaling.dato.Equals("2016-11-03")) {
                return "DATOFEIL";
            }
            // Hvis OK
            if (personnummer.Equals("01019012345")) {
                return "OK";
            } else {
                // Hvis DB feiler
                return "DBFEIL";
            }
        }

        // Usikker på implementering pga KundeContext som inputparameter.
        public bool oppdaterTransaksjoner(KundeContext db, List<Konto> konti) {
            throw new NotImplementedException();
        }

        public bool SlettBetaling(int betalingsId) {
            throw new NotImplementedException();
        }

        public List<DomeneBetaling> VisFremtidigeBetalinger(string personnummer) {
            // hvis kunde ikke finnes i database
            if (personnummer.Equals("02029012345")) {
                return null;
            }

            // Hvis oppdatering av transaksjoner feiler (her gitt ved bruk av et visst personnummer)
            if (personnummer.Equals("03039012345")) {
                return null;
            }

            // Alt OK
            List<DomeneBetaling> alleBetalinger = new List<DomeneBetaling>();
            DomeneBetaling testBetaling = new DomeneBetaling() {
                betalingsId = 1,
                dato = "2016-11-09",
                kid = "12345",
                fraKonto = "11112233333",
                tilKonto = "11112244444",
                melding = "Husleie",
                belopKroner = 1000
            };
            alleBetalinger.Add(testBetaling);
            alleBetalinger.Add(testBetaling);
            alleBetalinger.Add(testBetaling);
            
            return alleBetalinger;
        }

        public List<DomeneKonto> VisMineKonti(string personnummer) {
            // Hvis kunden ikke finnes i database
            if (!personnummer.Equals("01019012345")) {
                return null;
            }
            List<DomeneKonto> mineKonti = new List<DomeneKonto>();
            DomeneKonto konto = new DomeneKonto() {
                kontonavn = "Sparekonto",
                kontonummer = "11112233333",
                personnummer = personnummer,
                saldo = 1000
            };
            mineKonti.Add(konto);
            mineKonti.Add(konto);
            mineKonti.Add(konto);
            return mineKonti;
        }

        public List<DomeneBetaling> VisUtforteBetalinger(string personnummer) {
            // Hvis personen ikke finnes i databasen
            if (!personnummer.Equals("01019012345")) {
                return null;
            }

            List<DomeneBetaling> alleBetalinger = new List<DomeneBetaling>();
            for(int i = 0; i < 5; i++) {
                alleBetalinger.Add(new DomeneBetaling() {
                    betalingsId = i,
                    fraKonto = "11112233333",
                    tilKonto = "11112244444",
                    melding = "Husleie",
                    belopKroner = 1000 * i,
                    dato = "2016-11-01",
                    kid = "12345",
                });
            }
            return alleBetalinger;
        }

        public string RegistrerEndring(string adminPnr, string type, string kundePnr) {
            throw new NotImplementedException();
        }

        public string RegistrerEndring(string adminPnr, string type, string kundePnr, string kontonr, string nyttKontonr) {
            throw new NotImplementedException();
        }

        public string RegistrerFeil(string feilMelding) {
            throw new NotImplementedException();
        }

        public bool EndreKunde(string adminpnr, DomenePerson person) {
            if (person.personnummer.Equals("11111211111")) {
                return false;
            }
            // personen er endret
            return true;
        }

        public DomenePerson hentKunden(string pnr) {
            // Hvis kunden ikke finnes
            if (!pnr.Equals("01019012345")) {
                return null;
            } else {
                DomenePerson kunden = new DomenePerson() {
                    personnummer = "01019012345",
                    fornavn = "Ole",
                    etternavn = "Olsen",
                    adresse = "Osloveien 2",
                    epost = "ole@olsen.no",
                    postnummer = "0864",
                    poststed = "Oslo",
                    telefonnummer = "12344321"
                };
                return kunden;
            }
        }

        public bool finnesBruker(string pnr) {
            throw new NotImplementedException();
        }

        public List<DomeneBetaling> hentTransaksjoner(string knr) {
            if (knr.Equals("10000000000")) {
                List<DomeneBetaling> testBetaling = new List<DomeneBetaling>();
                DomeneBetaling b = new DomeneBetaling() {
                    dato = "2016-12-12",
                    betalingsId = 1,
                    kid = "1233",
                    belopKroner = 5,
                    melding = "betaler",
                    fraKonto = "10000000000",
                    tilKonto = "10000000001"
                };

                testBetaling.Add(b);
                testBetaling.Add(b);
                testBetaling.Add(b);

                return testBetaling;
            }
            return null;
        }

        public string EndreBetalingAdmin(DomeneBetaling nyBetaling, string adminPnr) {
            if (nyBetaling.fraKonto.Equals("10000000001")) {
                return "FEIL";
            }
            if (nyBetaling.dato.Equals("2016-02-02")) {
                return "DATOFEIL";
            }
            if (nyBetaling.belopKroner.Equals(1000)) {
                return "SALDOFEIL";
            }
            if (nyBetaling.fraKonto.Equals("10000000000")) {
                return "OK";
            } else {
                return "DBFEIL";
            }
        }

        public bool SlettBetalingAdmin(int betalingsId, string adminPnr) {
            if (betalingsId.Equals("1")) {
                return true;
            }
            return false;
        }

        public string SlettKundeAdmin(string pnr, string adminPnr) {
            if (pnr.Equals("05059012345")) {
                return "OK";
            } else {
                return "FEIL";
            }
        }

        public bool SlettKontoAdmin(string knr, string adminPnr) {
            if (knr.Equals("10000000000")) {
                return true;
            } else {
                return false;
            }
        }
    }
}
