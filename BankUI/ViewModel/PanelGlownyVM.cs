using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Classes;
using BankUI.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BankUI.ViewModel
{
    using R = Properties.Resources;
    class PanelGlownyVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        public string UserName { get => _model.WlascicielName; }
        public List<string> ListaKont { get => _model.NumeryKont; }
        public int ListaKontIndex { get; set; }
        public string Saldo { get => $"{_model.Saldo} PLN"; }
        public string TypKonta { get => $"Konto {_model.TypKonta}"; }
        public List<StringHistoria> Lista { get => _model.Historia; }

        public PanelGlownyVM(ref Data model) => _model = model;

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
                            OnPropertyChanged(nameof(ListaKontIndex), nameof(UserName), nameof(ListaKont), nameof(Saldo), nameof(TypKonta), nameof(Lista));
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
                            OnPropertyChanged(nameof(ListaKontIndex), nameof(ListaKont), nameof(Saldo), nameof(TypKonta), nameof(Lista));
                        },
                        arg => true
                    );
                }
                return _zmienKonto;
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
        private ICommand _otworzKonto = null;
        public ICommand OtworzKonto
        {
            get
            {
                if (_otworzKonto == null)
                {
                    _otworzKonto = new RelayCommand(
                        arg =>
                        {
                            var result = MessageBox.Show("Na pewno chcesz otworzyć nowe konto?", "Uwaga!", MessageBoxButton.YesNo, MessageBoxImage.Question);
                            if(result == MessageBoxResult.Yes)
                            {
                                _model.OtworzKonto();
                                MessageBox.Show("Pomyślnie otwarto nowe konto!", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                                OnPropertyChanged(nameof(ListaKontIndex), nameof(ListaKont));
                            }
                        },
                        arg => true
                    );
                }
                return _otworzKonto;
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
        public string RCards { get => R.cards; }
        public string RMyData { get => R.myData; }
        public string RLoginHistory { get => R.loginHistory; }
        public string RNewAccount { get => R.newAccount; }
        public string RDeleteAccount { get => R.deleteAccount; }
        #endregion
        public string RBalance { get => R.balance; }
        #region table
        public string RDate { get => R.date; }
        public string RSenderRec { get => $"{R.sender}/{R.recipient}"; }
        public string RTitle { get => R.title; }
        public string ROpType { get => R.operationType; }
        public string RAmount { get => R.amount; }
        #endregion
        #endregion
    }
}
