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
    class KartyVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        #region PUBLIC
        public string UserName { get => _model.WlascicielName; }
        public List<string> ListaKont { get => _model.NumeryKont; }
        public int ListaKontIndex { get; set; }
        //public List<StringKarta> Lista { get => _model.Karty; }
        #endregion
        public KartyVM(ref Data model)
        {
            _model = model;
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
                            OnPropertyChanged(nameof(ListaKontIndex), nameof(UserName), nameof(ListaKont)/*, nameof(Lista)*/);
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

        //private ICommand _splacRate = null;
        //public ICommand SplacRate
        //{
        //    get
        //    {
        //        if (_splacRate == null)
        //        {
        //            _splacRate = new RelayCommand((parameter)
        //                =>
        //            {
        //                int ID = Convert.ToInt32(parameter);
        //                _kredytInfo.Dane = Lista[ID];
        //                _kredytInfo.HasData = true;
        //                Mediator.Notify("GoToPage", "przelew");
        //            },
        //                arg => true
        //            );
        //        }
        //        return _splacRate;
        //    }
        //}
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
