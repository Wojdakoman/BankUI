using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.DAL.Entity
{
    class KartaOperacje
    {
        public int? idOperacji { get; set; }
        public string KartaPlatniczaNumerKarty { get; set; }
        public string Typ { get; set; }
        public double Wartosc { get; set; }
        public DateTime CzasOperacji { get; set; }

        public KartaOperacje(MySqlDataReader reader)
        {
            idOperacji = int.Parse(reader["idOperacji"].ToString());
            KartaPlatniczaNumerKarty = reader["KartaPlatniczaNumerKarty"].ToString();
            Typ = reader["Typ"].ToString();
            Wartosc = double.Parse(reader["Wartosc"].ToString());
            CzasOperacji = DateTime.Parse(reader["CzasOperacji"].ToString());
        }

        public KartaOperacje(string numer, string typ, double ile)
        {
            KartaPlatniczaNumerKarty = numer;
            Typ = typ;
            Wartosc = ile;
            CzasOperacji = DateTime.Now;
        }
    }
}
