using MySql.Data.MySqlClient;
using Projekt.Class;
using Projekt.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.DAL.Repositories
{
    class RepositoryKredyt
    {
        private static string ADD_CREDIT = "INSERT INTO kredyt (WlascicielPesel, Wartosc, NumerKonta, DataSplaty, Oprocentowanie, Rata) VALUES (@pesel, @wartosc, @numer, @data, @oprocentowanie, @rata)";
        private static string GET_CREDITS = "SELECT * FROM kredyt WHERE WlascicielPesel=@pesel";
        private static string DEL_CREDIT = "DELETE FROM kredyt WHERE NumerKredytu=@numer";

        /// <summary>
        /// Utworzenie nowego kredytu
        /// </summary>
        /// <param name="pesel"></param>
        /// <param name="wybraneKonto"></param>
        /// <param name="wartosc"></param>
        /// <param name="ileMiesiecy"></param>
        public static void TakeCredit(Int64 pesel, string wybraneKonto, double wartosc, int ileMiesiecy)
        {
            string accountNumber = string.Empty;
            bool isCorrect = false;
            while (isCorrect == false)
            {
                accountNumber = Methods.AccountNumberGenerator();
                isCorrect = RepositoryKonto.NumberExist(accountNumber);
            }

            Kredyt kredyt = new Kredyt(pesel, accountNumber, wartosc, ileMiesiecy);

            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(ADD_CREDIT, connection);
                command.Parameters.Add("@pesel", MySqlDbType.Int64, 11).Value = kredyt.WlascicielPesel;
                command.Parameters.Add("@numer", MySqlDbType.VarChar, 26).Value = kredyt.NumerKonta;
                command.Parameters.Add("@data", MySqlDbType.DateTime).Value = kredyt.DataSplaty;

                MySqlParameter parameter = new MySqlParameter("@wartosc", MySqlDbType.Decimal);
                parameter.Precision = 10;
                parameter.Scale = 2;
                parameter.Value = kredyt.Wartosc;

                command.Parameters.Add(parameter);

                parameter = new MySqlParameter("@oprocentowanie", MySqlDbType.Decimal);
                parameter.Value = kredyt.Oprocentowanie;
                command.Parameters.Add(parameter);

                parameter = new MySqlParameter("@rata", MySqlDbType.Decimal);
                parameter.Value = kredyt.Rata;
                command.Parameters.Add(parameter);

                command.ExecuteNonQuery();

                RepositoryKonto.AddAccount(pesel, accountNumber);

                RepositoryKonto.ChangeBalance(wybraneKonto, wartosc);

                connection.Close();
            }
        }

        /// <summary>
        /// Wczytanie listy kredytow
        /// </summary>
        /// <param name="pesel"></param>
        /// <returns></returns>
        public static List<Kredyt> LoadCredits(Int64 pesel)
        {
            List<Kredyt> credits = new List<Kredyt>();
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(GET_CREDITS, connection);
                command.Parameters.Add("@pesel", MySqlDbType.Int64).Value = pesel;

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        credits.Add(new Kredyt(reader));
                    }
                }
                reader.Close();
                
                connection.Close();
            }
            return credits;
        }

        /// <summary>
        /// Usuwanie kredytu z tabeli
        /// </summary>
        /// <param name="id">Numer kredytu</param>
        public static void DeleteCredit(int id)
        {
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(DEL_CREDIT, connection);

                command.Parameters.Add("@numer", MySqlDbType.Int32, 10).Value = id;

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
