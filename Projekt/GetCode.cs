namespace Projekt
{

    public class GetCode
    {       // Klasa istniejąca w celu wyodrębnienia informacji z wpisanego w programie kodu. Zczytujemy linię kodu i ją analizujemy.
            // Zawiera także zmienne, które modyfikujemy podczas wpisywania / wykonywania kodu.
        public int elementsInCodeLine { get; set; } = 0;        // Aktualna ilość wpisanych części instrukcji 1- instrukcja; 2- instrukcja+rejestr, 3-instrukcja+rejestr+R/liczba
        public int LineNumber { get; set; } = 1;                // Numer linii w którym aktualnie wpisujemy instrukcję
        public bool NextLine { get; set; } = false;             // Czy przejść do następnej linii
        public string OperationInProgress { get; set; } = "";   // Jaki rozkaz aktualnie odczytaliśmy
        public string nameOfRegister1 { get; set; } = "";       // Nazwy rejestrów
        public string nameOfRegister2 { get; set; } = "";
        public string hexValue { get; set; } = "";              // Wartość adresowania natychmiastowego

        public void ReadCode(string line)                       // Wyodrębniamy zmienne z linii kodu. Tworzymy kod wciskając przyciski, więc zawsze ma ten sam format.
        {                                                       // Brak możliwości błędu.
            line.ToCharArray();
            OperationInProgress = line[3].ToString() + line[4].ToString() + line[5].ToString() + line[6].ToString();
            nameOfRegister1 = line[9].ToString() + line[10].ToString();
            if (line[16].ToString() != "H")         // rozkaz "liczbaH" czy rozkaz "rejestr" 
            {
                nameOfRegister2 = line[14].ToString() + line[15].ToString();
            }
            else
            {
                hexValue = line[14].ToString() + line[15].ToString();
            }
        }

        public void ClearParameters()       // Wykonuje się przy przejściu do nowej linii (odczytywanie)
        {
            OperationInProgress = "";
            nameOfRegister1 = "";
            nameOfRegister2 = "";
            hexValue = "";
            NextLine = false;
        }
    }
}
