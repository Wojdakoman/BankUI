using MySql.Data.MySqlClient;
using Projekt.API;
using System;

namespace Projekt.DAL.Entity
{
    class HistoriaLogowan
    {
        public int? idHistoriaLogowan { get; set; }
        public Int64 WlascicielPesel { get; set; }
        public string DataLogowania { get; set; }
        public TimeSpan GodzinaLogowania { get; set; }
        public string AdresIP { get; set; }
        public string CzyPoprawne { get; set; }


        public HistoriaLogowan(MySqlDataReader reader)
        {
            idHistoriaLogowan = int.Parse(reader["idHistoriaLogowan"].ToString());
            WlascicielPesel = Int64.Parse(reader["WlascicielPesel"].ToString());
            DataLogowania = DateTime.Parse(reader["DataLogowania"].ToString()).ToShortDateString();
            GodzinaLogowania = TimeSpan.Parse(reader["GodzinaLogowania"].ToString());
            AdresIP = reader["AdresIP"].ToString();
            CzyPoprawne = reader["CzyPoprawne"].ToString();
        }

        public HistoriaLogowan(Int64 wlascicielPesel , bool czyPoprawne)
        {
            DateTime timeDay = DateTime.Now;

            WlascicielPesel = wlascicielPesel;
            DataLogowania = timeDay.Date.ToString("yyyy-MM-dd");
            GodzinaLogowania = new TimeSpan(timeDay.Hour, timeDay.Minute, timeDay.Second);
            AdresIP = IPBody.IP;

            if (czyPoprawne)
            {
                CzyPoprawne = "Tak";
            }
            else
            {
                CzyPoprawne = "Nie";
            }
        }

        public override string ToString()
        {
            return $"{WlascicielPesel} {DataLogowania} {GodzinaLogowania} {AdresIP} {CzyPoprawne}";
        }
    }
}
