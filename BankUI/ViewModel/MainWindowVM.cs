using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Classes;
using BankUI.ViewModel.Interfaces;

namespace BankUI.ViewModel
{
    class MainWindowVM : ViewModelBase
    {
        private IPageViewModel _currentPageViewModel;
        private Dictionary<string, IPageViewModel> _pageViewModels;
        private Data _model;
        private AppGlobalInfo _appInfo;

        public Dictionary<string, IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new Dictionary<string, IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get
            {
                return _currentPageViewModel;
            }
            set
            {
                _currentPageViewModel = value;
                OnPropertyChanged(nameof(CurrentPageViewModel));
            }
        }

        #region EventsMethods
        private void ZmianaWidoku(object obj)
        {
            string pageName = obj.ToString();
            if(PageViewModels.ContainsKey(pageName))
                CurrentPageViewModel = PageViewModels[obj.ToString()];
            else CurrentPageViewModel = PageViewModels["panelGlowny"];
        }
        private void Wyloguj(object obj)
        {
            //tworzy nowa, "czysta" instancje modelu
            _model = new Data();
            CurrentPageViewModel = PageViewModels["login"];
        }
        #endregion
        public MainWindowVM()
        {
            _model = new Data();
            _appInfo = new AppGlobalInfo();
            // Add available pages and set page
            PageViewModels.Add("login", new LoginVM(ref _model));
            PageViewModels.Add("panelGlowny", new PanelGlownyVM(ref _model));
            PageViewModels.Add("przelew", new PrzelewVM(ref _model, ref _appInfo));
            PageViewModels.Add("rejestracja", new RejestracjaVM(ref _model));
            PageViewModels.Add("historiaLogowan", new HistoriaLogowanVM(ref _model));
            PageViewModels.Add("daneOsobowe", new DaneOsoboweVM(ref _model));
            PageViewModels.Add("kredyty", new KredytVM(ref _model, ref _appInfo));
            PageViewModels.Add("karty", new KartyVM(ref _model, ref _appInfo));
            PageViewModels.Add("lBankomat", new LBankomatVM(ref _model));
            PageViewModels.Add("pokazKarte", new KartaVM(ref _model, ref _appInfo));
            PageViewModels.Add("bankomat", new BankomatVM(ref _model));

            CurrentPageViewModel = PageViewModels["login"];

            Mediator.Subscribe("GoToPage", ZmianaWidoku);
            Mediator.Subscribe("Wyloguj", Wyloguj);
        }
    }
}
