﻿using System;
using MySql.Data.MySqlClient;
using System.Collections.Generic;

namespace Projekt.DAL.Repositories
{
    using Projekt.DAL.Entity;

    static class RepositoryKonto
    {
        private static string GET_ACCOUNT = "SELECT * FROM konto WHERE WlascicielPesel=@pesel";
        private static string UPDATE_BALANCE = "UPDATE konto SET saldo = saldo + @ile WHERE NumerKonta = @numer";
        private static string ADD_NEW_ACCOUNT = "INSERT INTO konto VALUES (@numer, @pesel, @typ, @saldo)";
        private static string ADD_NEW_CREDIT_ACCOUNT = "INSERT INTO konto (NumerKonta, WlascicielPesel, TypKonta, Saldo ) VALUES (@numer, @pesel, @typ, @saldo)";
        private static string NUMBER_EXIST = "SELECT * FROM konto WHERE NumerKonta=@numer";


        /// <summary>
        /// Wczytanie kont dla konkretnego właściciela
        /// </summary>
        /// <param name="pesel"></param>
        /// <returns></returns>
        public static List<Konto> GetAccount(Int64 pesel)
        {
            List<Konto> list = new List<Konto>();
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand(GET_ACCOUNT, connection);
                command.Parameters.Add("@pesel", MySqlDbType.Int64).Value = pesel;

                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["TypKonta"].ToString() != "Kredytowe")
                            list.Add(new Konto(reader));
                    }
                }
                connection.Close();
            }
            return list;
        }
        /// <summary>
        /// Wczytanie kont kredytowych
        /// </summary>
        /// <param name="pesel"></param>
        /// <returns></returns>
        public static List<Konto> GetCreditAccount(Int64 pesel)
        {
            List<Konto> list = new List<Konto>();
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                MySqlCommand command = new MySqlCommand(GET_ACCOUNT, connection);
                command.Parameters.Add("@pesel", MySqlDbType.Int64).Value = pesel;

                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader["TypKonta"].ToString() == "Kredytowe")
                            list.Add(new Konto(reader));
                    }
                }
                connection.Close();
            }
            return list;
        }

        /// <summary>
        /// Zmiana salda kont przy wykonywaniu przelewu
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="receiverNumber"></param>
        /// <param name="money"></param>
        public static void ChangeBalanceT(string accountNumber, string receiverNumber, double money)
        {
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                /*MySqlCommand command = new MySqlCommand(UPDATE_BALANCE, connection);
                MySqlParameter parameter = new MySqlParameter("@ile", MySqlDbType.Decimal);
                parameter.Precision = 10;
                parameter.Scale = 2;
                parameter.Value = Math.Round(money, 2);

                command.Parameters.Add(parameter);
                command.Parameters.Add("@numer", MySqlDbType.VarChar, 26).Value = accountNumber;

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();*/
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                MySqlTransaction transaction = connection.BeginTransaction();

                command.Connection = connection;
                command.Transaction = transaction;

                command.CommandText = UPDATE_BALANCE;
                MySqlParameter parameter = new MySqlParameter("@ile", MySqlDbType.Decimal);
                parameter.Precision = 10;
                parameter.Scale = 2;
                parameter.Value = -Math.Round(money, 2);
                command.Parameters.Add(parameter);
                command.Parameters.Add("@numer", MySqlDbType.VarChar, 26).Value = accountNumber;

                command.ExecuteNonQuery();
                command.Parameters["@ile"].Value = Math.Round(money, 2);
                command.Parameters["@numer"].Value = receiverNumber;

                command.ExecuteNonQuery();

                transaction.Commit();
                connection.Close();
            }
        }
        /// <summary>
        /// Wykonaj wplate/wyplate 
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="money"></param>
        public static void ChangeBalance(string accountNumber, double money)
        {
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(UPDATE_BALANCE, connection);
                command.Parameters.Add("@numer", MySqlDbType.VarChar, 26).Value = accountNumber;
                MySqlParameter parameter = new MySqlParameter("@ile", MySqlDbType.Decimal);
                parameter.Precision = 10;
                parameter.Scale = 2;
                parameter.Value = money;

                command.Parameters.Add(parameter);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        /// <summary>
        /// Dodanie nowego konta
        /// </summary>
        /// <param name="konto"></param>
        public static void AddAccount(Konto konto)
        {
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(ADD_NEW_ACCOUNT, connection);
                command.Parameters.Add("@numer", MySqlDbType.VarChar, 26).Value = konto.NumerKonta;
                command.Parameters.Add("@pesel", MySqlDbType.Int64, 11).Value = konto.WlascicielPesel;
                command.Parameters.Add("@typ", MySqlDbType.VarChar).Value = konto.TypKonta;

                MySqlParameter parameter = new MySqlParameter("@saldo", MySqlDbType.Decimal);
                parameter.Precision = 10;
                parameter.Scale = 2;
                parameter.Value = konto.Saldo;
                command.Parameters.Add(parameter);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        public static void AddAccount(Int64 pesel, string numer)
        {
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(ADD_NEW_CREDIT_ACCOUNT, connection);
                command.Parameters.Add("@numer", MySqlDbType.VarChar, 26).Value = numer;
                command.Parameters.Add("@pesel", MySqlDbType.Int64, 11).Value = pesel;
                command.Parameters.Add("@typ", MySqlDbType.VarChar).Value = "Kredytowe";
                command.Parameters.Add("@saldo", MySqlDbType.Decimal).Value = 0;

                command.ExecuteNonQuery();
                connection.Close();
            }
        }
        /// <summary>
        /// Sprawdzenie czy instnieje konto o podanym numerze
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <returns></returns>
        public static bool NumberExist(string accountNumber)
        {
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(NUMBER_EXIST, connection);
                command.Parameters.Add("@numer", MySqlDbType.VarChar, 26).Value = accountNumber;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                    return false;
                else
                    return true;
                //Tu zamkniecie polaczenie pozostawimy programowani
            }
        }

        /// <summary>
        /// Sprawdzenie balansu konta
        /// </summary>
        /// <param name="accountNumber"></param>
        /// <param name="wartosc"></param>
        /// <returns></returns>
        public static bool CheckBalance(string accountNumber, double wartosc)
        {
            using (MySqlConnection connection = DB.Instance.Connection)
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(NUMBER_EXIST, connection);
                command.Parameters.Add("@numer", MySqlDbType.VarChar, 26).Value = accountNumber;
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    Konto konto = new Konto(reader);
                    if (konto.Saldo >= wartosc)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                    return false;
            }
        }
    }
}
