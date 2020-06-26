using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using BankUI.Properties;


namespace Projekt.DAL
{
    public class DB
    {
        private MySqlConnectionStringBuilder connetion = new MySqlConnectionStringBuilder();

        private static DB instance = null;
        public static DB Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DB();
                }
                return instance;
            }
        }
        private DB()
        {
            connetion.Server = Settings.Default.server;
            connetion.Port = Settings.Default.port;
            connetion.UserID = Settings.Default.userID;
            connetion.Password = Settings.Default.password;
            connetion.Database = Settings.Default.database;
        }
        public MySqlConnection Connection => new MySqlConnection(connetion.ToString());

    }
}
