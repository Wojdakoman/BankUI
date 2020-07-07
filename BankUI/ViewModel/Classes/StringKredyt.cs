using Projekt.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUI.ViewModel.Classes
{
    class StringKredyt
    {
        public int IDKredytu { get; }
        public double Wartosc { get; }
        public string WartoscString { get => $"{Wartosc} PLN"; }
        public double Oprocentowanie { get; }
        public string OprocentowanieString { get => $"{Oprocentowanie*100}%"; }
        public double Rata { get; }
        public string RataString { get => $"{Rata} PLN"; }
        public string NumerKonta { get; } //numer konta kredytowego na ktory przelewa sie raty
        public string DataSplaty { get; }
        public string SplaconoString { get; } //kwota na okncie kredytowym, ile już splacono
        public double Splacono { get; }
        public double Koszt { get; } //calkowita kwota kredytu do splacenia

        public StringKredyt(Kredyt kredyt, double splacono)
        {
            IDKredytu = Convert.ToInt32(kredyt.NumerKredytu);
            Wartosc = kredyt.Wartosc;
            Oprocentowanie = kredyt.Oprocentowanie;
            Rata = kredyt.Rata;
            NumerKonta = kredyt.NumerKonta;
            DataSplaty = kredyt.DataSplaty;
            Splacono = splacono;
            SplaconoString = $"{splacono} PLN";
            Koszt = Wartosc * (1 + Oprocentowanie);
        }
    }
}
