using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebOblig1.Model {
    
        public class DomenePerson {
            [Required(ErrorMessage = "Personnummer må oppgis.")]
            [RegularExpression(@"[0-9]{11}", ErrorMessage = "Personnummer må være 11 siffer.")]
            [Display(Name = "Personnummer")]
            public string personnummer { get; set; }

            [Required(ErrorMessage = "Passord må oppgis.")]
            [RegularExpression(@"[a-zæøåA-ZÆØÅ_@0-9]{6,30}", ErrorMessage = "Ugyldig passord. Minimum 6 tegn")]
            [Display(Name = "Passord")]
            [DataType(DataType.Password)]
            public string passord { get; set; }

            [Required(ErrorMessage = "Fornavn må oppgis.")]
            [StringLength(30, ErrorMessage = "Maks 30 tegn i fornavn.")]
            [RegularExpression(@"[a-zæøåA-ZÆØÅ]{2,30}", ErrorMessage = "Ugyldig tegn i fornavn.")]
            [Display(Name = "Fornavn")]
            public string fornavn { get; set; }

            [Required(ErrorMessage = "Etternavn må oppgis.")]
            [StringLength(50, ErrorMessage = "Maks 50 tegn i etternavn.")]
            [RegularExpression(@"[a-zæøåA-ZÆØÅ\-.' ]{2,30}", ErrorMessage = "Ugyldig tegn i etternavn.")]
            [Display(Name = "Etternavn")]
            public string etternavn { get; set; }

            [Required(ErrorMessage = "Adresse må oppgis.")]
            [StringLength(50, ErrorMessage = "Maks 50 tegn i adresse.")]
            [RegularExpression(@"[a-zæøåA-ZÆØÅ\-.' 0-9]{2,50}", ErrorMessage = "Ugyldig tegn i adresse.")]
            [Display(Name = "Adresse")]
            public string adresse { get; set; }

            [Required(ErrorMessage = "Postnummer må oppgis.")]
            [RegularExpression(@"[0-9]{4}", ErrorMessage = "Postnummer må være 4 siffer.")]
            [Display(Name = "Postnummer")]
            public string postnummer { get; set; }

            [Required(ErrorMessage = "Poststed må oppgis.")]
            [StringLength(50, ErrorMessage = "Maks 50 tegn i poststed.")]
            [RegularExpression(@"[a-zæøåA-ZÆØÅ \-]{2,50}", ErrorMessage = "Ugyldig poststed")]
            [Display(Name = "Poststed")]
            public string poststed { get; set; }

            [Required(ErrorMessage = "Telefonnumer må oppgis.")]
            [RegularExpression(@"[0-9]{8}", ErrorMessage = "Telefonnummer må være 8 siffer.")]
            [Display(Name = "Telefonnummer")]
            public string telefonnummer { get; set; }

            [Required(ErrorMessage = "Epost må oppgis.")]
            [StringLength(30, ErrorMessage = "Maks 30 tegn i EPOST.")]
            [RegularExpression(@"[a-zA-Z0-9_.@\-]{2,30}", ErrorMessage = "Ugyldig epost.")]
            [Display(Name = "Epost")]
            public string epost { get; set; }

            public List<DomeneKonto> konti;
            public DomeneKonto konto;

    }

    public class DomeneBetaling {
        // Ikke noe regex her pga den skal ikke brukes til å ta inn data.
        [Display(Name = "BetalingsID:")]
        public int betalingsId { get; set; }

        [Required(ErrorMessage = "Dato må oppgis.")]
        [DisplayFormat(DataFormatString ="{0:dd/MM/yyyy}", ApplyFormatInEditMode =true)]
        [Display(Name = "Dato:")]
        public string dato { get; set; }

        [Display(Name = "Fra Kontonummer:")]
        public string fraKonto { get; set; }

        [Required(ErrorMessage = "Til-konto må oppgis.")]
        [RegularExpression(@"[0-9]{11}", ErrorMessage = "Kontonummer må være 11 siffer.")]
        [Display(Name = "Til Kontonummer:")]
        public string tilKonto { get; set; }

        [Required(ErrorMessage = "Beløp må oppgis.")]
        [RegularExpression(@"[0-9]+", ErrorMessage = "Beløp må bestå av kun siffer")]
        [Display(Name = "Kroner:")]
        public double belopKroner { get; set; }

        [Required(ErrorMessage = "Beløp må oppgis.")]
        [RegularExpression(@"[0-9]{0,2}", ErrorMessage = "Øre må bestå av to siffer")]
        [Display(Name = "Øre:")]
        public double belopOrer { get; set; }

        [RegularExpression(@"[0-9]{2,25}", ErrorMessage = "KID må bestå av kun siffer")]
        [Required(ErrorMessage = "KID må oppgis.")]
        [StringLength(25, ErrorMessage = "Maks 25 siffer i KID.")]
        [Display(Name = "KID:")]
        public string kid { get; set; }

        [StringLength(30, ErrorMessage = "Maks 30 tegn i melding.")]
        [Required(ErrorMessage = "Melding må oppgis.")]
        [RegularExpression(@"[a-zæøåA-ZÆØÅ0-9 .()\-]{2,25}", ErrorMessage = "Ugyldig melding")]
        [Display(Name = "Melding:")]
        public string melding { get; set; }
    }

    public class DomeneKonto {
        [Display(Name = "Kontonummer")]
        public string kontonummer { get; set; }
        [Required(ErrorMessage = "Navn på konto må oppgis.")]
        [StringLength(30, ErrorMessage = "Maks 30 tegn i navn.")]
        [Display(Name = "Kontonavn")]
        public string kontonavn { get; set; }
        [Required]
        [Display(Name = "Saldo")]
        public double saldo { get; set; }
        [Display(Name = "Personnummer")]
        public string personnummer { get; set; }
        public List<DomeneBetaling> betalinger;

        public string GenererKontonummer() {
            Random rng = new Random();
            String kontonummer = "9780"; // Arbitrært valgt for å se kult ut.
            for (int i = 0; i < 7; i++) {
                kontonummer += rng.Next(0, 10).ToString();
            }
            return kontonummer;
        }
    }
}