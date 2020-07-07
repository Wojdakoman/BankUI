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
    /// Właściwości dołączone odpowiedzialne za odpowiednią stylizacje wprowadzanego numeru konta oraz wpisywanych wyłącznie liczb
    /// </summary>
    class NumerKartyProperties
    {
        public static readonly DependencyProperty NumerKartyProperty =
            DependencyProperty.RegisterAttached(
                "NumerKarty",
                typeof(string),
                typeof(NumerKartyProperties),
                new PropertyMetadata(string.Empty, NumerKartyChanged));

        public static void SetNumerKarty(TextBox textBox, string value)
        {
            textBox.SetValue(NumerKartyProperty, value);
        }
        public static string GetNumerKarty(TextBox textBox)
        {
            return (string)textBox.GetValue(NumerKartyProperty);
        }

        private static void NumerKartyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            textBox.PreviewTextInput += TextBox_PreviewTextInput;
            textBox.TextChanged += TextBox_TextChanged;
        }

        private static void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            string textInTB = textBox.Text.Replace(" ", "");
            SetNumerKarty(textBox, textInTB);

            for (int i = 4; i < textInTB.Length; i+=4)
            {
                textInTB = textInTB.Insert(i, " ");
                i++;
            }
            textBox.Text = textInTB;
            textBox.CaretIndex = textBox.Text.Length;
        }

        private static void TextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
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
