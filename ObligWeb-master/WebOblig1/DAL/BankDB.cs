using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using WebOblig1.Model;


namespace WebOblig1.DAL {
    public class BankDB : DAL.IRepository {
        public BankDB() {}

        public string hentNavn(string pnr) {
            using(var db = new KundeContext()) {
                var funnetKunde = db.Kunder.FirstOrDefault(k => k.Personnummer == pnr);
                if(funnetKunde == null) {
                    return null;
                }
                return funnetKunde.Fornavn + " " + funnetKunde.Etternavn;
            }
        }

        public List<DomeneKonto> VisMineKonti(string personnummer) {
            using (var db = new KundeContext()) {
                var funnetKunde = db.Kunder.FirstOrDefault(p => p.Personnummer == personnummer);
                if (funnetKunde == null) {
                    return null;
                }
                List<DomeneKonto> mineKonti = db.Konti.Where(p => p.Personnummer == personnummer).
                    Select(nk => new DomeneKonto() {
                        kontonavn = nk.Kontonavn,
                        kontonummer = nk.Kontonummer,
                        saldo = nk.Saldo
                    }).ToList();
                return mineKonti;
            }
        }

        // Viser betalinger for alle konti.
        public List<DomeneBetaling> VisUtforteBetalinger(string personnummer) {
            using (var db = new KundeContext()) {
                var hentKunde = db.Kunder.FirstOrDefault(k => k.Personnummer == personnummer);
                if (hentKunde == null) {
                    return null;
                }

                List<DomeneBetaling> alleBetalinger = new List<DomeneBetaling>();
                foreach(var konto in hentKunde.Konti) {
                    foreach (var betaling in db.Betalinger.Where(d => d.Gjennomfort.Equals(true) && 
                    (d.Kontonummer.Equals(konto.Kontonummer) || d.TilKonto.Equals(konto.Kontonummer)))) {
                        alleBetalinger.Add(new DomeneBetaling() {
                            betalingsId = betaling.BetalingsId,
                            fraKonto = betaling.Kontonummer,
                            tilKonto = betaling.TilKonto,
                            melding = betaling.Melding,
                            dato = betaling.Dato,
                            belopKroner = betaling.Belop,
                            kid = betaling.Kid
                        });
                    }
                }
                return alleBetalinger;
            }
        }

        // hjelpemetode. Stygg implementasjon, men fungerer
        private string GjeldendeDato() {
            return DateTime.Now.Year.ToString() + "-" + DateTime.Now.Month.ToString() + "-" +
                ((DateTime.Now.Day < 10) ? "0" + DateTime.Now.Day.ToString() : DateTime.Now.ToString());
        } 

        // Sjekk om det finnes betalinger som er klare til å bli gjennomført
        private bool oppdaterTransaksjoner(KundeContext db, List<Konto> konti) {
            foreach(var konto in konti) {
                foreach (var betaling in konto.Betalinger) {
                    if (betaling.Gjennomfort == false && String.Compare(betaling.Dato, GjeldendeDato()) < 0) {
                        konto.Saldo -= betaling.Belop;
                        var tilKonto = db.Konti.Find(betaling.TilKonto);
                        if (tilKonto == null) {
                            return false;
                        }
                        tilKonto.Saldo += betaling.Belop;
                        betaling.Gjennomfort = true;
                        
                    }
                }
            }
            try {
                db.SaveChanges();
                return true;
            } catch (Exception e) {
                RegistrerFeil(e.Message);
                return false;
            }
        }



        // Betalinger som ikke er utført
        public List<DomeneBetaling> VisFremtidigeBetalinger(string personnummer) {
            using (var db = new KundeContext()) {
                var hentKunde = db.Kunder.FirstOrDefault(k => k.Personnummer == personnummer);
                if (hentKunde == null) {
                    return null;
                }

                // Kjør oppdatering av transaksjoner
                // (transaksjoner som ikke er gjennomført blir gjennomført dersom dato ok)

                if (!oppdaterTransaksjoner(db, hentKunde.Konti)) {
                    return null;
                }

                List<DomeneBetaling> alleBetalinger = new List<DomeneBetaling>();
                foreach (var konto in hentKunde.Konti) {
                    foreach (var betaling in konto.Betalinger.Where(d => d.Gjennomfort.Equals(false))) {
                        alleBetalinger.Add(new DomeneBetaling() {
                            betalingsId = betaling.BetalingsId,
                            fraKonto = betaling.Kontonummer,
                            tilKonto = betaling.TilKonto,
                            melding = betaling.Melding,
                            dato = betaling.Dato,
                            belopKroner = betaling.Belop,
                            kid = betaling.Kid
                        });
                    }
                }
                return alleBetalinger;
            }
        }

        public DomenePerson KontoOversikt(string personnummer) {
            using (KundeContext db = new KundeContext()) {

                var funnetKunde = db.Kunder.Where(p => p.Personnummer == personnummer).FirstOrDefault();
                var nyDomenePerson = new DomenePerson() {
                    konti = new List<DomeneKonto>()
                };
                foreach (var konto in funnetKunde.Konti) {
                    var nyDomeneKonto = new DomeneKonto() {
                        kontonavn = konto.Kontonavn,
                        kontonummer = konto.Kontonummer,
                        saldo = konto.Saldo
                    };
                    nyDomenePerson.konti.Add(nyDomeneKonto);
                }
                return nyDomenePerson;
            }
        }
        
        public string NyBetaling(DomeneBetaling betaling, String personnummer) {
            using (KundeContext db = new KundeContext()) {
                var funnetKunde = db.Kunder.Find(personnummer);
                if (funnetKunde == null) {  // Kunde som overføres fra finnes ikke.
                    return "UKJENT";
                }
                var funnetTilKonto = db.Konti.Find(betaling.tilKonto);
                if (funnetTilKonto == null) { // Konto som overføres til finnes ikke.
                    return "UKJENT"; // Må endres til en egen feilmelding. Kun valgt for testing.
                }
                var funnetKonto = funnetKunde.Konti.Where(k => k.Kontonummer == betaling.fraKonto).FirstOrDefault();
                if (funnetKonto == null) {
                    return "UKJENT";
                }
                if ((betaling.belopKroner + (betaling.belopOrer / 100)) > funnetKonto.Saldo) {
                    return "SALDOFEIL";
                }

                // KOMMENTER UT MELLOM DISSE LINJENE FOR Å KUNNE TESTE OPPDATERING AV SALDO VED Å REGISTRERE BETALINGER "I FORTID"
                if (String.Compare(betaling.dato, GjeldendeDato()) < 0) {
                    return "DATOFEIL";
                }
                // KOMMENTER UT MELLOM DISSE LINJENE FOR Å KUNNE TESTE OPPDATERING AV SALDO VED Å REGISTRERE BETALINGER "I FORTID"

                var nyBetaling = new Betaling() {
                    Kontonummer = betaling.fraKonto,
                    Melding = betaling.melding,
                    Belop = betaling.belopKroner + (betaling.belopOrer / 100),
                    Kid = betaling.kid,
                    Dato = betaling.dato,
                    TilKonto = betaling.tilKonto,
                    Gjennomfort = false
                };
                try {
                    db.Betalinger.Add(nyBetaling);
                    db.SaveChanges();
                    return "OK";
                } catch (Exception e) {
                    RegistrerFeil(e.Message);
                    return "DBFEIL";
                }
            }
        }

        public bool SlettBetaling(int betalingsId) {
            using (var db = new KundeContext()) {
                var finnBetaling = db.Betalinger.Find(betalingsId);
                if (finnBetaling != null) {
                    try {
                        db.Betalinger.Remove(finnBetaling);
                        db.SaveChanges();
                        return true;
                    } catch (Exception e) {
                        RegistrerFeil(e.Message);
                        return false;
                    }
                }
                return false;
            }
        }

        public bool EndreBetaling( DomeneBetaling betalingsEndring) {
            using (var db = new KundeContext()) {
                if (SjekkBetaling(db, betalingsEndring.betalingsId)) {
                    var funnetKonto = db.Konti.Where(k => k.Kontonummer == betalingsEndring.fraKonto).FirstOrDefault();
                    if (funnetKonto == null) {
                        return false;
                    }
                    var eksisterendeBetaling = db.Betalinger.Find(betalingsEndring.betalingsId);
                    eksisterendeBetaling.Belop = 
                        betalingsEndring.belopKroner + (betalingsEndring.belopOrer / 100);
                    eksisterendeBetaling.Melding = betalingsEndring.melding;
                    eksisterendeBetaling.TilKonto = betalingsEndring.tilKonto;
                    eksisterendeBetaling.Kontonummer = betalingsEndring.fraKonto;
                    eksisterendeBetaling.Kid = betalingsEndring.kid;
                    eksisterendeBetaling.Dato = betalingsEndring.dato;
                    if (eksisterendeBetaling.Belop > funnetKonto.Saldo) {
                        return false;
                    }
                    if (String.Compare(eksisterendeBetaling.Dato, GjeldendeDato()) < 0) {
                        return false;
                    }
                    try {
                        db.SaveChanges();
                        return true;
                    } catch (Exception e) {
                        RegistrerFeil(e.Message);
                        return false;
                    }
                }
                return false;
            }
        }

        public bool Bruker_i_DB(DomenePerson person) {
            using (var db = new KundeContext()) {
                Autentisering funnetSalt = db.Autentiseringer.FirstOrDefault(k => k.Personnummer == person.personnummer);
                if (funnetSalt == null) {
                    return false;
                }
                string salt = funnetSalt.Salt;
                byte[] passordDb = Sikkerhet.lagHash(person.passord + salt);
                Autentisering funnetKunde = db.Autentiseringer.FirstOrDefault(k => k.Passord == passordDb && k.Personnummer == person.personnummer);
                if (funnetKunde == null) {
                    return false;
                } else {
                    return true;
                }
            }   
        }

        private bool SjekkBetaling(KundeContext db, int betalingsId) {
            var finnBetaling = db.Betalinger.Find(betalingsId);
            return finnBetaling != null;
        }

        public DomeneBetaling HentBetaling(int betalingsId) {
            using (var db = new KundeContext()) {
                var funnetBetaling = db.Betalinger.Find(betalingsId);
                if (funnetBetaling == null) {
                    return null;
                }
                DomeneBetaling betaling = new DomeneBetaling {
                    fraKonto = funnetBetaling.Kontonummer,
                    tilKonto = funnetBetaling.TilKonto,
                    dato = funnetBetaling.Dato,
                    kid = funnetBetaling.Kid,
                    melding = funnetBetaling.Melding,
                    betalingsId = funnetBetaling.BetalingsId,
                    belopKroner = Math.Truncate(funnetBetaling.Belop),
                    belopOrer = (double)(((decimal)funnetBetaling.Belop % 1) * 100)
                };
                return betaling;
            }
        }

        public DomeneKonto HentMinKonto(string knr, string pnr) {
            using (var db = new KundeContext()) {
                // bruk allerede laget metode for linjen under
                var hentKonto = db.Konti.Find(knr);
                if (hentKonto == null) {
                    return null;
                }
                bool egenKonto = hentKonto.Personnummer.Equals(pnr) ? true : false;
                return new DomeneKonto() {
                    kontonummer = hentKonto.Kontonummer,
                    saldo = egenKonto ? hentKonto.Saldo : 0,
                    kontonavn = egenKonto ? hentKonto.Kontonavn : "Ikke rettighet til å hente informasjon"
                };
            }
        }



        /******* ADMIN METODER *******/

        public string NyPerson(string adminpnr, DomenePerson person) {
            using (KundeContext db = new KundeContext()) {
                var funnetKunde = db.Kunder.Find(person.personnummer);
                if (funnetKunde == null) {
                    var nyPerson = new Kunde() {
                        Personnummer = person.personnummer,
                        Fornavn = person.fornavn,
                        Etternavn = person.etternavn,
                        Adresse = person.adresse,
                        Postnummer = person.postnummer,
                        Epost = person.epost,
                        Telefonnummer = person.telefonnummer
                    };
                    Autentisering nyttPassord = new Autentisering() {
                        Personnummer = person.personnummer
                    };
                    string Salt = Sikkerhet.lagSalt(30);
                    nyttPassord.Salt = Salt;
                    nyttPassord.Passord = Sikkerhet.lagHash(person.passord + Salt);
                    var eksistererPostnr = db.Poststeder.Find(person.postnummer);
                    if (eksistererPostnr == null) {
                        var nyttPoststed = new Poststed() {
                            postnummer = person.postnummer,
                            poststed = person.poststed
                        };
                        nyPerson.Poststed = nyttPoststed;
                    }
                    try {
                        db.Kunder.Add(nyPerson);
                        db.Autentiseringer.Add(nyttPassord);
                        db.SaveChanges();
                        RegistrerEndring(adminpnr, "Ny Kunde", person.personnummer);

                        return "OK";
                    } catch (Exception e) {
                        RegistrerFeil(e.Message);
                        return "DBFEIL";
                    }
                }
                return "KUNDEN EKSISTERER";
            }
        }

        public bool EndreKunde(string adminpnr, DomenePerson person) {
            using (var db = new KundeContext()) {
                var funnetKunde = db.Kunder.Where(k => k.Personnummer == person.personnummer);
                if (funnetKunde == null) {
                    return false;
                }
                var eksisterendeKunde = db.Kunder.Find(person.personnummer);
                eksisterendeKunde.Personnummer = person.personnummer;
                eksisterendeKunde.Fornavn = person.fornavn;
                eksisterendeKunde.Etternavn = person.etternavn;
                eksisterendeKunde.Adresse = person.adresse;
                eksisterendeKunde.Postnummer = person.postnummer;
                eksisterendeKunde.Epost = person.epost;
                eksisterendeKunde.Telefonnummer = person.telefonnummer;
                var eksistererPostnr = db.Poststeder.Find(person.postnummer);
                if (eksistererPostnr == null) {
                    var nyttPoststed = new Poststed() {
                        postnummer = person.postnummer,
                        poststed = person.poststed
                    };
                    eksisterendeKunde.Poststed = nyttPoststed;
                } else {
                    eksistererPostnr.postnummer = person.postnummer;
                    eksistererPostnr.poststed = person.poststed;
                }
                try {
                    db.SaveChanges();
                    RegistrerEndring(adminpnr, "Endre Kunde", person.personnummer);
                    return true;
                } catch (Exception e) {
                    RegistrerFeil(e.Message);
                    return false;
                }
            }
        }

        public string NyKonto(string adminPnr, DomeneKonto konto) {
            using (var db = new KundeContext()) {
                var funnetKonto = db.Konti.Find(konto.kontonummer);
                var funnetKunde = db.Kunder.Find(konto.personnummer);
                if (funnetKunde == null) {
                    return "KUNDE EKSISTERER IKKE";
                }
                if (funnetKonto != null) {
                    return "KONTO EKSISTERER";

                } else {
                    var nyKonto = new Konto() {
                        Personnummer = konto.personnummer,
                        Kontonummer = konto.kontonummer,
                        Kontonavn = konto.kontonavn,
                        Saldo = konto.saldo
                    };
                    try {
                        db.Konti.Add(nyKonto);
                        funnetKunde.Konti.Add(nyKonto);
                        db.SaveChanges();
                        RegistrerEndring(adminPnr, "Ny Konto", konto.personnummer, "", konto.kontonummer);
                        return "OK";
                    } catch (Exception e) {
                        RegistrerFeil(e.Message);
                        return "DBFEIL";
                    }
                }
            }
        }

        public bool EndreKonto(string adminPnr, DomeneKonto konto) {
            using (var db = new KundeContext()) {
                var funnetKonto = db.Konti.Where(k => k.Kontonummer == konto.kontonummer).FirstOrDefault();
                if (funnetKonto == null) {
                    return false;
                }
                var temp = funnetKonto.Kontonummer;
                var eksisterendeKonto = db.Konti.Find(konto.kontonummer);
                eksisterendeKonto.Personnummer = konto.personnummer;
                eksisterendeKonto.Kontonummer = konto.kontonummer;
                eksisterendeKonto.Kontonavn = konto.kontonavn;
                eksisterendeKonto.Saldo = konto.saldo;
                try {
                    db.SaveChanges();
                    RegistrerEndring(adminPnr, "Endre Konto", konto.personnummer, temp, konto.kontonummer);
                    return true;
                } catch (Exception e) {
                    RegistrerFeil(e.Message);
                    return false;
                }
            }
        }

        public bool Admin_i_DB(DomenePerson person) {
            using (var db = new KundeContext()) {
                Admin funnetSalt = db.Admin.FirstOrDefault(k => k.Personnummer == person.personnummer);

                if (funnetSalt == null) {
                    return false;
                }
                string salt = funnetSalt.Salt;
                byte[] passordDb = Sikkerhet.lagHash(person.passord + salt);
                Admin funnetKunde = db.Admin.FirstOrDefault(k => k.Passord == passordDb && k.Personnummer == person.personnummer);
                if (funnetKunde == null) {
                    return false;
                } else {
                    return true;
                }
            }
        }

        public List<DomeneBetaling> AdminVisAlleBetalinger() {
            using (var db = new KundeContext()) {
                var liste = db.Betalinger.ToList().OrderBy(d => d.Dato);

                List<DomeneBetaling> betalinger = new List<DomeneBetaling>();
                foreach (var b in liste) {
                    betalinger.Add(new DomeneBetaling() {
                        betalingsId = b.BetalingsId,
                        fraKonto = b.Kontonummer,
                        tilKonto = b.TilKonto,
                        belopKroner = b.Belop,
                        kid = b.Kid,
                        dato = b.Dato,
                        melding = b.Melding
                    });
                }
                return betalinger;
            }
        }

        public string RegistrerEndring(string adminPnr, string type, string kundePnr) {
            using( var db = new KundeContext()) {
                var nyEndring = new Endring() {
                    Dato = DateTime.Now.ToString(),
                    Type = type,
                    AdminPnr = adminPnr,
                    KundePnr = kundePnr,
                    Kontonr = "",
                    nyttKontonr = ""
                };
                try {
                    db.Endringer.Add(nyEndring);
                    db.SaveChanges();
                    return "OK";
                } catch(Exception e) {
                    RegistrerFeil(e.Message);
                    return "FEIL";
                }
            }
        }

        public string RegistrerEndring(string adminPnr, string type, string kundePnr, string kontonr, string nyttkontonr) {
            using (var db = new KundeContext()) {
                var nyEndring = new Endring() {
                    Dato = DateTime.Now.ToString(),
                    Type = type,
                    AdminPnr = adminPnr,
                    KundePnr = kundePnr,
                    Kontonr = kontonr,
                    nyttKontonr = nyttkontonr
                };
                try {
                    db.Endringer.Add(nyEndring);
                    db.SaveChanges();
                    return "OK";
                } catch (Exception e) {
                    RegistrerFeil(e.Message);
                    return "FEIL";
                }
            }
        }

        public string RegistrerFeil(string feilMelding) {
            // Funker både på localhost og remote server
            string path = HttpContext.Current.Server.MapPath("~/App_Data/Error.txt");
            try {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true)) {
                    file.WriteLine("Dato: " + DateTime.Now.ToString() + ". Feil: " + feilMelding + "\n");
                }
                return "OK";
            } catch (Exception e) {
                // Eventuell feilhåndtering her?
            }
            return "FEIL";
        }

        // Henter en kunde med alle konti og hver konto sine betalinger.
        public DomenePerson hentKunden(string pnr) {
            using (var db = new KundeContext()) {
                // sjekker om kunden finnes
                var finnes = db.Kunder.FirstOrDefault(k => k.Personnummer == pnr);
                if (finnes != null) {
                    DomenePerson kunden = new DomenePerson();
                    kunden.personnummer = finnes.Personnummer;
                    kunden.fornavn = finnes.Fornavn;
                    kunden.etternavn = finnes.Etternavn;
                    kunden.adresse = finnes.Adresse;
                    kunden.postnummer = finnes.Postnummer;
                    kunden.poststed = finnes.Poststed.poststed;
                    kunden.epost = finnes.Epost;
                    kunden.telefonnummer = finnes.Telefonnummer;
                    kunden.konti = mineKonti(finnes.Personnummer);

                    return kunden;
                }
                return null;
            }
        }

        // Henter alle konti til en kunde.
        private List<DomeneKonto> mineKonti(string pnr) {
            using (var db = new KundeContext()) {
                // henter alle kundens konti
                var kontoer = db.Konti.Where(k => k.Personnummer == pnr).ToList();
                
                List<DomeneKonto> konti = new List<DomeneKonto>();
                foreach (var konto in kontoer) {
                    konti.Add(new DomeneKonto() {
                        kontonummer = konto.Kontonummer,
                        kontonavn = konto.Kontonavn,
                        saldo = konto.Saldo,
                        betalinger = mineBetalinger(konto.Kontonummer)
                    });
                }
                return konti;
            }
        }

        // Henter alle betalinger til en konto.
        private List<DomeneBetaling> mineBetalinger(string knr) {
            using (var db = new KundeContext()) {
                // henter alle betalinger til en konti
                var betalinger = db.Betalinger.Where(k => k.Kontonummer == knr).ToList();

                List<DomeneBetaling> kontoBetalinger = new List<DomeneBetaling>();
                foreach (var b in betalinger) {
                    kontoBetalinger.Add(new DomeneBetaling() {
                        betalingsId = b.BetalingsId,
                        fraKonto = b.Kontonummer,
                        tilKonto = b.TilKonto,
                        dato = b.Dato,
                        belopKroner = b.Belop,
                        kid = b.Kid,
                        melding = b.Melding
                    });
                }

                return kontoBetalinger;
            } 
        }

        // skrevet 8/11/2016
        // sjekker om brukeren finnes
        public bool finnesBruker(string pnr) {
            using (var db = new KundeContext()) {
                return db.Kunder.Any(p => p.Personnummer == pnr);
            }
        }

        public List<DomeneBetaling> hentTransaksjoner(string knr) {
            using (var db = new KundeContext()) {
                var sjekk = db.Betalinger.Where(k => (k.Gjennomfort.Equals(false)) && (k.Kontonummer  == knr)).ToList();
                if (sjekk == null) {
                    return null;
                }

                List<DomeneBetaling> betalinger = new List<DomeneBetaling>();
                foreach (var b in sjekk) {
                    betalinger.Add(new DomeneBetaling() {
                        betalingsId = b.BetalingsId,
                        belopKroner = b.Belop,
                        fraKonto = b.Kontonummer,
                        tilKonto = b.TilKonto,
                        kid = b.Kid,
                        dato = b.Dato,
                        melding = b.Melding
                    });
                }
                return betalinger;
            }
        }
        
        public string EndreBetalingAdmin(DomeneBetaling nyBetaling, string adminPnr) {
            using (var db = new KundeContext()) {
                var sjekk = db.Konti.Where(k => k.Kontonummer == nyBetaling.fraKonto).FirstOrDefault();

                if (sjekk == null) {
                    return "FEIL";
                }

                var hentBetaling = db.Betalinger.Find(nyBetaling.betalingsId);
                hentBetaling.Dato = nyBetaling.dato;
                hentBetaling.Melding = nyBetaling.melding;
                hentBetaling.TilKonto = nyBetaling.tilKonto;
                hentBetaling.Kontonummer = nyBetaling.fraKonto;
                hentBetaling.Kid = nyBetaling.kid;
                hentBetaling.Belop = nyBetaling.belopKroner + (nyBetaling.belopOrer / 100);

                if (String.Compare(hentBetaling.Dato, GjeldendeDato()) < 0) {
                    return "DATOFEIL";
                }

                if (hentBetaling.Belop > sjekk.Saldo) {
                    return "SALDOFEIL";
                }

                try {
                    db.SaveChanges();
                    RegistrerEndring(adminPnr, "Endre Betaling", sjekk.Personnummer);
                    return "OK";
                } catch (Exception e) {
                    RegistrerFeil(e.Message);
                    return "FEIL_EXCEPTION";
                }
            }
        }
        
        public bool SlettBetalingAdmin(int betalingsId, string adminPnr) {
            using (var db = new KundeContext()) {
                var finnBetaling = db.Betalinger.Find(betalingsId);
                var sjekk = db.Konti.Where(k => k.Kontonummer == finnBetaling.Kontonummer).FirstOrDefault();
                if (finnBetaling != null) {
                    try {
                        db.Betalinger.Remove(finnBetaling);
                        db.SaveChanges();
                        RegistrerEndring(adminPnr, "Slett Betaling", sjekk.Personnummer);
                        return true;
                    } catch (Exception e) {
                        RegistrerFeil(e.Message);
                        return false;
                    }
                }
                return false;
            }
        }

        public string SlettKundeAdmin(string pnr, string adminPnr) {
            using (var db = new KundeContext()) {
                var finnKunde = db.Kunder.Find(pnr);
                var finnAutorisasjon = db.Autentiseringer.Find(pnr);
                if (finnKunde != null) {
                    try { 
                        if (slettKundensKonti(finnKunde.Personnummer, adminPnr)) {
                            db.Kunder.Remove(finnKunde);
                            db.Autentiseringer.Remove(finnAutorisasjon);
                            db.SaveChanges();
                            RegistrerEndring(adminPnr, "Slett Kunde", pnr);
                            return "OK";
                        } else {
                            db.Kunder.Remove(finnKunde);
                            db.Autentiseringer.Remove(finnAutorisasjon);
                            db.SaveChanges();
                            RegistrerEndring(adminPnr, "Slett Kunde", pnr);
                            return "OK";
                        }
                    } catch(Exception e) {
                        RegistrerFeil(e.Message);
                    }
                }
                return "FEIL";
            }
        }

        private bool slettKundensKonti(string pnr, string adminPnr) {
            using(var db = new KundeContext()) {
                var hentKonti = db.Konti.Where(p => p.Personnummer == pnr).ToList();

                if(hentKonti != null) {
                    foreach(var konto in hentKonti) {
                        try {
                            if (slettKundensBetalinger(konto.Kontonummer, adminPnr, konto.Personnummer)) {
                                RegistrerEndring(adminPnr, "Slett Konto", konto.Personnummer);
                                db.Konti.Remove(konto);
                                db.SaveChanges();
                                // hvis en konto ikke har betalinger - slett konto
                            } else {
                                RegistrerEndring(adminPnr, "Slett Konto", konto.Personnummer);
                                db.Konti.Remove(konto);
                                db.SaveChanges();
                            }
                        } catch(Exception e) {
                            RegistrerFeil(e.Message);
                        }
                    }
                    return true;
                }
                return false;
            }
        }

        private bool slettKundensBetalinger(string knr, string adminPnr, string kundensPnr) {
            using (var db = new KundeContext()) {
                var hentAlleBetalinger = db.Betalinger.Where(p => p.Kontonummer == knr).ToList();

                if (hentAlleBetalinger != null) {
                    foreach(var betaling in hentAlleBetalinger) {
                        try {
                            db.Betalinger.Remove(betaling);
                            db.SaveChanges();
                            RegistrerEndring(adminPnr, "Slett betaling", kundensPnr);
                        } catch(Exception e) {
                            RegistrerFeil(e.Message);
                        }
                    }
                    return true;
                }
                return false;
            }
        }

        public bool SlettKontoAdmin(string knr, string adminPnr) {
            using(var db = new KundeContext()) {
                var hentKonti = db.Konti.Find(knr);
                if(hentKonti != null) {
                    try {
                        slettKundensBetalinger(hentKonti.Kontonummer, adminPnr, hentKonti.Personnummer);
                        RegistrerEndring(adminPnr, "Slett konto", hentKonti.Personnummer);
                        db.Konti.Remove(hentKonti);
                        db.SaveChanges();
                        return true;
                    } catch(Exception e) {
                        RegistrerFeil(e.Message);
                    }
                }
                return false;
            }
        }
    }
}