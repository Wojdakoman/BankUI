using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.DAL.Repositories
{
    using Projekt.DAL.Entity;
    static class RepositoryPrzelew
    {

        private static string GET_OPERATIONS = "SELECT * FROM przelew WHERE NumerNadawcy=@numer or NumerOdbiorcy=@numer";
        private static string ADD_OPERATION = "INSERT INTO przelew (NumerNadawcy, NumerOdbiorcy, Wartosc, CzasOperacji, Tytul, Opis) VALUES (@nadawca, @odbiorca, @wartosc, @czas, @tytul, @opis)";

        /// <summary>
        /// Wczytanie histori operacji dla każdego konta
        /// </summary>
        /// <param name="listaKont"></param>
        /// <returns></returns>
        public static Dictionary<string,List<Przelew>> LoadOperations(List<Konto> listaKont)
        {
            Dictionary<string, List<Przelew>> dictionaryOfOperation = new Dictionary<string, List<Przelew>>();
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();

                foreach(Konto x in listaKont)
                {
                    List<Przelew> listOfOpeations = new List<Przelew>();
                    MySqlCommand command = new MySqlCommand(GET_OPERATIONS, connection);
                    command.Parameters.Add("@numer", MySqlDbType.VarChar, 26).Value = x.NumerKonta;

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            listOfOpeations.Add(new Przelew(reader));
                        }
                    }
                    dictionaryOfOperation.Add(x.NumerKonta, listOfOpeations);
                    reader.Close();
                }
                connection.Close();
            }
            return dictionaryOfOperation;
        }
        /// <summary>
        /// Wykonanie przelewu
        /// </summary>
        /// <param name="kontoNadawca"></param>
        /// <param name="odbiorca"></param>
        /// <param name="wartosc"></param>
        /// <param name="tytul"></param>
        /// <param name="opis"></param>
        public static void ExecuteOperation(Konto kontoNadawca, string odbiorca, double wartosc, string tytul, string opis)
        {
            //Utworz nowy obiekt PRZELEW
            Przelew operation = new Przelew(kontoNadawca.NumerKonta, odbiorca, wartosc, tytul, opis);
            //Dodaj go do bazy danych
            AddOperation(operation);
            //Zmien wartosci sald, tych konto
            //Brak sprawdzania czy np. konta nadawcy ma odpowiednie środki => to gdzieś w mvvm
            RepositoryKonto.ChangeBalanceT(kontoNadawca.NumerKonta, odbiorca, wartosc);

        }
        /// <summary>
        /// Dodanie przelewu jako operacji do bazy danych
        /// </summary>
        /// <param name="operation"></param>
        public static void AddOperation(Przelew operation)
        {
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand(ADD_OPERATION, connection);

                command.Parameters.Add("@nadawca", MySqlDbType.VarChar, 26).Value = operation.NumerNadawcy;
                command.Parameters.Add("@odbiorca", MySqlDbType.VarChar, 26).Value = operation.NumerOdbiorcy;
                
                MySqlParameter parameter = new MySqlParameter("@wartosc", MySqlDbType.Decimal);
                parameter.Precision = 10;
                parameter.Scale = 2;
                parameter.Value = operation.Wartosc;

                command.Parameters.Add(parameter);
                command.Parameters.Add("@czas", MySqlDbType.DateTime).Value = operation.CzasOperacji;
                command.Parameters.Add("@tytul", MySqlDbType.VarChar, 20).Value = operation.Tytul;
                command.Parameters.Add("@opis", MySqlDbType.VarChar, 50).Value = operation.Opis;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

    }
}
