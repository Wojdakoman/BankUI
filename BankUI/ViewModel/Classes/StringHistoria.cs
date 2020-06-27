using Projekt.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUI.ViewModel.Classes
{
    class StringHistoria
    {
        public string Data { get; }
        public string Person { get; }
        public string Name { get; }
        public string Type { get; }
        public double Amount { get; }

        public StringHistoria(Przelew przelew, string konto)
        {
            Data = przelew.CzasOperacji.ToString();
            if (przelew.NumerNadawcy == konto)
                Person = przelew.NumerOdbiorcy;
            else Person = przelew.NumerNadawcy;
            Name = przelew.Tytul;
            Type = "Przelew";
            Amount = przelew.Wartosc;
        }
    }
}
