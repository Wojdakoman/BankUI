using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace BankUI.ViewModel
{
    class LoginVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        public string Error { get; set; }
        public string LoginName { get; set; }

        public LoginVM(ref Data model) => _model = model;

        private ICommand _login = null;
        public ICommand Login
        {
            get
            {
                if (_login == null)
                {
                    _login = new RelayCommand((parameter)
                        =>
                        {
                            string Pass = (parameter as PasswordBox).Password;
                            //check in model; return true or false
                            if (_model.Login(LoginName, Pass))
                            {
                                //login successfull
                                Mediator.Notify("GoToPage", "panelGlowny");
                            }
                            else
                            {
                                //login failed
                                Error = "Błędny login lub hasło!";
                                OnPropertyChanged(nameof(Error));
                            }

                        },
                        arg => true
                    );
                }
                return _login;
            }
        }

        private ICommand _stronaRejestracji = null;
        public ICommand StronaRejestracji
        {
            get
            {
                if(_stronaRejestracji == null)
                {
                    _stronaRejestracji = new RelayCommand(
                        arg =>
                        {
                            Mediator.Notify("GoToPage", "rejestracja");
                        },
                        arg => true
                        );
                }
                return _stronaRejestracji;
            }
        }
    }
}
