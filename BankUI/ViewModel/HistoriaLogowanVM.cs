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
