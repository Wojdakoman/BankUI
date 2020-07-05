using BankUI.Model;
using BankUI.ViewModel.Base;
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
    class NowyKredytVM : ViewModelBase, IPageViewModel
    {
        #region PRIVATE
        private Data _model;
        private int _wartosc = 100;
        private int _miesiecy = 3;
        #endregion
        #region PUBLIC
        public string UserName { get => _model.WlascicielName; }
        public List<string> ListaKont { get => _model.NumeryKont; }
        public int ListaKontIndex { get; set; }
        #region Kredyt info
        public string Rata { get => $"{Math.Round((_wartosc / _miesiecy) + ((_wartosc / _miesiecy) * 0.15), 2)} PLN"; }
        public string Oprocentowanie { get => "15%"; }
        public int Wartosc { get => _wartosc; set {
                _wartosc = value;
                OnPropertyChanged(nameof(Rata), nameof(Oprocentowanie));
            } }
        public int Miesiecy { get => _miesiecy; set {
                _miesiecy = value;
                OnPropertyChanged(nameof(Rata), nameof(Oprocentowanie));
            } }
        #endregion
        #endregion
        public NowyKredytVM(ref Data model) => _model = model;

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
                            OnPropertyChanged(nameof(ListaKontIndex), nameof(UserName), nameof(ListaKont));
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

        private ICommand _wezKredyt = null;
        public ICommand WezKredyt
        {
            get
            {
                if (_wezKredyt == null)
                {
                    _wezKredyt = new RelayCommand(
                        arg =>
                        {
                            _model.WezKredyt(Wartosc, Miesiecy);
                            MessageBox.Show("Pomyślnie zaciagnieto kredyt", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
                            Mediator.Notify("GoToPage", "kredyty");
                        },
                        arg => true
                    );
                }
                return _wezKredyt;
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
        public string RCards { get => R.cards; }
        public string RMyData { get => R.myData; }
        public string RLoginHistory { get => R.loginHistory; }
        public string RTakeLoan { get => R.takeLoan; }
        #endregion
        public string RLoanValue { get => $"{R.loanValue}:"; }
        public string RCountMonths { get => $"{R.countMonths}:"; }
        public string RInterest { get => $"{R.interest}:"; }
        public string RInstallment { get => $"{R.installment}:"; }
        #endregion
    }
}
