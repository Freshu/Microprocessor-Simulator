
using System.Windows;

namespace Projekt
{

    public class Register : MainRegister
    {                       // Register to klasa dziedzicząca po klasie MainRegister. Główny rejestr możemy podzielić na część młodsza i starszą. 
        public Register()   // Właśnie to ma symbolizować ta relacja
        {
            value = "00000000";
        }

        public override void WhoAreYou()
        {
            MessageBox.Show("Hello! I am an SUB-register from uP " + uPtype + ". First of  this type was produced in " + yearOfPrototype +
                " It has " + numberOfPorts + " ports. ALU? " + ALU, "Who am I?");
        }

    }


}