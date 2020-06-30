﻿using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BankUI.ViewModel
{
    class PrzelewVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
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
        public string Wartosc { get; set; } //potrzebny converter
        #endregion
        #endregion
        public PrzelewVM(ref Data model) => _model = model;

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
                            OnPropertyChanged(nameof(ListaKontIndex), nameof(UserName), nameof(ListaKont), nameof(Saldo), nameof(SaldoString));
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
        #endregion
    }
}