using BankUI.ViewModel.Classes;
using Projekt.API;
using Projekt.Class;
using Projekt.DAL.Entity;
using Projekt.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUI.Model
{
    class Data
    {
        private KontoBankowe kontoBankowe;
        private Wlasciciel wlasciciel;
        private List<string> _numeryKont = new List<string>();
        public int Konto { get; set; } = 0;

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
        public override string ToString()
        {
            return "jestem datą";
        }

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
        public List<StringHistoria> Historia { get
            {
                List<Konto> temp = new List<Konto>();
                temp.Add(kontoBankowe.ListaKont[Konto]);
                List<StringHistoria> result = new List<StringHistoria>();

                foreach(var przelew in RepositoryPrzelew.LoadOperations(temp).ElementAt(0).Value)
                {
                    result.Add(new StringHistoria(przelew, kontoBankowe.ListaKont[Konto].NumerKonta));
                }
                return result;
            } }
        #endregion
    }
}
