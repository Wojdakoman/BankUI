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
        public static void TakeCredit(Int64 pesel, string wybraneKonto, double wartosc, int ileMiesiecy)
        {
            string accountNumber = string.Empty;
            bool isCorrect = false;
            while(isCorrect == false)
            {
                accountNumber = Methods.AccountNumberGenerator();
                isCorrect = RepositoryKonto.NumberExist(accountNumber);
            }

            Kredyt kredyt = new Kredyt(pesel, accountNumber, wartosc, ileMiesiecy);

            using (MySqlConnection connection = DB.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand(ADD_CREDIT, connection);
                command.Parameters.Add("@pesel", MySqlDbType.Int64, 11).Value = kredyt.WlascicielPesel;
                command.Parameters.Add("@numer", MySqlDbType.VarChar, 26).Value = kredyt.NumerKonta;
                command.Parameters.Add("@data", MySqlDbType.DateTime).Value = kredyt.DataSplaty;

                MySqlParameter parameter = new MySqlParameter("@wartosc", MySqlDbType.Decimal);
                parameter.Precision = 10;
                parameter.Scale = 2;
                parameter.Value = kredyt.Wartosc;

                command.Parameters.Add(parameter);

                //Nie wiem czy zadziala ale YOLO
                parameter.ParameterName = "@oprocentowanie";
                parameter.Value = kredyt.Oprocentowanie;
                command.Parameters.Add(parameter);

                parameter.ParameterName = "@rata";
                parameter.Value = kredyt.Rata;
                command.Parameters.Add(parameter);

                command.ExecuteNonQuery();

                RepositoryKonto.AddAccount(pesel, accountNumber);

                RepositoryKonto.ChangeBalance(wybraneKonto, wartosc);
            }
        }
    }
}
