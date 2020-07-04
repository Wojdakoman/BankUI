﻿using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Classes;
using BankUI.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BankUI.ViewModel
{
    class PanelGlownyVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        public string UserName { get => _model.WlascicielName; }
        public List<string> ListaKont { get => _model.NumeryKont; }
        public int ListaKontIndex { get; set; } = 0;
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
                            OnPropertyChanged(nameof(UserName), nameof(ListaKont), nameof(Saldo), nameof(TypKonta), nameof(Lista));
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
        #region goTo
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
        private ICommand _goKarty = null;
        public ICommand GoKarty
        {
            get
            {
                if (_goKarty == null)
                {
                    _goKarty = new RelayCommand(
                        arg =>
                        {
                            Mediator.Notify("GoToPage", "karty");
                        },
                        arg => true
                    );
                }
                return _goKarty;
            }
        }
        private ICommand _goKredyty = null;
        public ICommand GoKredyty
        {
            get
            {
                if (_goKredyty == null)
                {
                    _goKredyty = new RelayCommand(
                        arg =>
                        {
                            Mediator.Notify("GoToPage", "kredyt");
                        },
                        arg => true
                    );
                }
                return _goKredyty;
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
        #endregion
        #endregion
    }
}
