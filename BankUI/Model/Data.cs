using BankUI.ViewModel.Classes;
using MySql.Data.MySqlClient;
using Projekt.API;
using Projekt.Class;
using Projekt.DAL;
using Projekt.DAL.Entity;
using Projekt.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUI.Model
{
    /// <summary>
    /// Klasa odpowiedzialna za polaczenie modelu z view modelem. Do tej klasy kierowane sa wszystkei zapytania i polecenia
    /// </summary>
    class Data
    {
        private KontoBankowe kontoBankowe;
        private Wlasciciel wlasciciel;
        private List<string> _numeryKont = new List<string>();
        public int Konto { get; set; } = 0; //indeks konta z kontoBankowe.ListaKont, które jest aktualnie wybrane

        #region Ctor
        public Data()
        {
            Init();
        }

        async Task Init()
        {
            APIHelper.InitializeClient();
            await IP.GetData();
        }
        #endregion

        #region Funkcje
        public string PinNowejKarty()
        {
            return kontoBankowe.KartyPlatnicze[NumeryKont[Konto]].Last().Pin;
        }
        /// <summary>
        /// Sprawdza, czy mozna otworzyc polaczenie z baza danych
        /// </summary>
        public bool PolaczeniePoprawne()
        {
            try
            {
                DB.Instance.Connection.Open();
            } catch(MySqlException e)
            {
                return false;
            }
            return true;
        }
        public bool Login(string uzytkownik, string haslo)
        {
            wlasciciel = RepositoryWlasciciel.FindOwner(uzytkownik, haslo);
            if (wlasciciel is null) return false;
            else
            {
                kontoBankowe = new KontoBankowe(wlasciciel);
                return true;
            }
        }
        public bool LoginIstnieje(string login)
        {
            return RepositoryWlasciciel.DoesLoginExist(login);
        }
        public void AktualizujDaneOsobowe(WlascicielDane noweDane)
        {
            kontoBankowe.ChangeOwnerData(new Wlasciciel(noweDane, wlasciciel.Pesel));
            wlasciciel = RepositoryWlasciciel.FindOwner(noweDane.Login, noweDane.Haslo);
            kontoBankowe = new KontoBankowe(wlasciciel);
        }
        public void OtworzKonto()
        {
            kontoBankowe.AddAccount("Zwykle");
            kontoBankowe.Update();
        }
        public StringKarta PobierzKarte(string numerkarty)
        {
            foreach(var karta in kontoBankowe.KartyPlatnicze.ElementAt(Konto).Value)
            {
                if (karta.NumerKarty == numerkarty)
                    return new StringKarta(karta);
            }
            return null;
        }
        public List<StringHistoriaKarta> PobierzHistorieKarty(string numerkarty)
        {
            List<StringHistoriaKarta> result = new List<StringHistoriaKarta>();
            foreach(var operacja in kontoBankowe.ListOperacjiKart[numerkarty])
            {
                    result.Add(new StringHistoriaKarta(operacja));
            }
            result.Sort((x, y) => DateTime.Compare(y.Czas, x.Czas));
            return result;
        }
        /// <summary>
        /// Sprawdza, czy podany numer konta bankowego istnieje
        /// </summary>
        /// <param name="numerKonta">Sprawdzany numer konta</param>
        public bool NumerIstnieje(string numerKonta)
        {
            return !RepositoryKonto.NumberExist(numerKonta);
        }
        public void NowyPrzelew(string odbiorca, double wartosc, string tytul, string opis)
        {
            RepositoryPrzelew.ExecuteOperation(kontoBankowe.ListaKont[Konto], odbiorca, wartosc, tytul, opis);
            kontoBankowe.Update();
        }
        public void DodajKarte()
        {
            kontoBankowe.AddCard(kontoBankowe.ListaKont[Konto].NumerKonta);
            kontoBankowe.Update();
        }
        /// <summary>
        /// Usuwa kartę z konta
        /// </summary>
        /// <param name="karta">Numer usuwanej karty</param>
        /// <param name="konto">Numer konta, do ktorego karta jest przypisana</param>
        public void UsunKarte(string karta, string konto)
        {
            RepositoryKartaPlatnicza.DeleteCard(kontoBankowe.KartyPlatnicze, konto, karta);
            kontoBankowe.Update();
        }
        public void AktualizujKarte(string numerkarty, string pin, double limit)
        {
            RepositoryKartaPlatnicza.UpdateCard(numerkarty, pin, limit);
            kontoBankowe.Update();
        }
        /// <summary>
        /// Tworzenie nowego kredytu
        /// </summary>
        /// <param name="wartosc">Wartosc zaciaganego kredytu</param>
        /// <param name="dlugosc">Ilosc miesiecy splacania kredytu</param>
        public void WezKredyt(int wartosc, int dlugosc)
        {
            kontoBankowe.AddCreditAccount(wlasciciel.Pesel, kontoBankowe.ListaKont[Konto].NumerKonta, wartosc, dlugosc);
            kontoBankowe.Update();
        }
        /// <summary>
        /// Zwraca saldo konta kredytowego (na ktore przelewane sa raty)
        /// </summary>
        /// <param name="numerKonta">Numer konta kredytowego</param>
        public double Splacono(string numerKonta)
        {
            foreach(var konto in kontoBankowe.KontaKredytowe)
            {
                if (konto.NumerKonta == numerKonta)
                    return konto.Saldo;
            }
            return 0;
        }
        /// <summary>
        /// Kończy kredyt
        /// </summary>
        /// <param name="IDkredyt">Numer kredytu</param>
        /// <param name="numerKonta">Numer konta kredytowego</param>
        public void ZamknijKredyt(int IDkredyt, string numerKonta)
        {
            RepositoryKredyt.DeleteCredit(IDkredyt);
        }

        #endregion

        #region Public
        public string WlascicielName { get { if (!(wlasciciel is null)) return $"{wlasciciel.Imie} {wlasciciel.Nazwisko}"; return "unDef"; } }
        public List<string> NumeryKont { get {
                _numeryKont.Clear();
                foreach (var konto in kontoBankowe.ListaKont)
                {
                    _numeryKont.Add(konto.NumerKonta);
                }
                return _numeryKont;
            } }
        public string TypKonta { get => kontoBankowe.ListaKont[Konto].TypKonta; }
        public double Saldo { get => kontoBankowe.ListaKont[Konto].Saldo; }
        /// <summary>
        /// Zwraca liste informacji o kartach przypisanych do konta
        /// </summary>
        public List<StringKarta> Karty { get
            {
                List<StringKarta> result = new List<StringKarta>();
                foreach(var karta in kontoBankowe.KartyPlatnicze[NumeryKont[Konto]])
                {
                    result.Add(new StringKarta(karta));
                }
                return result;
            } }
        /// <summary>
        /// Zwraca liste wykonanych/odebranych przelewow oraz operacji kartowych na danym koncie
        /// </summary>
        public List<StringHistoria> Historia { get
            {
                List<Konto> temp = new List<Konto>();
                temp.Add(kontoBankowe.ListaKont[Konto]);
                List<StringHistoria> result = new List<StringHistoria>();
                //pobierz i dodaj przelewy do historii
                foreach(var przelew in RepositoryPrzelew.LoadOperations(temp).ElementAt(0).Value)
                {
                    result.Add(new StringHistoria(przelew, kontoBankowe.ListaKont[Konto].NumerKonta));
                }
                //dla wszystkich kart dla danego konta, dodaj operacje do historii
                foreach (var karta in kontoBankowe.KartyPlatnicze[kontoBankowe.ListaKont[Konto].NumerKonta])
                {
                    foreach (var operacja in kontoBankowe.ListOperacjiKart[karta.NumerKarty])
                    {
                        result.Add(new StringHistoria(operacja));
                    }
                }
                result.Sort((x, y) => DateTime.Compare(y.Czas, x.Czas));
                return result;
            } }
        public List<StringHistoriaLogowan> HistoriaLogowan
        {
            get
            {
                List<StringHistoriaLogowan> result = new List<StringHistoriaLogowan>();

                foreach (var log in RepositoryHistoriaLogowan.LoadHistory(wlasciciel.Pesel))
                {
                    result.Add(new StringHistoriaLogowan(log));
                }
                result.OrderBy(a => a.Data).ThenBy(a => a.Godzina);
                return result;
            }
        }
        public List<StringKredyt> Kredyty
        {
            get
            {
                List<StringKredyt> result = new List<StringKredyt>();

                foreach (var kredyt in RepositoryKredyt.LoadCredits(wlasciciel.Pesel))
                {
                    result.Add(new StringKredyt(kredyt, Splacono(kredyt.NumerKonta)));
                }
                return result;
            }
        }
        public WlascicielDane DaneWlasciciela { get => new WlascicielDane(wlasciciel); }
        #endregion
    }
}
