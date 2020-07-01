using BankUI.Model;
using BankUI.ViewModel.Base;
using BankUI.ViewModel.Interfaces;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BankUI.ViewModel
{
    class RejestracjaVM : ViewModelBase, IPageViewModel
    {

        #region Private Methods
        private Data _model;

        private string pesel;

        #endregion

        #region Public Properties
        public ICommand Powrot { get; set; }
        public ICommand Zarejestruj { get; set; }

        public string Pesel
        {
            get
            {
                return pesel;
            }
            set
            {
                OnPropertyChanged(nameof(Pesel));
                pesel = value;
                //Sprawdz istnienie wpisanego Pesla
                MessageBox.Show(pesel.ToString());
                CheckPesel(pesel);
            }
        }

        #endregion


        #region Contructors
        public RejestracjaVM(ref Data model)
        {
            _model = model;
        }
        #endregion
        #region Methods

        private void CheckPesel(string pesel)
        {
            MessageBox.Show(pesel.ToString());
        }

        #endregion
    }
}
