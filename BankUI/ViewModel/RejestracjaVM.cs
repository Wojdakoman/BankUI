using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Interfaces;
using Google.Protobuf.WellKnownTypes;
using Projekt.Class;
using Projekt.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BankUI.ViewModel
{
    using R = Properties.Resources;
    class RejestracjaVM : ViewModelBase, IPageViewModel
    {

        #region Private Methods
        private Data _model;
        private string haslo;
        private ICommand powrot;
        private ICommand zarejestruj;
        #endregion
        #region Public Properties
        public string Pesel { get; set; }
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
        public DateTime Data { get; set; } = DateTime.Now.AddYears(-13);
        public DateTime DataKoncowa { get; set; } = DateTime.Now.AddYears(-13);
        public DateTime DataPoczatkowa { get; set; } = DateTime.Now.AddYears(-120);
        public string Miasto { get; set; }
        public string Adres { get; set; }
        public string Telefon { get; set; }
        public string Login { get; set; }
        public string Haslo
        {
            get
            {
                return haslo;
            }
            set
            {
                haslo = value;
                OnPropertyChanged(nameof(Haslo));
            }
        }
        #endregion
        #region Commands
        public ICommand Zarejestruj
        {
            get
            {
                if (zarejestruj == null)
                {
                    zarejestruj = new RelayCommand((parameter)
                        =>
                    {
                    Haslo = (parameter as PasswordBox).Password;
                        //check in model; return true or false
                        if (!(RepositoryWlasciciel.DoesLoginExist(Login) || RepositoryWlasciciel.DoesPeselExist(Int64.Parse(Pesel))))
                        {
                            //Mlodziezowe do 18 roku zycia
                            string typ = (DateTime.Now - Data).Days / 365 <= 18 ? "Mlodziezowe" : "Normalne";
                            NewOwner nowaOsoba = new NewOwner(typ, Pesel, Imie, Nazwisko, Data.ToShortDateString(), Miasto, Adres, Telefon, Login, Haslo);
                            Task.Delay(1000);

                            //Zalogowanie nowego klienta
                            _model.Login(Login, Haslo);
                            Mediator.Notify("GoToPage", "panelGlowny");
                        }

                    },
                        arg => (Pesel != null && Imie != null && Nazwisko != null && Data != null && Miasto != null && Adres != null && Telefon != null && Login != null && Haslo != null)
                    );
                }
                return zarejestruj;
            }
        }

        public ICommand Powrot
        {
            get
            {
                if (powrot == null)
                {
                    powrot = new RelayCommand(
                        arg =>
                        {
                            Mediator.Notify("GoToPage", "login");
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
        #region Contructors
        public RejestracjaVM(ref Data model)
        {
            _model = model;
        }
        #endregion
        #region Zasoby
        //Zawiera odwołania do zasobów aplikacji, aby pobrać odpowiednią wersję językową dla kontorlek
        #region menu
        public string RBack { get => R.back; }
        public string RRegister { get => R.signUp; }
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
        public string RPESEL { get => R.PESEL; }
        public string RBirthDate { get => R.birthDate; }
        #endregion
        #endregion
    }
}
