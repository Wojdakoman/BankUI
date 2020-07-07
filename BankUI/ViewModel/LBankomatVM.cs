using BankUI.Model;
using BankUI.View;
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
    using R = Properties.Resources;
    class LBankomatVM : ViewModelBase, IPageViewModel
    {
        #region Private Methods
        private KartaPlatnicza _kartaPlatnicza;
        private string numerKarty;
        private ICommand zaloguj;
        private ICommand powrot;
        #endregion
        #region Public Properties
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
        #endregion
        #region Commands
        /// <summary>
        /// Sprawdzenie czy karta o podanym numerze i pinie została wprowadzona poprawnie
        /// </summary>
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
                               KartaPlatnicza test = RepositoryKartaPlatnicza.GetCard(NumerKarty);

                               _kartaPlatnicza.NumerKarty = test.NumerKarty;
                               _kartaPlatnicza.DataWaznosci = test.DataWaznosci;
                               _kartaPlatnicza.LimitPlatnosci = test.LimitPlatnosci;
                               _kartaPlatnicza.NumerKonta = test.NumerKonta;
                               _kartaPlatnicza.Pin = test.Pin;

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
        #endregion
        #region Constructors
        public LBankomatVM(ref KartaPlatnicza kartaPlatnicza)
        {
            _kartaPlatnicza = kartaPlatnicza;
        }
        #endregion
        #region Zasoby
        //Zawiera odwołania do zasobów aplikacji, aby pobrać odpowiednią wersję językową dla kontorlek
        public string RBack { get => R.back; }
        public string RConnect { get => R.connect; }
        public string RCardNumber { get => R.cardNumber; }
        public string RPIN { get => R.pin; }
        #endregion
    }
}
