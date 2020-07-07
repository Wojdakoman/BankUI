﻿using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Classes;
using BankUI.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BankUI.ViewModel
{
    using R = Properties.Resources;
    class KredytVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        private AppGlobalInfo _kredytInfo; //wykorzystuje klase do przeslania danych do kredytu po wyborze opcji "Splac kredyt"
        #region PUBLIC
        public string UserName { get => _model.WlascicielName; }
        public List<string> ListaKont { get => _model.NumeryKont; }
        public int ListaKontIndex { get; set; }
        public List<StringKredyt> Lista { get => _model.Kredyty; } //lista zaciagnietych kredytow dla wlasciciela
        #endregion
        public KredytVM(ref Data model, ref AppGlobalInfo kredyt)
        {
            _model = model;
            _kredytInfo = kredyt;
        }

        #region Komendy
        private ICommand _onLoad = null;
        public ICommand OnLoad
        {
            get
            {
                if (_onLoad == null)
                {
                    _onLoad = new RelayCommand(
                        arg =>
                        {
                            ListaKontIndex = _model.Konto;
                            OnPropertyChanged(nameof(ListaKontIndex), nameof(UserName), nameof(ListaKont), nameof(Lista));
                        },
                        arg => true
                    );
                }
                return _onLoad;
            }
        }

        private ICommand _zmienKonto = null;
        public ICommand ZmienKonto
        {
            get
            {
                if (_zmienKonto == null)
                {
                    _zmienKonto = new RelayCommand(
                        arg =>
                        {
                            _model.Konto = ListaKontIndex;
                            OnPropertyChanged(nameof(ListaKont));
                        },
                        arg => true
                    );
                }
                return _zmienKonto;
            }
        }

        private ICommand _splacRate = null;
        public ICommand SplacRate
        {
            get
            {
                if (_splacRate == null)
                {
                    _splacRate = new RelayCommand((parameter)
                        =>
                    {
                        int ID = Convert.ToInt32(parameter); //numer kredytu, dla ktorego wcisniety zostal przycisk
                        foreach(var kredyt in Lista)
                        {
                            if(kredyt.IDKredytu == ID)
                            {
                                _kredytInfo.DaneKredyt = kredyt; //przkazanie danych o kredycie do klasy AppGlobalInfo aby byly dostepne do wczytania w oknie Przelewy
                                break;
                            }
                        }
                        _kredytInfo.HasData = true;

                        Mediator.Notify("GoToPage", "przelew");
                    },
                        arg => true
                    );
                }
                return _splacRate;
            }
        }

        private ICommand _wyloguj = null;
        public ICommand Wyloguj
        {
            get
            {
                if (_wyloguj == null)
                {
                    _wyloguj = new RelayCommand(
                        arg =>
                        {
                            Mediator.Notify("Wyloguj", "login");
                        },
                        arg => true
                    );
                }
                return _wyloguj;
            }
        }
        #region goTo
        private ICommand _goTo = null;
        public ICommand GoTo
        {
            get
            {
                if (_goTo == null)
                {
                    _goTo = new RelayCommand((parameter)
                        =>
                    {
                        string page = parameter.ToString();
                        Mediator.Notify("GoToPage", page);
                    },
                        arg => true
                    );
                }
                return _goTo;
            }
        }
        #endregion
        #endregion

        #region Zasoby
        //Zawiera odwołania do zasobów aplikacji, aby pobrać odpowiednią wersję językową dla kontorlek
        #region menu
        public string RActiveAccount { get => R.activeAccount; }
        public string RLogout { get => R.logout; }
        public string RTransfers { get => R.transfers; }
        public string RAccount { get => R.account; }
        public string RCards { get => R.cards; }
        public string RMyData { get => R.myData; }
        public string RLoginHistory { get => R.loginHistory; }
        public string RTakeLoan { get => R.takeLoan; }
        #endregion
        #region table
        public string RLoanValue { get => R.loanValue; }
        public string RPaidBack { get => R.paidBack; }
        public string RPayDate { get => R.payDate; }
        public string RAccountNumber { get => R.accountNumber; }
        public string RInterest { get => R.interest; }
        public string RInstallment { get => R.installment; }
        public string RPayInstall { get => R.payInstallment; }
        #endregion
        #endregion
    }
}
