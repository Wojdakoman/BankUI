using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Classes;
using BankUI.ViewModel.Interfaces;
using Projekt.DAL.Entity;

namespace BankUI.ViewModel
{
    /// <summary>
    /// Glowna klasa ViewModelu polaczona z MainWindow.xaml
    /// </summary>
    class MainWindowVM : ViewModelBase
    {
        #region PRIVATE
        private IPageViewModel _currentPageViewModel; //aktualnie wyswietlana strona
        private Dictionary<string, IPageViewModel> _pageViewModels; //lista dostepnych stron
        private Data _model;
        private AppGlobalInfo _appInfo; //klasa odpowiedzialna za przesylanie danych pomiedzy oknami
        private KartaPlatnicza _kartaPlatnicza;
        #endregion

        #region PUBLIC
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
        #endregion

        #region EventsMethods
        /// <summary>
        /// Metody wywolywane przez zdarzenia Mediatora
        /// </summary>

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
            _kartaPlatnicza = new KartaPlatnicza();
            //sprawdza polacznie z baza danych
            if (!_model.PolaczeniePoprawne())
            {
                MessageBox.Show(Properties.Resources.DBerror, Properties.Resources.attention, MessageBoxButton.OK, MessageBoxImage.Error);
                Application.Current.Shutdown(); //zamknij aplikacje, dgy nie można się połaczyć z bazą danych
            }
            //dodanie dostepnych stron do listy
            /*
             * Aby strony wyswietlaly sie poprawnie nalezy rowniez dodac odpowiednie dane w pliku App.xaml
             * Schemat:
             * <DataTemplate DataType="{x:Type vm:KlasaVM(.cs)}">
             *  <okna:Okno(.xaml) />
             * </DataTemplate>
             */
            PageViewModels.Add("login", new LoginVM(ref _model));
            PageViewModels.Add("panelGlowny", new PanelGlownyVM(ref _model));
            PageViewModels.Add("przelew", new PrzelewVM(ref _model, ref _appInfo));
            PageViewModels.Add("rejestracja", new RejestracjaVM(ref _model));
            PageViewModels.Add("historiaLogowan", new HistoriaLogowanVM(ref _model));
            PageViewModels.Add("daneOsobowe", new DaneOsoboweVM(ref _model));
            PageViewModels.Add("kredyty", new KredytVM(ref _model, ref _appInfo));
            PageViewModels.Add("nowyKredyt", new NowyKredytVM(ref _model));
            PageViewModels.Add("karty", new KartyVM(ref _model, ref _appInfo));
            PageViewModels.Add("lBankomat", new LBankomatVM(ref _kartaPlatnicza));
            PageViewModels.Add("pokazKarte", new KartaVM(ref _model, ref _appInfo));
            PageViewModels.Add("bankomat", new BankomatVM(ref _kartaPlatnicza));
            //ustawienei strony startowej
            CurrentPageViewModel = PageViewModels["login"];
            //subskrybcja nasluchiwania zdarzen
            Mediator.Subscribe("GoToPage", ZmianaWidoku);
            Mediator.Subscribe("Wyloguj", Wyloguj);
        }
    }
}
