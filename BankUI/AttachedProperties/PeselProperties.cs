using Projekt.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BankUI.AttachedProperties
{
    using R = Properties.Resources;
    /// <summary>
    /// Właściwości dołączone odpowiedzialne za możliwe wyłącznie wprowadzanie liczb do pola
    /// </summary>
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
        // Zmiana koloru 


        /// <summary>
        /// Właściwości dołączone odpowiedzialne za sprawdzenie unikalności wprowadzonego numeru pesel
        /// </summary>
        public static readonly DependencyProperty PeselPoprawnyProperty =
            DependencyProperty.RegisterAttached(
                "PeselPoprawny",
                typeof(string),
                typeof(PeselProperties),
                new PropertyMetadata(string.Empty, OnPeselPoprawnyChanged)
                );

        public static string GetPeselPoprawny(TextBox textBox)
        {
            return (string)textBox.GetValue(PeselPoprawnyProperty);
        }

        public static void SetPeselPoprawny(TextBox textBox, string value)
        {
            textBox.SetValue(PeselPoprawnyProperty, value);
        }

        private static void OnPeselPoprawnyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var textBox = sender as TextBox;

            textBox.LostFocus += TextBox_LostFocus;
            textBox.GotFocus += TextBox_GotFocus;
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            string wpisanyPesel = textBox.Text;
            if(wpisanyPesel == R.differentPESEL)
            {
                textBox.Foreground = new SolidColorBrush(Colors.Black);
                textBox.Text = string.Empty;
            }
        }

        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            string wpisanyPesel = textBox.Text;
            if (wpisanyPesel.Length > 0 && RepositoryWlasciciel.DoesPeselExist(Int64.Parse(wpisanyPesel)))
            {
                MessageBox.Show(R.PESELexists);
                textBox.Foreground = new SolidColorBrush(Colors.Red);
                textBox.Text = R.differentPESEL;
                SetPeselPoprawny(textBox, null);
            }
        }
    }
}
