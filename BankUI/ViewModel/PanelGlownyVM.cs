using BankUI.Model;
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
        #endregion
    }
}
