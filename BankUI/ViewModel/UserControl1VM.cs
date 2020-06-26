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
    class UserControl1VM : ViewModelBase, IPageViewModel
    {
        /*public List<Osoba> Lista { get; set; }

        public UserControl1VM()
        {
            Lista = new List<Osoba>();
            Lista.Add(new Osoba("14.06.2020", "Politechnika", "Na pewno nie stypendium", "Przelew środków", "+145,85 PLN"));
            Lista.Add(new Osoba("14.06.2020", "Politechnika", "Na pewno nie stypendium", "Przelew środków", "+145,85 PLN"));
            Lista.Add(new Osoba("14.06.2020", "Politechnika", "Na pewno nie stypendium", "Przelew środków", "+145,85 PLN"));
            Lista.Add(new Osoba("14.06.2020", "Politechnika", "Na pewno nie stypendium", "Przelew środków", "+145,85 PLN"));
            Lista.Add(new Osoba("14.06.2020", "Politechnika", "Na pewno nie stypendium", "Przelew środków", "+145,85 PLN"));
            Lista.Add(new Osoba("14.06.2020", "Politechnika", "Na pewno nie stypendium", "Przelew środków", "+145,85 PLN"));
            Lista.Add(new Osoba("14.06.2020", "Politechnika", "Na pewno nie stypendium", "Przelew środków", "+145,85 PLN"));
            Lista.Add(new Osoba("14.06.2020", "Politechnika", "Na pewno nie stypendium", "Przelew środków", "+145,85 PLN"));
        }*/

        private ICommand _goTo2 = null;
        public ICommand GoTo2
        {
            get
            {
                if (_goTo2 == null)
                {
                    _goTo2 = new RelayCommand(
                        arg =>
                        {
                            Mediator.Notify("GoTo2Screen", "");
                        },
                        arg => true
                    );
                }
                return _goTo2;
            }
        }
    }
}
