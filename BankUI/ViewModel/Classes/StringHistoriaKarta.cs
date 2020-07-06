﻿using Projekt.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUI.ViewModel.Classes
{
    class StringHistoriaKarta
    {
        public string Data { get; }
        public DateTime Czas { get; }
        public string Typ { get; }
        public string Wartosc { get; }

        public StringHistoriaKarta(KartaOperacje operacja)
        {
            Data = operacja.CzasOperacji.ToString();
            Czas = operacja.CzasOperacji;
            Typ = operacja.Typ;
            Wartosc = $"{operacja.Wartosc} PLN";
        }
    }
}
