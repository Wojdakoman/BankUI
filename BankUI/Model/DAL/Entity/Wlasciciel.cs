using System;
using MySql.Data.MySqlClient;

namespace Projekt.DAL.Entity
{
    class Wlasciciel
    {
        public Int64 Pesel { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public DateTime DataUrodzenia { get; set; }
        public string Miasto { get; set; }
        public string Adres { get; set; }
        public uint Telefon { get; set; }
        public string Login { get; set; }
        public string Haslo { get; set; }

        public Wlasciciel(params string[] dane)
        {
            Pesel = Int64.Parse(dane[0]);
            Imie = dane[1];
            Nazwisko = dane[2];
            DataUrodzenia = DateTime.Parse(dane[3]);
            Miasto = dane[4];
            Adres = dane[5];
            Telefon = uint.Parse(dane[6]);
            Login = dane[7];
            Haslo = dane[8];
        }

        public Wlasciciel(MySqlDataReader reader)
        {
            Pesel = Int64.Parse(reader["Pesel"].ToString());
            Imie = reader["Imie"].ToString();
            Nazwisko = reader["Nazwisko"].ToString();
            DataUrodzenia = DateTime.Parse(reader["DataUrodzenia"].ToString());
            Miasto = reader["Miasto"].ToString();
            Adres = reader["Adres"].ToString();
            Telefon = uint.Parse(reader["Telefon"].ToString());
            Login = reader["Login"].ToString();
            Haslo = reader["Haslo"].ToString();
        }
    }
}
