﻿using Projekt.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BankUI.AttachedProperties
{
    public static class LoginProperties
    {
        public static readonly DependencyProperty UnikatowyLoginProperty =
            DependencyProperty.RegisterAttached(
                "UnikatowyLogin",
                typeof(string),
                typeof(LoginProperties),
                new PropertyMetadata(string.Empty, OnUnikatowyLoginChanged));

        public static string GetUnikatowyLogin(TextBox textBox)
        {
            return (string)textBox.GetValue(UnikatowyLoginProperty);
        }
        public static void SetUnikatowyLogin(TextBox textBox, string value)
        {
            textBox.SetValue(UnikatowyLoginProperty, value);
        }

        public static void OnUnikatowyLoginChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            
            textBox.GotFocus += TextBox_GotFocus;
            textBox.LostFocus += TextBox_LostFocus;
        }

        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            if (RepositoryWlasciciel.DoesLoginExist(textBox.Text))
            {
                MessageBox.Show("Podany login jest już zajęty");
                textBox.Foreground = new SolidColorBrush(Colors.Red);
                textBox.Text = "Podaj inny login";
                SetUnikatowyLogin(textBox, null);
            }
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox.Text == "Podaj inny login")
            {
                textBox.Foreground = new SolidColorBrush(Colors.Black);
                textBox.Text = string.Empty;
            }
        }
    }
}
