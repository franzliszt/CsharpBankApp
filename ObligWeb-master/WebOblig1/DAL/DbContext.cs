namespace WebOblig1.DAL
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Data;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class KundeContext : DbContext {
        
        public KundeContext()
            : base("name=Kunde")  {
            //Database.CreateIfNotExists();
            Database.SetInitializer<KundeContext>(null);
        }

        public DbSet<Kunde> Kunder{ get; set;}

        public DbSet<Poststed> Poststeder { get; set; }

        public DbSet<Konto> Konti { get; set; }

        public DbSet<Betaling> Betalinger { get; set; }

        public DbSet<Autentisering> Autentiseringer { get; set; }

        public DbSet<Admin> Admin { get; set; }
        public DbSet<Endring> Endringer { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
        
    }

    public class Kunde {
        [Key]
        public string Personnummer { get; set; }

        public string Fornavn { get; set; }

        public string Etternavn { get; set; }

        public string Adresse { get; set; }

        public string Postnummer { get; set; }

        public string Telefonnummer { get; set; }
        public string Epost { get; set; }

       public virtual Poststed Poststed { get; set; }
       public virtual List<Konto> Konti { get; set; }

    }

    public class Poststed {
        [Key]
        public string postnummer { get; set; }
        public string poststed { get; set; }

    }

    public class Konto {
        [Key]
        public string Kontonummer { get; set; }
        public string Kontonavn { get; set; }
        public string Personnummer { get; set; }
        public double Saldo { get; set; }
        public virtual List<Betaling> Betalinger { get; set; }
    }

    public class Betaling
    {
        [Key]
        public int BetalingsId { get; set; }
        public string Kontonummer { get; set; }
        public string Dato { get; set; }
        public string TilKonto { get; set; }
        public double Belop { get; set; }
        public string Kid { get; set; }
        public string Melding { get; set; }
        public bool Gjennomfort { get; set; }
    }

    // Passord-tabell
    public class Autentisering {
        [Key]
        public string Personnummer { get; set; }
        public byte[] Passord { get; set; }
        public string Salt { get; set; }
    }

    // Admin-tabell
    public class Admin {
        [Key]
        public string Personnummer { get; set; }
        public byte[] Passord { get; set; }
        public string Salt { get; set; }
    }

    // Endrings-tabell
    public class Endring {
        [Key]
        public int EndringsId { get; set; }
        public string Dato { get; set; }
        public string AdminPnr { get; set; }
        public string Type { get; set; }
        public string KundePnr { get; set; }
        public string Kontonr { get; set; }
        public string nyttKontonr { get; set; }
    }
}