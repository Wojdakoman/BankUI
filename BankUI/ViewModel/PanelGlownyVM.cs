using BankUI.Model;
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
    class PanelGlownyVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        public string UserName { get; set; }
        public List<string> ListaKont { get; set; }
        public int ListaKontIndex { get; set; } = 0;
        public string Saldo { get; set; }

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
                            UserName = _model.WlascicielName;
                            ListaKont = _model.NumeryKont;
                            Saldo = $"{_model.Saldo} PLN";
                            OnPropertyChanged(nameof(UserName), nameof(ListaKont), nameof(Saldo));
                        },
                        arg => true
                    );
                }
                return _onLoad;
            }
        }
        #endregion
    }
}
