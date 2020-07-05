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
        private KartaPlatnicza _kartaPlatnicza;
        private ICommand wykonaj;


        public string Wybrany { get; set; }
        public int? Typ { get; set; }
        
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
                           MessageBox.Show(_kartaPlatnicza.NumerKarty);
                           //Operacja karta
                           if (wybranyTyp == "wyplata")
                           {
                               //Sprawdz dostepne srodki
                               if (RepositoryKonto.CheckBalance(_kartaPlatnicza.NumerKarty, double.Parse(Wybrany)))
                               {
                                   RepositoryKartaOperacje.ExecuteOperation(_kartaPlatnicza.NumerKarty, wybranyTyp, double.Parse(Wybrany), _kartaPlatnicza.NumerKonta);
                                   MessageBox.Show("Operacja wykonana poprawnie");
                                   Mediator.Notify("GoToPage", "login");
                               }
                               else
                                   MessageBox.Show("Brak wystarczajacej ilosci srodkow na koncie");
                           }
                           else
                           {
                               RepositoryKartaOperacje.ExecuteOperation(_kartaPlatnicza.NumerKarty, wybranyTyp, double.Parse(Wybrany), _kartaPlatnicza.NumerKonta);
                               MessageBox.Show("Operacja wykonana poprawnie");
                               Mediator.Notify("GoToPage", "login");
                           }
                       },
                        arg => Wybrany != null && Typ != null
                    );
                }
                return wykonaj;
            }
        }
        public BankomatVM(ref KartaPlatnicza kartaPlatnicza)
        {
            _kartaPlatnicza = kartaPlatnicza;
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

        #region Zasoby
        //Zawiera odwołania do zasobów aplikacji, aby pobrać odpowiednią wersję językową dla kontorlek
        public string RBack { get => R.back; }
        public string RExec { get => R.execute; }
        #endregion
    }
}
