using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BankUI.AttachedProperties
{
    public static class RadioButtonBankomat
    {
        public static readonly DependencyProperty RadioProperty =
            DependencyProperty.RegisterAttached(
                "Radio",
                typeof(string),
                typeof(RadioButtonBankomat),
                new PropertyMetadata(string.Empty, RadioButtonChecked));

        public static void SetRadio(RadioButton radioButton, string value)
        {
            radioButton.SetValue(RadioProperty, value);
        }
        public static string GetRadio(RadioButton radioButton)
        {
            return (string)radioButton.GetValue(RadioProperty);
        }

        private static void RadioButtonChecked(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;

            radioButton.Click += RadioButton_Click;
        }

        private static void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            SetRadio(radioButton, radioButton.Content.ToString());
        }
    }
}
