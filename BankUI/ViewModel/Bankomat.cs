using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Interfaces;
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
    class BankomatVM : ViewModelBase, IPageViewModel
    {
        #region Private Method
        private KartaPlatnicza _kartaPlatnicza;
        private ICommand wykonaj;
        #endregion
        #region Public Properties
        public string Kwota { get; set; }
        public int? Typ { get; set; }
        #endregion
        #region Commands
        /// <summary>
        /// Wykonanie operacji wpłaty/wypłaty
        /// W przypadku wypłaty:
        /// Sprawdzany jest limit dzienny przypisany do danej karty
        /// Oraz czy stan konta pozwala na wypłate
        /// </summary>
        public ICommand Wykonaj
        {
            get
            {
                if (wykonaj == null)
                {
                    wykonaj = new RelayCommand(
                       arg =>
                       {
                           string wybranyTyp = Typ == 0 ? "wplata" : "wyplata";
                           //Operacja karta
                           if (wybranyTyp == "wyplata")
                           {
                               if (RepositoryKartaOperacje.CheckLimit(_kartaPlatnicza, double.Parse(Kwota)))
                               {
                                   //Sprawdz dostepne srodki
                                   if (RepositoryKonto.CheckBalance(_kartaPlatnicza.NumerKonta, double.Parse(Kwota)))
                                   {
                                       RepositoryKartaOperacje.ExecuteOperation(_kartaPlatnicza.NumerKarty, wybranyTyp, double.Parse(Kwota), _kartaPlatnicza.NumerKonta);
                                       MessageBox.Show(R.operationSuccessful);
                                       Mediator.Notify("GoToPage", "login");
                                   }
                                   else
                                       MessageBox.Show(R.lackMoney);
                               }
                               else
                                   MessageBox.Show(R.limitExceed);
                           }
                           else
                           {
                               RepositoryKartaOperacje.ExecuteOperation(_kartaPlatnicza.NumerKarty, wybranyTyp, double.Parse(Kwota), _kartaPlatnicza.NumerKonta);
                               MessageBox.Show(R.operationSuccessful);
                               Mediator.Notify("GoToPage", "login");
                           }
                       },
                        arg => Kwota != null && Typ != null
                    );
                }
                return wykonaj;
            }
        }


        private ICommand powrot;
        public ICommand Powrot
        {
            get
            {
                if (powrot == null)
                {
                    powrot = new RelayCommand(
                        arg =>
                        {
                            Mediator.Notify("GoToPage", "lBankomat");
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
        #region Contructor
        public BankomatVM(ref KartaPlatnicza kartaPlatnicza)
        {
            _kartaPlatnicza = kartaPlatnicza;
        }
        #endregion
        #region Zasoby
        //Zawiera odwołania do zasobów aplikacji, aby pobrać odpowiednią wersję językową dla kontorlek
        public string RBack { get => R.back; }
        public string RExec { get => R.execute; }
        public string RWithdraw { get => R.withdraw; }
        public string RDeposit { get => R.deposit; }
        #endregion
    }
}
