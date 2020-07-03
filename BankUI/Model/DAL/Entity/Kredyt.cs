using MySql.Data.MySqlClient;
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
        public string DataSplaty { get; set; }
        public double Oprocentowanie { get; set; }
        public double Rata { get; set; }

        public Kredyt(MySqlDataReader reader)
        {
            NumerKredytu = int.Parse(reader["NumerKredytu"].ToString());
            WlascicielPesel = Int64.Parse(reader["WlascicielPesel"].ToString());
            Wartosc = double.Parse(reader["Wartosc"].ToString());
            NumerKonta = reader["NumerKonta"].ToString();
            DataSplaty = reader["DataSplaty"].ToString();
            Oprocentowanie = double.Parse(reader["Oprocentowanie"].ToString());
            Rata = double.Parse(reader["Rata"].ToString());
        }

        public Kredyt(Int64 pesel, string numerK, double wartosc, int czasM)
        {
            WlascicielPesel = pesel;
            Wartosc = wartosc;
            NumerKonta = numerK;
            DataSplaty = DateTime.Now.Date.AddMonths(czasM).ToString("yyyy-MM-dd");
            Oprocentowanie = 0.15;
            Rata = Math.Round((wartosc / czasM) + ((wartosc / czasM) * Oprocentowanie), 2);
        }
    }
}
