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
    class DaneOsoboweVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        private WlascicielDane _dane { get => _model.DaneWlasciciela; }

        #region PUBLIC
        public string UserName { get => _model.WlascicielName; }
        #endregion
        public DaneOsoboweVM(ref Data model) => _model = model;

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
                            OnPropertyChanged(nameof(UserName));
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
