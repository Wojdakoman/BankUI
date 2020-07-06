using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Projekt.DAL.Repositories
{
    //Wczytanie wszytkich wplat dla wszsytkich kart przypisanych do wszystkich kont
    //Utworzenie nowej wplaty
    //Usuniecie wplat dla danej karty
    using Projekt.DAL.Entity;
    using System;
    using System.Linq;

    class RepositoryKartaOperacje
    {
        private static string GET_HISTORY = "SELECT * FROM kartaoperacje WHERE KartaPlatniczaNumerKarty = @numer";
        private static string GET_HISTORY_TIME = "SELECT * FROM kartaoperacje WHERE KartaPlatniczaNumerKarty = @numer AND DATE(CzasOperacji)=@czas";
        private static string ADD_OPERATION = "INSERT INTO kartaoperacje (KartaPlatniczaNumerKarty , Typ, Wartosc, CzasOperacji) VALUES (@numer,@typ,@wartosc,@czas)";
        private static string DELETE_OPERATIONS = "DELETE FROM kartaoperacje WHERE KartaPlatniczaNumerKarty=@numer";

        public static Dictionary<string, List<KartaOperacje>> LoadHistory(Dictionary<string, List<KartaPlatnicza>> cards)
        {
            Dictionary<string, List<KartaOperacje>> list = new Dictionary<string, List<KartaOperacje>>();

            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                foreach (var x in cards)
                {
                    foreach(var y in x.Value)
                    {
                        List<KartaOperacje> listOperations = new List<KartaOperacje>();
                        MySqlCommand command = new MySqlCommand(GET_HISTORY, connection);
                        command.Parameters.Add("@numer", MySqlDbType.VarChar, 16).Value = y.NumerKarty;
                        MySqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                listOperations.Add(new KartaOperacje(reader));
                            }
                        }
                        reader.Close();
                        list.Add(y.NumerKarty, listOperations);
                    }
                }
                connection.Close();
            }
            return list;
        }
        //OPERACJA WPLATY/WYPLATY
        //KARTA POWIAZANA Z KONTEM, CZYLI STAN KONTA TEZ SIE ZMIENIA XD
        //1. Dodanie nowej operacji => do karty operacji
        //2. Zmiana stanu konta przypisanego do konkretnej karty
        //      3. Pobranie numeru konta z klasy kontobankowe i wykonanie opearcji na tym koncie

        public static void ExecuteOperation(string cardNumber, string typ, double wartosc, Dictionary<string, List<KartaPlatnicza>> data)
        {
            AddOperation(new KartaOperacje(cardNumber, typ, wartosc));
            //Jest jeszcze druga sciezka, pobierajaca wszystko z klasy KontoBankowe (numerkarty => w KartyPlatnicze i wziecie numeru konta)
            //string accountNumber = RepositoryKartaPlatnicza.ReturnAccountNumber(cardNumber);

            //Czyli druga opcja:
            string accountNumber = data.FirstOrDefault(x => x.Value.Any(y => y.NumerKarty == cardNumber)).Key;

            //Zla kolejnosc => brak sprawdzenia czy jest wystarczajaca ilosc srodkow na koncie xD (czyli dodajemy operacje, a anulujemy z powodu braku srodkow)
            //Wykonanie zmiany salda na koncie o podanym wyzej numerze
            if (typ == "wyplata")
                wartosc = -wartosc;
            RepositoryKonto.ChangeBalance(accountNumber, wartosc);
        }

        public static void ExecuteOperation(string cardNumber, string typ, double wartosc, string accountNumber)
        {
            AddOperation(new KartaOperacje(cardNumber, typ, wartosc));
            if (typ == "wyplata")
                wartosc = -wartosc;
            RepositoryKonto.ChangeBalance(accountNumber, wartosc);
        }

        public static void AddOperation(KartaOperacje cardOperation)
        {
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(ADD_OPERATION, connection);
                command.Parameters.Add("@numer", MySqlDbType.VarChar, 16).Value = cardOperation.KartaPlatniczaNumerKarty;
                command.Parameters.Add("@typ", MySqlDbType.VarChar).Value = cardOperation.Typ;
                command.Parameters.Add("@czas", MySqlDbType.DateTime).Value = cardOperation.CzasOperacji;
                MySqlParameter parameter = new MySqlParameter("@wartosc", MySqlDbType.Decimal);
                parameter.Precision = 10;
                parameter.Scale = 2;
                parameter.Value = cardOperation.Wartosc;
                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static void DeleteOperations(string cardNumber)
        {
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(DELETE_OPERATIONS, connection);
                command.Parameters.Add("@numer", MySqlDbType.VarChar, 16).Value = cardNumber;
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        /// <summary>
        /// Sprawdzenie czy dana operacja nie wykrocza poza dzienny limit karty
        /// </summary>
        /// <param name="kartaPlatnicza"></param>
        /// <param name="kwotaWyplaty"></param>
        public static bool CheckLimit(KartaPlatnicza kartaPlatnicza, double kwotaWyplaty)
        {
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(GET_HISTORY_TIME, connection);

                command.Parameters.Add("@numer", MySqlDbType.VarChar, 16).Value = kartaPlatnicza.NumerKarty;
                command.Parameters.Add("@czas", MySqlDbType.DateTime).Value = DateTime.Now.ToShortTimeString();

                MySqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    List<KartaOperacje> operacje = new List<KartaOperacje>();
                    while (reader.Read())
                    {
                        operacje.Add(new KartaOperacje(reader));
                    }

                    double sumaWyplat = kwotaWyplaty;
                    foreach (var x in operacje)
                    {
                        if (x.Typ == "wyplata")
                        {
                            sumaWyplat += x.Wartosc;
                        }
                    }

                    if (sumaWyplat > kartaPlatnicza.LimitPlatnosci)
                        return false;
                    else
                        return true;
                }
                else
                    return true;
            }
        }
    }
}