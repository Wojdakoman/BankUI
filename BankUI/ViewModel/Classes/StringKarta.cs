using Projekt.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUI.ViewModel.Classes
{
    class StringKarta
    {
        public string Numer { get; }
        public string Data { get; }
        public double Limit { get; }
        public string LimitString { get; }
        public string Konto { get; }
        public string Pin { get; }

        public StringKarta(KartaPlatnicza karta)
        {
            Numer = karta.NumerKarty;
            Data = karta.DataWaznosci;
            Limit = karta.LimitPlatnosci;
            LimitString = $"{karta.LimitPlatnosci} PLN";
            Konto = karta.NumerKonta;
            Pin = karta.Pin;
        }
    }
}
