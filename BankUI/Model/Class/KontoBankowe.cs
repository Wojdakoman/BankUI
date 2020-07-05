using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.Class
{
    using BankUI.ViewModel.Classes;
    using DAL.Repositories;
    using Projekt.DAL.Entity;

    class KontoBankowe
    {
        public Wlasciciel Wlasciciel { get; set; }
        public List<HistoriaLogowan> ListaLogowan { get; set; }
        public List<Konto> ListaKont { get; set; }
        public Dictionary<string,List<Przelew>> ListaPrzelewow { get; set; }
        public Dictionary<string,List<KartaPlatnicza>> KartyPlatnicze { get; set; }
        public Dictionary<string,List<KartaOperacje>> ListOperacjiKart { get; set; }
        public List<Konto> KontaKredytowe { get; set; }

        public KontoBankowe(Wlasciciel wlasciciel)
        {
            Wlasciciel = wlasciciel;
            ListaLogowan = RepositoryHistoriaLogowan.LoadHistory(Wlasciciel.Pesel);
            ListaKont = RepositoryKonto.GetAccount(Wlasciciel.Pesel);
            KontaKredytowe = RepositoryKonto.GetCreditAccount(Wlasciciel.Pesel);
            ListaPrzelewow = RepositoryPrzelew.LoadOperations(ListaKont);
            KartyPlatnicze = RepositoryKartaPlatnicza.LoadCards(ListaKont);
            ListOperacjiKart = RepositoryKartaOperacje.LoadHistory(KartyPlatnicze);
        }

        public void AddCard(string choosedAccount)
        {
            RepositoryKartaPlatnicza.AddCard(this.KartyPlatnicze, choosedAccount);
        }

        public void AddCreditAccount(Int64 pesel, string wybraneKonto, double wartosc, int ileM)
        {
            RepositoryKredyt.TakeCredit(pesel, wybraneKonto, wartosc, ileM);
        }

        public void AddAccount(string typ)
        {
            string accountNumber = Methods.AccountNumberGenerator();
            while (RepositoryKonto.NumberExist(accountNumber) != true)
            {
                accountNumber = Methods.AccountNumberGenerator();
            }
            Konto konto = new Konto(accountNumber, this.Wlasciciel.Pesel, typ);
            RepositoryKonto.AddAccount(konto);
        }
        /// <summary>
        /// Aktualizuje dane po ich zmianie w bazie
        /// </summary>
        public void Update()
        {
            ListaKont = RepositoryKonto.GetAccount(Wlasciciel.Pesel);
            ListaPrzelewow = RepositoryPrzelew.LoadOperations(ListaKont);
            KartyPlatnicze = RepositoryKartaPlatnicza.LoadCards(ListaKont);
            ListOperacjiKart = RepositoryKartaOperacje.LoadHistory(KartyPlatnicze);
        }

        //Wszystko zalezy od budowy w MVVM => wtedy sie zrobi
        public void ChangeOwnerData(Wlasciciel noweDane)
        {
            RepositoryWlasciciel.UpdateOwnerData(noweDane);
        }
    }
}
