using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BankUI.AttachedProperties
{
    /// <summary>
    /// Właściwości dołączone umożliwiające zbindowanie hasła do zmiennej w VM
    /// </summary>
    public static class PasswordBoxProperties
    {
        public static readonly DependencyProperty HasloProperty =
            DependencyProperty.RegisterAttached(
                "Haslo",
                typeof(string),
                typeof(PasswordBoxProperties),
                new PropertyMetadata(string.Empty, OnHasloChanged)
                );

        public static string GetHaslo(PasswordBox passwordBox)
        {
            return (string)passwordBox.GetValue(HasloProperty);
        }
        public static void SetHaslo(PasswordBox passwordBox, string value)
        {
            passwordBox.SetValue(HasloProperty, value);
        }

        private static void OnHasloChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if(passwordBox == null)
            {
                return;
            }
            passwordBox.PasswordChanged += PasswodrBox_PasswordChanged;
        }

        private static void PasswodrBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetHaslo(passwordBox, passwordBox.Password);
        }
    }
}
