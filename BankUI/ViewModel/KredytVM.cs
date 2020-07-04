using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Classes;
using BankUI.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BankUI.ViewModel
{
    class KredytVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        private KredytPrzelewInfo _kredytInfo;
        #region PUBLIC
        public string UserName { get => _model.WlascicielName; }
        public List<string> ListaKont { get => _model.NumeryKont; }
        public int ListaKontIndex { get; set; }
        public List<StringKredyt> Lista { get => _model.Kredyty; }
        #endregion
        public KredytVM(ref Data model, ref KredytPrzelewInfo kredyt)
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
                            OnPropertyChanged(nameof(ListaKont));
                        },
                        arg => true
                    );
                }
                return _zmienKonto;
            }
        }

        private ICommand _splacRate = null;
        public ICommand SplacRate
        {
            get
            {
                if (_splacRate == null)
                {
                    _splacRate = new RelayCommand((parameter)
                        =>
                    {
                        int ID = Convert.ToInt32(parameter);
                        _kredytInfo.Dane = Lista[ID];
                        _kredytInfo.HasData = true;
                        Mediator.Notify("GoToPage", "przelew");
                    },
                        arg => true
                    );
                }
                return _splacRate;
            }
        }
        #region goTo
        private ICommand _goMain = null;
        public ICommand GoMain
        {
            get
            {
                if (_goMain == null)
                {
                    _goMain = new RelayCommand(
                        arg =>
                        {
                            Mediator.Notify("GoToPage", "panelGlowny");
                        },
                        arg => true
                    );
                }
                return _goMain;
            }
        }
        private ICommand _goHistoriaLogowan = null;
        public ICommand GoHistoriaLogowan
        {
            get
            {
                if (_goHistoriaLogowan == null)
                {
                    _goHistoriaLogowan = new RelayCommand(
                        arg =>
                        {
                            Mediator.Notify("GoToPage", "historiaLogowan");
                        },
                        arg => true
                    );
                }
                return _goHistoriaLogowan;
            }
        }
        private ICommand _goDaneOsobowe = null;
        public ICommand GoDaneOsobowe
        {
            get
            {
                if (_goDaneOsobowe == null)
                {
                    _goDaneOsobowe = new RelayCommand(
                        arg =>
                        {
                            Mediator.Notify("GoToPage", "daneOsobowe");
                        },
                        arg => true
                    );
                }
                return _goDaneOsobowe;
            }
        }
        private ICommand _goPrzelewy = null;
        public ICommand GoPrzelewy
        {
            get
            {
                if (_goPrzelewy == null)
                {
                    _goPrzelewy = new RelayCommand(
                        arg =>
                        {
                            Mediator.Notify("GoToPage", "przelew");
                        },
                        arg => true
                    );
                }
                return _goPrzelewy;
            }
        }
        #endregion
        #endregion
    }
}
