using System.Collections.Generic;
using System;

namespace Projekt.DAL.Repositories
{
    using MySql.Data.MySqlClient;
    using Projekt.DAL.Entity;

    static class RepositoryHistoriaLogowan
    {
        private static string INSERT_DATA = "INSERT INTO historialogowan(WlascicielPesel, DataLogowania, GodzinaLogowania, AdresIP, CzyPoprawne) VALUES ( @pesel, @data, @godzina, @ip, @poprawne )";
        private static string GET_HISTORY = "SELECT * FROM historialogowan WHERE WlascicielPesel=@pesel";

        /// <summary>
        /// Wczytaj historie logowań dla przypisanego właściciela
        /// </summary>
        /// <param name="pesel"></param>
        /// <returns></returns>
        public static List<HistoriaLogowan> LoadHistory(Int64 pesel)
        {
            List<HistoriaLogowan> list = new List<HistoriaLogowan>();

            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(GET_HISTORY, connection);

                command.Parameters.Add("@pesel", MySqlDbType.Int64).Value = pesel;

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new HistoriaLogowan(reader));
                    }
                }
                connection.Close();
            }
            return list;
        }

        /// <summary>
        /// Dodanie nowej próby logowania
        /// </summary>
        /// <param name="data"></param>
        public static void CreateHistory(HistoriaLogowan data)
        {
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(INSERT_DATA, connection);

                command.Parameters.Add("@pesel", MySqlDbType.Int64).Value = data.WlascicielPesel;
                command.Parameters.Add("@data", MySqlDbType.DateTime).Value = data.DataLogowania;
                command.Parameters.Add("@godzina", MySqlDbType.Time).Value = data.GodzinaLogowania;
                command.Parameters.Add("@ip", MySqlDbType.VarChar, 15).Value = data.AdresIP;
                command.Parameters.AddWithValue("@poprawne", data.CzyPoprawne);


                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
