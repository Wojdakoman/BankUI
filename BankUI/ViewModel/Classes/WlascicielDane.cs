using Projekt.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUI.ViewModel.Classes
{
    class WlascicielDane
    {
        public string Imie { get; }
        public string Nazwisko { get; }
        public string Miasto { get; }
        public string Adres { get; }
        public string Telefon { get; }
        public string Login { get; }
        public string Haslo { get; }

        public WlascicielDane(Wlasciciel wlasciciel)
        {
            this.Imie = wlasciciel.Imie;
            this.Nazwisko = wlasciciel.Nazwisko;
            this.Miasto = wlasciciel.Miasto;
            this.Adres = wlasciciel.Adres;
            this.Telefon = wlasciciel.Telefon.ToString();
            this.Login = wlasciciel.Login;
            this.Haslo = wlasciciel.Haslo;
        }

        public WlascicielDane(string imie, string nazwisko, string miasto, string adres, string telefon, string login, string haslo)
        {
            this.Imie = imie;
            this.Nazwisko = nazwisko;
            this.Miasto = miasto;
            this.Adres = adres;
            this.Telefon = telefon;
            this.Login = login;
            this.Haslo = haslo;
        }
    }
}
