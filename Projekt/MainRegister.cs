using PropertyChanged;
using System.Windows;

namespace Projekt
{
    [ImplementPropertyChanged]
    public class MainRegister : IwhatShouldRegisterHave
    {
        public const string uPtype = "8051";
        public const int yearOfPrototype = 1980;
        public const int numberOfPorts = 4;
        public const bool ALU = true;

        public string value { get; set; } = "0000000000000000";


        public virtual void WhoAreYou()
        {
            MessageBox.Show("Hello! I am an register from uP " + uPtype + ". First of  this type was produced in " + yearOfPrototype +
                "It has " + numberOfPorts + " ports. ALU? " + ALU, "Who am I?");
        }

    }


}
