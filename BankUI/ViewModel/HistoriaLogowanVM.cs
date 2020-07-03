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
    class HistoriaLogowanVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        public string UserName { get => _model.WlascicielName; }
        public List<StringHistoriaLogowan> Lista { get => _model.HistoriaLogowan; }

        public HistoriaLogowanVM(ref Data model) => _model = model;

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
                            OnPropertyChanged(nameof(UserName), nameof(Lista));
                        },
                        arg => true
                    );
                }
                return _onLoad;
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
        #endregion
        #endregion
    }
}
