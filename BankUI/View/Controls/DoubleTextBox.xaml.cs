using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BankUI.View.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy DoubleTextBox.xaml
    /// </summary>
    public partial class DoubleTextBox : UserControl
    {
        public DoubleTextBox()
        {
            InitializeComponent();
        }

        #region Zdarznie własne
        //rejestracja zdarzenia tak, żeby możliwe było jego bindowanie
        public static readonly RoutedEvent NumberChangedEvent =
        EventManager.RegisterRoutedEvent("TabItemSelected",
                     RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                     typeof(DoubleTextBox));

        //definicja zdarzenia NumberChanged
        public event RoutedEventHandler NumberChanged
        {
            add { AddHandler(NumberChangedEvent, value); }
            remove { RemoveHandler(NumberChangedEvent, value); }
        }

        //Metoda pomocnicza wywołująca zdarzenie
        //przy okazji metoda ta tworzy obiekt argument przekazywany przez to zdarzenie
        void RaiseNunberChanged()
        {
            //argument zdarzenia
            RoutedEventArgs newEventArgs =
                    new RoutedEventArgs(DoubleTextBox.NumberChangedEvent);
            //wywołanie zdarzenia
            RaiseEvent(newEventArgs);
        }
        #endregion

        #region Własność zależna
        //zarejestrowanie własności zależenej - taki mechanizm konieczny jest
        // aby możliwe było Bindowanie tej właśności z innnymi obiektami
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                "Text",
                typeof(string),
                typeof(DoubleTextBox),
                new FrameworkPropertyMetadata(null)
            );
        //"czysta" właściwość powiązania z właściwości zależną
        //do niej będziemy się odnosić w XAMLU
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        #region Metody obsługujące wewnętrzne zdarzenia kontrolki



        //zdarzenie wywoływane zanim zmianie ulegnie tekst textBox-a
        //e.Text  - string zawierający ostatnio dopisany znakm jeszcze niedodany do 
        //własności Text obiektu textBox
        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

            //znak separacji zależny od bieżącej kultury językowej
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

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            //przy każdej zmianie tekstu w polu textBox
            //wyrzucamy zdarzenie, które informuje o tym,
            //że zmieniła się liczba
            RaiseNunberChanged();
        }

        #endregion
    }
}
