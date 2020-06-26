using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Projekt.DAL.Repositories
{
    using Projekt.Class;
    using Projekt.DAL.Entity;

    class RepositoryKartaPlatnicza
    {
        private static string LOAD_CARDS = "SELECT * FROM kartaplatnicza WHERE NumerKonta = @numer";
        private static string FIND_CARD = "SELECT * FROM kartaplatnicza WHERE NumerKarty = @numer";
        private static string ADD_CARD = "INSERT INTO kartaplatnicza (NumerKarty, NumerKonta, DataWaznosci, LimitPlatnosci, Pin) VALUES (@numerKarty, @numerKonta, @data, @limit, @pin)";
        private static string DELETE_CARD = "DELETE FROM kartaplatnicza WHERE NumerKarty=@numer";
        private static string GET_ACCOUNT_NUMBER = "SELECT NumerKonta FROM kartaplatnicza WHERE NumerKarty=@numer";

        //Wczytanie wszystkich kart platniczych
        public static Dictionary<string,List<KartaPlatnicza>> LoadCards(List<Konto> accounts)
        {
            Dictionary<string, List<KartaPlatnicza>> listOfCards = new Dictionary<string, List<KartaPlatnicza>>();
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();

                foreach (Konto x in accounts)
                {
                    List<KartaPlatnicza> list = new List<KartaPlatnicza>();
                    MySqlCommand command = new MySqlCommand(LOAD_CARDS, connection);
                    command.Parameters.Add("@numer", MySqlDbType.VarChar, 26).Value = x.NumerKonta;
                    MySqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            list.Add(new KartaPlatnicza(reader));
                        }
                    }
                    reader.Close();
                    listOfCards.Add(x.NumerKonta, list);
                }
                connection.Close();
            }
            return listOfCards;
        }

        //Dodanie nowej karty
        public static void AddCard(Dictionary<string, List<KartaPlatnicza>> list, string accountNumber)
        {
            string cardNumber = string.Empty;
            bool isUnique = false;
            while (isUnique == false)
            {
                cardNumber = Methods.CardNumberGenerator();
                isUnique = CardExists(cardNumber);
            }
            string pin = Methods.PinCodeGenerator();
            KartaPlatnicza newCard = new KartaPlatnicza(cardNumber, accountNumber, pin);
            if (list.ContainsKey(accountNumber))
            {
                List<KartaPlatnicza> listCopy = list[accountNumber];
                listCopy.Add(newCard);
                list[accountNumber] = listCopy;
            }
            else
            {
                List<KartaPlatnicza> listOfCards = new List<KartaPlatnicza>();
                listOfCards.Add(newCard);
                list.Add(accountNumber, listOfCards);
            }
            //Dodanie karty do bazy
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(ADD_CARD, connection);
                command.Parameters.Add("@numerKarty", MySqlDbType.VarChar, 16).Value = newCard.NumerKarty;
                command.Parameters.Add("@numerKonta", MySqlDbType.VarChar, 26).Value = newCard.NumerKonta;
                command.Parameters.Add("@data", MySqlDbType.Date).Value = newCard.DataWaznosci;
                command.Parameters.Add("@pin", MySqlDbType.Int32, 4).Value = newCard.Pin;
                MySqlParameter parameter = new MySqlParameter("@limit", MySqlDbType.Decimal);
                parameter.Precision = 10;
                parameter.Scale = 2;
                parameter.Value = newCard.LimitPlatnosci;
                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        //Sprawdzenie czy wygenerowana karta juz istnieje, jak nie to dodanie
        private static bool CardExists(string cardNumber)
        {
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(FIND_CARD, connection);

                command.Parameters.Add("@numer", MySqlDbType.VarChar, 16).Value = cardNumber;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    return false;
                else
                    return true;
            }
        }
        //Wypadaloby tez usunac historie wplat dla usuwanej karty
        public static void DeleteCard(Dictionary<string, List<KartaPlatnicza>> list, string accountNumber, string cardNumber)
        {
            //Usuniecie histori podanej wyzej karty
            RepositoryKartaOperacje.DeleteOperations(cardNumber);

            //Usuniecie karty z listy (zamiast wczytywać na nowo karty z bazy)
            int index = list[accountNumber].FindIndex(x => x.NumerKarty == cardNumber);
            list[accountNumber].RemoveAt(index);
            
            //Usuniecie karty z bazy
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(DELETE_CARD, connection);

                command.Parameters.Add("@numer", MySqlDbType.VarChar, 16).Value = cardNumber;
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        //Zapytanie o numer konta;
        public static string ReturnAccountNumber(string cardNumber)
        {
            //Jako ze karta ma przypisane dokladnie jedno konto
            string accountNumber = string.Empty;
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(GET_ACCOUNT_NUMBER, connection);
                command.Parameters.Add("@numer", MySqlDbType.VarChar, 16).Value = cardNumber;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    accountNumber = reader.ToString();
                }
                connection.Close();
            }
            return accountNumber;
        }
    }
}
