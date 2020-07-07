using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace BankUI.View.Controls
{
    class MoneyTextBox : TextBox
    {
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            string text = this.Text;
            string znak = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            //nie moze pojawic sie minus
            if(e.Text == "-")
            {
                e.Handled = true;
                return;
            }
            //separator nie moze być pierwszy
            if (e.Text == znak && text == "")
            {
                e.Handled = true;
                return;
            }
            //po zero musi byc separator
            if (text == "0" & e.Text != znak)
            {
                e.Handled = true;
                return;
            }
            //nie moze byc dwoch separatorow
            if(e.Text == znak && text.Contains(znak))
            {
                e.Handled = true;
                return;
            }
            //sprawdz czy znak jest liczba
            if (e.Text != znak && !double.TryParse(e.Text, out _))
            {
                e.Handled = true;
                return;
            }

            base.OnPreviewTextInput(e);
        }
    }
}
