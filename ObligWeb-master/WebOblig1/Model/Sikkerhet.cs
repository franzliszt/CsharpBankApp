using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace WebOblig1.Model {
    public class Sikkerhet {
        public static string DummyID = "12345";

        private Sikkerhet() { }

        // DISSE TO METODENE BLE BRUKT TIL Å GENERERE EN BRUKER. KAN HENDE DE SKAL BRUKES I DEL 2.
        public static byte[] lagHash(string passord) {
            byte[] innData, utData;
            var algoritme = System.Security.Cryptography.SHA512.Create();
            innData = System.Text.Encoding.ASCII.GetBytes(passord);
            utData = algoritme.ComputeHash(innData);
            return utData;
        }

        public static string lagSalt(int lengde) {
            Random rng = new Random();
            string salt = "";
            for (int i = 0; i < lengde; i++) {
                string ulikeTegn = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                int index = rng.Next(0, ulikeTegn.Length);
                salt += ulikeTegn[index];
            }
            return salt;
        }

        public static bool validerdummyID(string id) {
            return DummyID.Equals(id);
        }

        public static bool validerPersonnummer(string pnr) {
            Regex regex = new Regex(@"[0-9]{11}");
            Match match = regex.Match(pnr);
            return match.Success;
        }
    }
}