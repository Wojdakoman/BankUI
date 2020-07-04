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
        public string Oprocentowanie { get; }
        public double Rata { get; }
        public string RataString { get => $"{Rata} PLN"; }
        public string NumerKonta { get; }
        public string DataSplaty { get; }

        public StringKredyt()
        {
            IDKredytu = 1;
            Wartosc = 1000.0;
            Oprocentowanie = "3,45%";
            Rata = 100;
            NumerKonta = "435735467264761";
            DataSplaty = "12.04.2020";
        }
    }
}
