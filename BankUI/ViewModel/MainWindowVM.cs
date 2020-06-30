using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Interfaces;

namespace BankUI.ViewModel
{
    class MainWindowVM : ViewModelBase
    {
        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;
        private Data _model;

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

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

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
                PageViewModels.Add(viewModel);

            CurrentPageViewModel = PageViewModels
                .FirstOrDefault(vm => vm == viewModel);
        }

        #region ZmianaWidoku
        private void OnGo1Screen(object obj)
        {
            Console.WriteLine(obj.ToString());
            ChangeViewModel(PageViewModels[1]);
        }

        private void OnGo2Screen(object obj)
        {
            Console.WriteLine(obj.ToString());
            ChangeViewModel(PageViewModels[2]);
        }

        private void OnLogin(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
        }

        private void NowyPrzelew(object obj)
        {
            ChangeViewModel(PageViewModels[2]);
        }

        private void ToPanelGlowny(object obj)
        {
            ChangeViewModel(PageViewModels[1]);
        }
        #endregion
        public MainWindowVM()
        {
            _model = new Data();
            // Add available pages and set page
            PageViewModels.Add(new LoginVM(ref _model));
            PageViewModels.Add(new PanelGlownyVM(ref _model));
            PageViewModels.Add(new PrzelewVM(ref _model));
            PageViewModels.Add(new UserControl1VM());
            PageViewModels.Add(new UserControl2VM());

            CurrentPageViewModel = PageViewModels[0];

            Mediator.Subscribe("Zalogowano", OnLogin);
            Mediator.Subscribe("PanelGlowny", ToPanelGlowny);
            Mediator.Subscribe("NowyPrzelew", NowyPrzelew);
            Mediator.Subscribe("GoTo1Screen", OnGo1Screen);
            Mediator.Subscribe("GoTo2Screen", OnGo2Screen);
        }
    }
}
