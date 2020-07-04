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
    class PrzelewVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        private AppGlobalInfo _kredytInfo;
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
                            if (_kredytInfo.HasData)
                            {
                                Odbiorca = _kredytInfo.DaneKredyt.NumerKonta;
                                Tytul = "Spłata raty";
                                Wartosc = _kredytInfo.DaneKredyt.Rata;
                                _kredytInfo.HasData = false;
                            }
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
                            _model.NowyPrzelew(Odbiorca, (double)Wartosc, Tytul, Opis);
                            Clear();
                            OnPropertyChanged(nameof(Saldo), nameof(SaldoString));
                            MessageBox.Show("Wykonano przelew");
                        },
                        arg => !(string.IsNullOrEmpty(Odbiorca) && string.IsNullOrEmpty(Tytul)) && Wartosc > 0 && Wartosc <= Saldo && _model.NumerIstnieje(Odbiorca)
                    );
                }
                return _wykonajPrzelew;
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
    }
}
