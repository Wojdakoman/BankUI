using System;
using MySql.Data.MySqlClient;

namespace Projekt.DAL.Repositories
{
    using Projekt.DAL.Entity;

    static class RepositoryWlasciciel
    {
        private static string FIND_OWNER = "SELECT * FROM wlasciciel WHERE login=@login AND haslo=@password";
        private static string FIND_PESEL = "SELECT * FROM wlasciciel WHERE login=@login";
        private static string CREATE_OWNER = "INSERT INTO wlasciciel VALUES (@pesel, @imie, @nazwisko, @data, @miasto, @adres, @telefon, @login, @haslo)";

        public static Wlasciciel FindOwner(string login, string password)
        {
            Wlasciciel wlasciciel = null;
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand(FIND_OWNER, connection);

                command.Parameters.Add("@login", MySqlDbType.VarChar, 20).Value = login;
                command.Parameters.Add("@password", MySqlDbType.VarChar, 15).Value = password;

                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    wlasciciel =  new Wlasciciel(reader);
                    RepositoryHistoriaLogowan.CreateHistory(new HistoriaLogowan(wlasciciel.Pesel, true));
                }
                else
                {
                    //Utworz probe logowania
                    reader.Close();
                    command = new MySqlCommand(FIND_PESEL, connection);
                    command.Parameters.Add("login", MySqlDbType.VarChar, 20).Value = login;

                    reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        //utworz probe logowania;
                        reader.Read();
                        RepositoryHistoriaLogowan.CreateHistory(new HistoriaLogowan(Int64.Parse(reader["Pesel"].ToString()), false));
                    }
                }
                connection.Close();
            }
            return wlasciciel;
        }
        public static void CreateOwner(Wlasciciel owner)
        {
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(CREATE_OWNER, connection);

                command.Parameters.Add("@pesel", MySqlDbType.Int64).Value = owner.Pesel;
                command.Parameters.Add("@imie", MySqlDbType.VarChar,20).Value = owner.Imie;
                command.Parameters.Add("@nazwisko", MySqlDbType.VarChar,20).Value = owner.Nazwisko;
                command.Parameters.Add("@data", MySqlDbType.Date).Value = owner.DataUrodzenia;
                command.Parameters.Add("@miasto", MySqlDbType.VarChar,30).Value = owner.Miasto;
                command.Parameters.Add("@adres", MySqlDbType.VarChar,30).Value = owner.Adres;
                command.Parameters.Add("@telefon", MySqlDbType.Int32,10).Value = owner.Telefon;
                command.Parameters.Add("@login", MySqlDbType.VarChar,20).Value = owner.Login;
                command.Parameters.Add("@haslo", MySqlDbType.VarChar,15).Value = owner.Haslo;

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
    }
}
