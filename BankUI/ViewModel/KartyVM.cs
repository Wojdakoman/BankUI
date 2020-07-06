using BankUI.Model;
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
    class KartyVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        private AppGlobalInfo _appInfo; //wykorzystuje klase do przekazania numeru karty, ktorej szczegoly chcemy zobaczyc
        #region PUBLIC
        public string UserName { get => _model.WlascicielName; }
        public List<string> ListaKont { get => _model.NumeryKont; }
        public int ListaKontIndex { get; set; }
        public List<StringKarta> Lista { get => _model.Karty; } //lista kart przypisanych do konta
        #endregion
        public KartyVM(ref Data model, ref AppGlobalInfo appInfo)
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
                            OnPropertyChanged(nameof(ListaKont), nameof(Lista));
                        },
                        arg => true
                    );
                }
                return _zmienKonto;
            }
        }

        private ICommand _pokazKarte = null;
        public ICommand PokazKarte
        {
            get
            {
                if (_pokazKarte == null)
                {
                    _pokazKarte = new RelayCommand((parameter)
                        =>
                    {
                        string numerkarty = parameter.ToString();
                        _appInfo.NumerKarty = numerkarty; //zapisuje numer karty w klasie AppGlobalInfo
                        Mediator.Notify("GoToPage", "pokazKarte");
                    },
                        arg => true
                    );
                }
                return _pokazKarte;
            }
        }
        private ICommand _nowaKarta = null;
        public ICommand NowaKarta
        {
            get
            {
                if (_nowaKarta == null)
                {
                    _nowaKarta = new RelayCommand(
                        arg =>
                        {
                            _model.DodajKarte();
                            OnPropertyChanged(nameof(Lista));
                            MessageBox.Show(R.cardAdded, R.success, MessageBoxButton.OK, MessageBoxImage.Information);
                        },
                        arg => true
                    );
                }
                return _nowaKarta;
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
        public string RAccount { get => R.account; }
        public string RMyData { get => R.myData; }
        public string RLoginHistory { get => R.loginHistory; }
        public string RNewCard { get => R.newCard; }
        #endregion
        #region table
        public string RCardNumber { get => R.cardNumber; }
        public string RExpireDate { get => R.expireDate; }
        public string RPaymentLimit { get => R.paymentLimit; }
        public string RDetails { get => R.details; }
        #endregion
        #endregion
    }
}
