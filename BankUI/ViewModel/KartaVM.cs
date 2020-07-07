﻿using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Classes;
using BankUI.ViewModel.Interfaces;
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
    class KartaVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        private AppGlobalInfo _appInfo; //wykorzystuje klase do odebrania numeru karty, ktora ma byc wyswietlana
        #region PUBLIC
        public string UserName { get => _model.WlascicielName; }
        #region DaneKarty
        public string NumerKarty { get; set; }
        public string NumerKonta { get; set; }
        public string Data { get; set; }
        public double? Limit { get; set; }
        public string Pin { get; set; }
        public List<StringHistoriaKarta> Lista { get => _model.PobierzHistorieKarty(_appInfo.NumerKarty); } //lista z historia operacji na karcie
        #endregion
        #endregion
        public KartaVM(ref Data model, ref AppGlobalInfo appInfo)
        {
            _model = model;
            _appInfo = appInfo;
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
                            NumerKarty = _appInfo.NumerKarty;
                            StringKarta daneKarty = _model.PobierzKarte(NumerKarty);
                            NumerKonta = daneKarty.Konto;
                            Data = daneKarty.Data;
                            Limit = daneKarty.Limit;
                            Pin = daneKarty.Pin;
                            OnPropertyChanged(nameof(UserName), nameof(NumerKarty), nameof(NumerKonta), nameof(Data), nameof(Limit), nameof(Pin));
                            Console.WriteLine($"LP: {Pin}");
                        },
                        arg => true
                    );
                }
                return _onLoad;
            }
        }
        private ICommand _update = null;
        public ICommand Update
        {
            get
            {
                if (_update == null)
                {
                    _update = new RelayCommand(
                        arg =>
                        {
                            _model.AktualizujKarte(NumerKarty, Pin, Convert.ToDouble(Limit));
                            MessageBox.Show(R.cardDataUpdated, R.success, MessageBoxButton.OK, MessageBoxImage.Information);
                        },
                        arg => Limit > 0 && !string.IsNullOrEmpty(Pin) && Pin.Length == 4
                    );
                }
                return _update;
            }
        }
        private ICommand _usunKarte = null;
        public ICommand UsunKarte
        {
            get
            {
                if (_usunKarte == null)
                {
                    _usunKarte = new RelayCommand(
                        arg =>
                        {
                            var result = MessageBox.Show(R.cardDelConfirm, R.attention, MessageBoxButton.YesNo, MessageBoxImage.Warning);
                            if(result == MessageBoxResult.Yes)
                            {
                                _model.UsunKarte(NumerKarty, NumerKonta);
                                Mediator.Notify("GoToPage", "karty");
                            }
                        },
                        arg => Limit > 0 && !string.IsNullOrEmpty(Pin) && Pin.Length == 4
                    );
                }
                return _usunKarte;
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
        public string RLoans { get => R.loans; }
        public string RBack { get => R.back; }
        public string RMyData { get => R.myData; }
        public string RLoginHistory { get => R.loginHistory; }
        public string RNewCard { get => R.newCard; }
        #endregion
        #region cardInfo
        public string RCardNumber { get => R.cardNumber; }
        public string RExpireDate { get => R.expireDate; }
        public string RPaymentLimit { get => R.paymentLimit; }
        public string RAssignedAccount { get => R.assignedAccount; }
        public string RPin { get => R.pin; }
        public string RUpdate { get => R.update; }
        public string RDeleteCard { get => R.deleteCard; }
        #endregion
        #region table
        public string RDate { get => R.date; }
        public string RType { get => R.operationType; }
        public string RAmount { get => R.amount; }
        public string ROpHistry { get => R.operationHistory; }
        #endregion
        #endregion
    }
}
