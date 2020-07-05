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
    class KartyVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        private AppGlobalInfo _appInfo;
        #region PUBLIC
        public string UserName { get => _model.WlascicielName; }
        public List<string> ListaKont { get => _model.NumeryKont; }
        public int ListaKontIndex { get; set; }
        public List<StringKarta> Lista { get => _model.Karty; }
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
                        _appInfo.NumerKarty = numerkarty;
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
                            MessageBox.Show("Dodano nową kartę do konta", "Sukces", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
