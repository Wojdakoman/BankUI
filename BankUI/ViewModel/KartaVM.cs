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
    class KartaVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        private AppGlobalInfo _appInfo;
        #region PUBLIC
        public string UserName { get => _model.WlascicielName; }
        #region DaneKarty
        public string NumerKarty { get; set; }
        public string NumerKonta { get; set; }
        public string Data { get; set; }
        public string Limit { get; set; }
        public string Pin { get; set; }
        #endregion
        #endregion
        public KartaVM(ref Data model, ref AppGlobalInfo appInfo)
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
                            NumerKarty = _appInfo.NumerKarty;
                            StringKarta daneKarty = _model.PobierzKarte(NumerKarty);
                            NumerKonta = daneKarty.Konto;
                            Data = daneKarty.Data;
                            Limit = daneKarty.Limit;
                            Pin = "1234";
                            OnPropertyChanged(nameof(UserName), nameof(NumerKarty), nameof(NumerKonta), nameof(Data), nameof(Limit), nameof(Pin));
                        },
                        arg => true
                    );
                }
                return _onLoad;
            }
        }

        //private ICommand _pokazKarte = null;
        //public ICommand PokazKarte
        //{
        //    get
        //    {
        //        if (_pokazKarte == null)
        //        {
        //            _pokazKarte = new RelayCommand((parameter)
        //                =>
        //            {
        //                string numerkarty = parameter.ToString();
        //                _appInfo.NumerKarty = numerkarty;
        //                Mediator.Notify("GoToPage", "pokazKarte");
        //            },
        //                arg => true
        //            );
        //        }
        //        return _pokazKarte;
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
