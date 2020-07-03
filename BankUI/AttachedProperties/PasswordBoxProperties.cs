using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BankUI.AttachedProperties
{
    public static class PasswordBoxProperties
    {
        public static readonly DependencyProperty HasloProperty =
            DependencyProperty.RegisterAttached(
                "Haslo",
                typeof(string),
                typeof(PasswordBoxProperties),
                new PropertyMetadata(string.Empty, PasswordBoxTextChanged)
                );

        public static string GetHaslo(PasswordBox passwordBox)
        {
            return (string)passwordBox.GetValue(HasloProperty);
        }
        public static void SetHaslo(PasswordBox passwordBox, bool value)
        {
            passwordBox.SetValue(HasloProperty, value);
        }

        public static void PasswordBoxTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passworBox = sender as PasswordBox;
            if(passworBox == null)
            {
                return;
            }
            var Haslo = (string)e.NewValue ?? string.Empty;
            passworBox.Password = Haslo;
        }
    }
}
