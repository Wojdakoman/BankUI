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
        public double Rata { get; }
        public string RataString { get => $"{Rata} PLN"; }
        public string NumerKonta { get; }
        public string DataSplaty { get; }

        public StringKredyt(Kredyt kredyt)
        {
            IDKredytu = Convert.ToInt32(kredyt.NumerKredytu);
            Wartosc = kredyt.Wartosc;
            Oprocentowanie = kredyt.Oprocentowanie;
            Rata = kredyt.Rata;
            NumerKonta = kredyt.NumerKonta;
            DataSplaty = kredyt.DataSplaty;
        }
    }
}
