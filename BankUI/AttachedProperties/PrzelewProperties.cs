using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BankUI.AttachedProperties
{
    public static class PrzelewProperties
    {
        public static readonly DependencyProperty PolePrzelewProperty =
                    DependencyProperty.RegisterAttached(
                    "PolePrzelew",
                    typeof(string),
                    typeof(PrzelewProperties),
                    new PropertyMetadata(string.Empty, OnPolePrzelewChanged));

        public static string GetPolePrzelew(TextBox textBox)
        {
            return (string)textBox.GetValue(PolePrzelewProperty);
        }
        public static void SetPolePrzelew(TextBox textBox, string value)
        {
            textBox.SetValue(PolePrzelewProperty, value);
        }

        public static void OnPolePrzelewChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            textBox.PreviewTextInput += TextBox_PreviewTextInput;
            textBox.TextChanged += TextBox_TextChanged;
        }

        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            SetPolePrzelew(textBox, textBox.Text.ToString());
        }

        private static void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string znak = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            //dotychczasowy tekst przed dopisaniem bieżącego znaku
            string t = ((TextBox)sender).Text;

            //minus nei moze sie pojawic
            if (e.Text == "-")
                return;

            //separator tez nie moze byc pierwszy
            if (e.Text == znak && t == "")
            {
                e.Handled = true;
                return;
            }


            //po pierwszym zerze może być tylko znak separator
            if (t == "0")
                if (e.Text != znak)
                {
                    e.Handled = true;
                    return;
                }


            //jeśli napis nie rzutuje się na double to nie pozwalamy dopisać bieżącego znaku
            if (!(double.TryParse((e.Text == znak) ? t + e.Text + "0" : t + e.Text, out _)))
            {
                e.Handled = true;
                return;
            }
        }
    }
}
