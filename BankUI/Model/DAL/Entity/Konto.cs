using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Mozilla;
using Projekt.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.DAL.Entity
{
    class Konto
    {
        public string NumerKonta { get; set; }
        public Int64 WlascicielPesel { get; set; }
        public string TypKonta { get; set; }
        public double Saldo { get; set; }

        public Konto(MySqlDataReader reader)
        {
            NumerKonta = reader["NumerKonta"].ToString();
            WlascicielPesel = Int64.Parse(reader["WlascicielPesel"].ToString());
            TypKonta = reader["TypKonta"].ToString();
            Saldo = double.Parse(reader["Saldo"].ToString());
        }

        public Konto(string accountNumber, Int64 owner, string type)
        {
            NumerKonta = accountNumber;
            WlascicielPesel = owner;
            TypKonta = type;
            Saldo = 150;

        }
    }
}
