using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.DAL.Entity
{
    class KartaPlatnicza
    {
        public string NumerKarty { get; set; }
        public string NumerKonta { get; set; }
        public string DataWaznosci { get; set; }
        public double LimitPlatnosci { get; set; }
        public string Pin { get; set; }


        public KartaPlatnicza(MySqlDataReader reader)
        {
            NumerKarty = reader["NumerKarty"].ToString();
            NumerKonta = reader["NumerKonta"].ToString();
            DataWaznosci = reader["DataWaznosci"].ToString();
            LimitPlatnosci = double.Parse(reader["LimitPlatnosci"].ToString());
            Pin = reader["Pin"].ToString();
        }

        public KartaPlatnicza(string cardNumber, string accountNumber, string pin)
        {
            NumerKarty = cardNumber;
            NumerKonta = accountNumber;
            DateTime timeDay = DateTime.Now;
            timeDay = timeDay.AddYears(3);
            DataWaznosci = timeDay.Date.ToString("yyyy-MM-dd");
            LimitPlatnosci = 100;
            Pin = pin;
        }
    }
}
