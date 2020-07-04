using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BankUI.AttachedProperties
{
    public static class BankomatPin
    {
        public static readonly DependencyProperty PinLiczbyProperty =
            DependencyProperty.RegisterAttached(
                "PinLiczby",
                typeof(string),
                typeof(BankomatPin),
                new PropertyMetadata(string.Empty, PinLiczbaChanged)
        );

        public static string GetPinLiczby(PasswordBox passwordBox)
        {
            return (string)passwordBox.GetValue(PinLiczbyProperty);
        }
        public static void SetPinLiczby(PasswordBox passwordBox, string value)
        {
            passwordBox.SetValue(PinLiczbyProperty, value);
        }

        private static void PinLiczbaChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            passwordBox.PreviewTextInput += PasswordBox_PreviewTextInput;
            passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetPinLiczby(passwordBox, passwordBox.Password);
        }

        private static void PasswordBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            foreach (char x in e.Text)
            {
                if (!char.IsDigit(x))
                {
                    e.Handled = true;
                    break;
                }
            }
        }
    }
}
