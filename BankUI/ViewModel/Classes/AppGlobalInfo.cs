using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankUI.ViewModel.Classes
{
    /// <summary>
    /// Przechowuje dane, ktore sa przekazywane pomiedzy roznymi oknami.
    /// Klasa musi zostac utworzona przed wywolaniem konstruktorow okien i powinna zostac referencja do klasy, aby dane mogly byc aktualizowane
    /// </summary>
    class AppGlobalInfo
    {
        /// <summary>
        /// Nalezy ustawic na true, jezeli istanieja dane do uzupelnienia przelewu przekazane z kredytem
        /// Po odczytaniu danych nalezy ustawic wartosc na false, aby nie byly juz wiecej wykorzystywane
        /// </summary>
        public bool HasData { get; set; }
        public StringKredyt DaneKredyt { get; set; }
        /// <summary>
        /// Numer karty jaki nalezy wycwietlic w oknie Karta
        /// </summary>
        public string NumerKarty { get; set; }
    }
}
