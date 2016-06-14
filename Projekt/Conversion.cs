using System;

namespace Projekt
{
    public static class Conversion
    {       // Statyczna klasa umożliwiająca operowanie na zmiennych zapisanych jako stringi
        public static string Add2BinaryStrings(string sOne, string sTwo)
        {
            int number_one = Convert.ToInt16(sOne, 2);
            int number_two = Convert.ToInt16(sTwo, 2);
            string result = Convert.ToString(number_one + number_two, 2);
            while(result.Length < 8)
            {
                result = "0" + result;  // Uzupełnianie do 8 bitów - tyle można zapisać do rejestru
            }
            return result;
        }

        public static string Subtract2BinaryStrings(string sOne, string sTwo)
        {
            int number_one = Convert.ToInt16(sOne, 2);
            int number_two = Convert.ToInt16(sTwo, 2);
            string result = Convert.ToString(number_one - number_two, 2);
            while (result.Length < 8)
            {
                result = "0" + result;
            }
            return result;
        }

        public static string DecimalToBinaryString(int decimalNr)
        {
            string result = "";
            int remainder = 0;
            while (decimalNr > 0)
            {
                remainder = decimalNr % 2;
                decimalNr /= 2;
                result = remainder.ToString() + result;
            }
            while (result.Length < 8)
            {
                result = "0" + result;
            }
            return result;
        }

        public static string HexaDecimalToBinaryString(string hex)
        {
            hex.ToCharArray();
            string result = "";
            string x0 = SwitchHexChar(hex[0]);
            string x1 = SwitchHexChar(hex[1]);
            result = x0 + x1;
            return result;
        }

        public static string SwitchHexChar(char input)
        {
            switch (input)
            {
                case '0': { return "0000"; }
                case '1': { return "0001"; }
                case '2': { return "0010"; }
                case '3': { return "0011"; }
                case '4': { return "0100"; }
                case '5': { return "0101"; }
                case '6': { return "0110"; }
                case '7': { return "0111"; }
                case '8': { return "1000"; }
                case '9': { return "1001"; }
                case 'a':
                case 'A': { return "1010"; }
                case 'b':
                case 'B': { return "1011"; }
                case 'c':
                case 'C': { return "1100"; }
                case 'd':
                case 'D': { return "1101"; }
                case 'e':
                case 'E': { return "1110"; }
                case 'f':
                case 'F': { return "1111"; }
                default:  { return "0000"; }
            }
        }
    }
}
