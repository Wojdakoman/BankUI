using Projekt.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUI.ViewModel.Classes
{
    /// <summary>
    /// Struktura danych wykorzystywana przy historii przelewow i operacji karta w panelu glownym
    /// </summary>
    class StringHistoria
    {
        public string Data { get => Czas.ToString(); }
        public DateTime Czas { get; }
        public string Person { get; }
        public string Name { get; }
        public string Type { get; }
        public string Amount { get; }

        public StringHistoria(Przelew przelew, string konto)
        {
            Czas = przelew.CzasOperacji;
            if (przelew.NumerNadawcy == konto)
                Person = przelew.NumerOdbiorcy;
            else Person = przelew.NumerNadawcy;
            Name = przelew.Tytul;
            Type = "Przelew";
            Amount = $"{przelew.Wartosc} PLN";
        }
        public StringHistoria(KartaOperacje operacja)
        {
            Czas = operacja.CzasOperacji;
            Person = operacja.KartaPlatniczaNumerKarty;
            Name = operacja.Typ;
            Type = "Operacja kartą";
            Amount = $"{operacja.Wartosc} PLN";
        }
    }
}
