using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BankUI.AttachedProperties
{
    public static class PeselProperties
    {
        public static readonly DependencyProperty OnlyDigitProperty =
            DependencyProperty.RegisterAttached(
                "OnlyDigit",
                typeof(bool),
                typeof(PeselProperties),
                new PropertyMetadata(false, OnOnlyDigitChanged));

        public static bool GetOnlyDigit(TextBox textBox)
        {
            return (bool)textBox.GetValue(OnlyDigitProperty);
        }

        public static void SetOnlyDigit(TextBox textBox, bool value)
        {
            textBox.SetValue(OnlyDigitProperty, value);
        }

        private static void OnOnlyDigitChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            bool isOnlyDigit = (bool)(e.NewValue);
            if (isOnlyDigit)
            {
                textBox.PreviewTextInput += CheckCharacters;
            }
            else
                textBox.PreviewTextInput -= CheckCharacters;
        }

        private static void CheckCharacters(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            foreach (var ch in e.Text)
            {
                if (!(Char.IsDigit(ch))) {
                    e.Handled = true;

                    break;
                }
            }
        }
    }
}
