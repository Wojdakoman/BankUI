using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.DAL.Entity
{
    class Przelew
    {
        public int? IDPrzelew { get; set; }
        public string NumerNadawcy { get; set; }
        public string NumerOdbiorcy { get; set; }
        public double Wartosc { get; set; }
        public DateTime CzasOperacji { get; set; }
        public string Tytul { get; set; }
        public string Opis { get; set; }

        public Przelew(MySqlDataReader reader)
        {
            IDPrzelew = int.Parse(reader["IDPrzelew"].ToString());
            NumerNadawcy = reader["NumerNadawcy"].ToString();
            NumerOdbiorcy = reader["NumerOdbiorcy"].ToString();
            Wartosc = double.Parse(reader["Wartosc"].ToString());
            CzasOperacji = DateTime.Parse(reader["CzasOperacji"].ToString());
            Tytul = reader["Tytul"].ToString();
            Opis = reader["Opis"].ToString();
        }

        public Przelew(string nadawca, string odbiorca, double wartosc, string tytul, string opis)
        {
            NumerNadawcy = nadawca;
            NumerOdbiorcy = odbiorca;
            Wartosc = wartosc;
            CzasOperacji = DateTime.Now;
            Tytul = tytul;
            Opis = opis;
        }
    }
}
