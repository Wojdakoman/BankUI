using Projekt.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUI.ViewModel.Classes
{
    class StringHistoriaLogowan
    {
        public string Data { get; }
        public string Godzina { get; }
        public string AdresIP { get; }
        public string Poprawne { get; }

        public StringHistoriaLogowan(HistoriaLogowan log)
        {
            Data = log.DataLogowania;
            Godzina = log.GodzinaLogowania.ToString();
            AdresIP = log.AdresIP;
            Poprawne = log.CzyPoprawne;
        }
    }
}
