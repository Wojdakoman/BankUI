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
    class BankomatVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        private ICommand wykonaj;




        public BankomatVM(ref Data model)
        {
            _model = model;
        }
    }
}
