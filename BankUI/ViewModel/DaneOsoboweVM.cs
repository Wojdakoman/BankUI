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
    class DaneOsoboweVM : ViewModelBase, IPageViewModel
    {
        #region PRIVATE
        private Data _model;
        private WlascicielDane _dane { get => _model.DaneWlasciciela; }
        private string _haslo;
        #endregion

        #region PUBLIC
        public string UserName { get => _model.WlascicielName; }
        #region Dane
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public string Miasto { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public string Login { get; set; }
        public string Haslo
        {
            get
            {
                return _haslo;
            }
            set
            {
                _haslo = value;
                OnPropertyChanged(nameof(Haslo));
            }
        }
        #endregion
        #endregion
        public DaneOsoboweVM(ref Data model) => _model = model;

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
                            Imie = _dane.Imie;
                            Nazwisko = _dane.Nazwisko;
                            Miasto = _dane.Miasto;
                            Adres = _dane.Adres;
                            Telefon = _dane.Telefon;
                            Login = _dane.Login;
                            _haslo = _dane.Haslo;
                            OnPropertyChanged(nameof(UserName), nameof(Imie), nameof(Nazwisko), nameof(Miasto), nameof(Adres), nameof(Telefon), nameof(Login), nameof(Haslo));
                        },
                        arg => true
                    );
                }
                return _onLoad;
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
                            if (Login != _dane.Login & _model.LoginIstnieje(Login)) MessageBox.Show(R.loginOccupied, R.attention, MessageBoxButton.OK, MessageBoxImage.Warning);
                            else
                            {
                                _model.AktualizujDaneOsobowe(new WlascicielDane(Imie, Nazwisko, Miasto, Adres, Telefon, Login, Haslo));
                                OnPropertyChanged(nameof(UserName));
                                MessageBox.Show(R.dataUpdated, R.success, MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        },
                        arg => (Imie != null && Nazwisko != null && Miasto != null && Adres != null && Telefon != null && Login != null && Haslo != null)
                    );
                }
                return _update;
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
        public string RAccount { get => R.account; }
        public string RLoginHistory { get => R.loginHistory; }
        #endregion
        #region dane
        public string RName { get => R.name; }
        public string RSurname { get => R.surname; }
        public string RCity { get => R.city; }
        public string RAddres { get => R.addres; }
        public string RTel { get => R.telephone; }
        public string RLogin { get => R.login; }
        public string RPassword { get => R.password; }
        public string RUpdate { get => R.update; }
        #endregion
        #endregion
    }
}
