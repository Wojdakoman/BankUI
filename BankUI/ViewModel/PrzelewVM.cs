using BankUI.Model;
using BankUI.View;
using BankUI.View.Controls;
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
    class PrzelewVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        private AppGlobalInfo _kredytInfo; //wykorzystuje klase do odebrania danych o przlewie
        private bool _sprawdzKredyt = false; //warunek, czy sprawdzic czy kredyt zostal splacony 
        #region PUBLIC
        public string UserName { get => _model.WlascicielName; }
        public List<string> ListaKont { get => _model.NumeryKont; }
        public int ListaKontIndex { get; set; }
        public double Saldo { get => _model.Saldo; }
        public string SaldoString { get => $"{_model.Saldo} PLN"; }
        #region DanePrzelewu
        public string Odbiorca { get; set; }
        public string Tytul { get; set; }
        public string Opis { get; set; }
        public double? Wartosc { get; set; }
        #endregion
        #endregion
        public PrzelewVM(ref Data model, ref AppGlobalInfo kredyt)
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
                            //uzupelnienie danych przelewu, jezeli sa przeslane
                            if (_kredytInfo.HasData)
                            {
                                Odbiorca = _kredytInfo.DaneKredyt.NumerKonta;
                                Tytul = R.loanPaymentTitle;
                                Wartosc = _kredytInfo.DaneKredyt.Rata;
                                _kredytInfo.HasData = false;
                                _sprawdzKredyt = true;
                            }
                            //inaczej nalezy wyczyscic pola, gdyz moga zawierac dane ze starego przelewu
                            else Clear();
                            OnPropertyChanged(nameof(ListaKontIndex), nameof(UserName), nameof(ListaKont), nameof(Saldo), nameof(SaldoString), nameof(Odbiorca), nameof(Tytul), nameof(Wartosc));
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
                            OnPropertyChanged(nameof(ListaKont), nameof(Saldo), nameof(SaldoString));
                        },
                        arg => true
                    );
                }
                return _zmienKonto;
            }
        }

        private ICommand _wykonajPrzelew = null;
        public ICommand WykonajPrzelew
        {
            get
            {
                if (_wykonajPrzelew == null)
                {
                    _wykonajPrzelew = new RelayCommand(
                        arg =>
                        {
                            _model.NowyPrzelew(Odbiorca, Convert.ToDouble(Wartosc), Tytul, Opis);
                            Clear();
                            OnPropertyChanged(nameof(Saldo), nameof(SaldoString));
                            MessageBox.Show(R.transferSuccess);
                            if (_sprawdzKredyt) //sprawdza, czy wukonanie przelwu splacilo kredyt
                            {
                                if (_kredytInfo.DaneKredyt.Splacono + _kredytInfo.DaneKredyt.Rata >= _kredytInfo.DaneKredyt.Koszt)
                                {
                                    _model.ZamknijKredyt(_kredytInfo.DaneKredyt.IDKredytu, _kredytInfo.DaneKredyt.NumerKonta);
                                    MessageBox.Show(R.loanPaid, R.success, MessageBoxButton.OK, MessageBoxImage.Information);
                                }
                                _sprawdzKredyt = false;
                            }
                        },
                        arg => !(string.IsNullOrEmpty(Odbiorca) && string.IsNullOrEmpty(Tytul)) && Wartosc > 0 && Wartosc <= Saldo && _model.NumerIstnieje(Odbiorca)
                    );
                }
                return _wykonajPrzelew;
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

        private void Clear()
        {
            Odbiorca = "";
            Tytul = "";
            Opis = "";
            Wartosc = null;
            OnPropertyChanged(nameof(Odbiorca), nameof(Tytul), nameof(Opis), nameof(Wartosc));
        }

        #region Zasoby
        //Zawiera odwołania do zasobów aplikacji, aby pobrać odpowiednią wersję językową dla kontorlek
        #region menu
        public string RActiveAccount { get => R.activeAccount; }
        public string RLogout { get => R.logout; }
        public string RAccount { get => R.account; }
        public string RLoans { get => R.loans; }
        public string RCards { get => R.cards; }
        public string RMyData { get => R.myData; }
        public string RLoginHistory { get => R.loginHistory; }
        #endregion
        public string RFromAccount { get => R.fromAccount; }
        public string RRecipent { get => R.recipient; }
        public string RTitle { get => R.title; }
        public string RDesc { get => R.description; }
        public string RAmount { get => R.amount; }
        public string RExec { get => R.execute; }
        #endregion
    }
}
