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
    class LBankomatVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        private string numerKarty;
        public string Pin { get; set; }
        public ICommand Zaloguj { get; set; }
        public ICommand Powrot { get; set; }

        public string NumerKarty
        {
            get
            {
                return numerKarty;
            }
            set
            {
                numerKarty = value;
                OnPropertyChanged(nameof(NumerKarty));
            }
        }

        public LBankomatVM(ref Data model)
        {
            _model = model;
        }
    }
}
