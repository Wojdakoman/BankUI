using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Interfaces;
using Renci.SshNet.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BankUI.ViewModel
{
    class BankomatVM : ViewModelBase, IPageViewModel
    {
        private Data _model;
        private ICommand wykonaj;


        public string Wybrany { get; set; }
        public string Typ { get; set; }
        
        public ICommand Wykonaj
        {
            get
            {
                if (wykonaj == null)
                {
                    wykonaj = new RelayCommand(
                       arg =>
                       {
                           MessageBox.Show(Wybrany);
                           MessageBox.Show(Typ);
                       },
                        arg => true
                    );
                }
                return wykonaj;
            }
        }
        public BankomatVM(ref Data model)
        {
            _model = model;
        }
    }
}
