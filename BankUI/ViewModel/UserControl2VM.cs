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
    class UserControl2VM : ViewModelBase, IPageViewModel
    {
        private ICommand _goTo1 = null;
        public ICommand GoTo1
        {
            get
            {
                if (_goTo1 == null)
                {
                    _goTo1 = new RelayCommand(
                        arg =>
                        {
                            Mediator.Notify("GoTo1Screen", "");
                        },
                        arg => true
                    );
                }
                return _goTo1;
            }
        }
    }
}
