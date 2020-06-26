using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.DAL.Entity
{
    class Kredyt
    {
        //Przemyslec budowe
        public int? NumerKredytu { get; set; }
        public Int64 WlascicielPesel { get; set; }
        public double Wartosc { get; set; }
        public string NumerKonta { get; set; }
    }
}
