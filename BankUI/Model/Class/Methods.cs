using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projekt.Class
{
    class Methods
    {
        private static Random random = new Random();
        private static int pinLength = 4;
        private static int credictCardLength = 16;
        private static string bankNumber = "PL5810501562";
        //Generowanie numeru karty
        public static string CardNumberGenerator()
        {
            int[] cardNumber = new int[credictCardLength];
            int[] luhnNumber = new int[credictCardLength - 1];

            //Zalozmy ze mamy tylko karty VISA, one zaczynaja sie na 4 (Mozna wybrac inne)
            cardNumber[0] = 4;
            luhnNumber[0] = (2 * cardNumber[0]) % 9;

            for (int i = 1; i < credictCardLength - 1; i++)
            {
                cardNumber[i] = random.Next(0, 9);
                luhnNumber[i] = (i % 2 == 0 ? (cardNumber[i] * 2) % 9 : cardNumber[i]);
            }
            cardNumber[credictCardLength - 1] = (luhnNumber.Sum() * 9) % 10;

            StringBuilder sbCard = new StringBuilder();
            for (int i = 0; i < credictCardLength; i++)
            {
                sbCard.Append(cardNumber[i].ToString());
            }
            return sbCard.ToString();
        }

        public static string PinCodeGenerator()
        {
            StringBuilder pin = new StringBuilder();

            for (int i = 0; i < pinLength; i++)
            {
                pin.Append(random.Next(0, 9).ToString());
            }
            return pin.ToString();
        }

        public static string AccountNumberGenerator()
        {
            bool isCorrect = false;
            string accountNumber = string.Empty;
            while (isCorrect == false)
            {
                accountNumber = string.Empty;
                accountNumber += bankNumber;
                for (int i = 0; i < 16; i++)
                {
                    accountNumber += random.Next(0, 9);
                }
                isCorrect = NumberValidator(accountNumber);
            }
            return accountNumber;
        }

        private static bool NumberValidator(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
                return false;
            else if (System.Text.RegularExpressions.Regex.IsMatch(accountNumber, "^[A-Z0-9]"))
            {
                string swapNumber = accountNumber.Substring(4, accountNumber.Length - 4) + accountNumber.Substring(0, 4);
                //A = 10, A w ASCII 65
                int ASCII_Shift = 55;
                StringBuilder sb = new StringBuilder();
                foreach (char x in swapNumber)
                {
                    int number;
                    if (char.IsLetter(x)) number = x - ASCII_Shift;
                    else number = int.Parse(x.ToString());
                    sb.Append(number);
                }
                string checkString = sb.ToString();
                int checkSum = int.Parse(checkString.Substring(0, 1));
                for (int i = 0; i < checkString.Length; i++)
                {
                    int x = int.Parse(checkString.Substring(i, 1));
                    checkSum *= 10;
                    checkSum += x;
                    checkSum %= 97;
                }
                return checkSum == 1;
            }
            else
                return false;
        }
    }
}
