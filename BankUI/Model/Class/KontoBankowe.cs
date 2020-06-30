
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.Class
{
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

        public KontoBankowe(Wlasciciel wlasciciel)
        {
            Wlasciciel = wlasciciel;
            ListaLogowan = RepositoryHistoriaLogowan.LoadHistory(Wlasciciel.Pesel);
            ListaKont = RepositoryKonto.GetAccount(Wlasciciel.Pesel);
            ListaPrzelewow = RepositoryPrzelew.LoadOperations(ListaKont);
            KartyPlatnicze = RepositoryKartaPlatnicza.LoadCards(ListaKont);
            ListOperacjiKart = RepositoryKartaOperacje.LoadHistory(KartyPlatnicze);
        }

        public void AddCard(string choosedAccount)
        {
            RepositoryKartaPlatnicza.AddCard(this.KartyPlatnicze, choosedAccount);
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
    }
}
