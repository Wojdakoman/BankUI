using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Interfaces;
using Org.BouncyCastle.Asn1.Nist;
using Projekt.DAL.Entity;
using Projekt.DAL.Repositories;
using Renci.SshNet.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BankUI.ViewModel
{
    class LBankomatVM : ViewModelBase, IPageViewModel
    {
        private KartaPlatnicza _kartaPlatnicza;
        private string numerKarty;
        private ICommand zaloguj;
        private ICommand powrot;
        public string Pin { get; set; }
        public string NumerKarty
        {
            get
            {
                return numerKarty;
            }
            set
            {
                numerKarty = value;
                OnPropertyChanged(nameof(NumerKarty));
            }
        }
        public ICommand Zaloguj
        {
            get
            {
                if (zaloguj == null)
                {
                    zaloguj = new RelayCommand(
                       arg =>
                       {

                           if (RepositoryKartaPlatnicza.DoesCardExist(NumerKarty, Pin))
                           {
                               _kartaPlatnicza = RepositoryKartaPlatnicza.GetCard(NumerKarty);
                               Mediator.Notify("GoToPage", "bankomat");
                           }
                           else
                           {
                               MessageBox.Show("Błąd danych");
                               NumerKarty = string.Empty;
                           }
                       },
                        arg => NumerKarty != null && NumerKarty.Length == 16 && Pin != null && Pin.Length == 4
                    );
                }
                return zaloguj;
            }
        }
        public LBankomatVM(ref KartaPlatnicza kartaPlatnicza)
        {
            _kartaPlatnicza = kartaPlatnicza;
        }

        public ICommand Powrot
        {
            get
            {
                if (powrot == null)
                {
                    powrot = new RelayCommand(
                        arg =>
                        {
                            Mediator.Notify("GoToPage", "login");
                        },
                arg => true);
                }
                return powrot;
            }
            set
            {
                powrot = value;
            }
        }


    }
}
