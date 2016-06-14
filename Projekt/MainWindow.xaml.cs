using System.Windows;
using System;
using System.IO;

namespace Projekt
{
    /// <summary>
    ///     Logika interakcji dla okna programu. 
    /// </summary>

    public partial class MainWindow : Window
    {

        public enum Instructions { MOV, ADD, SUB, PUSH, POP}
        public enum Registers { AH, AL, BH, BL, CH, CL, DH, DL}

        private Register RegisterAH = new Register();
        private Register RegisterAL = new Register();
        private Register RegisterBH = new Register();
        private Register RegisterBL = new Register();
        private Register RegisterCH = new Register();
        private Register RegisterCL = new Register();
        private Register RegisterDH = new Register();
        private Register RegisterDL = new Register();
        private GetCode code = new GetCode();
        private Stack stack = new Stack();
 


        public MainWindow()
        {
            InitializeComponent();

            InstructionComboBox.ItemsSource = Enum.GetValues(typeof(Instructions));
            InstructionComboBox.SelectedIndex = 0;
            RegisterComboBox.ItemsSource = Enum.GetValues(typeof(Registers));
            RegisterComboBox.SelectedIndex = 0;
            HexaDecimalTextBox.Text = "00";
            CodeTextBlock.Text = code.LineNumber + "  ";

            RegisterAHTextBlock.DataContext = RegisterAH;
            RegisterALTextBlock.DataContext = RegisterAL;
            RegisterBHTextBlock.DataContext = RegisterBH;
            RegisterBLTextBlock.DataContext = RegisterBL;
            RegisterCHTextBlock.DataContext = RegisterCH;
            RegisterCLTextBlock.DataContext = RegisterCL;
            RegisterDHTextBlock.DataContext = RegisterDH;
            RegisterDLTextBlock.DataContext = RegisterDL;


        }

        private void AddInstructionButton_Click(object sender, RoutedEventArgs e)
        {
            if (code.elementsInCodeLine < 1)
            {             
                if(InstructionComboBox.Text != "PUSH")  // Instrukcja PUSH jako jedyna jest 4-znakowa, a dbamy o taki sam format każdej linii kodu
                {
                    code.elementsInCodeLine++;
                    CodeTextBlock.Text = CodeTextBlock.Text + InstructionComboBox.Text + "   ";
                }
                else
                {
                    code.elementsInCodeLine++;
                    CodeTextBlock.Text = CodeTextBlock.Text + InstructionComboBox.Text + "  ";
                }
            }
            else
            {
                MessageBox.Show("You already have chosen an instruction.", "Wrong buttton clicked.");
            }          
        }

        private void AddRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if (code.elementsInCodeLine == 1)
            {
                code.elementsInCodeLine++;
                CodeTextBlock.Text = CodeTextBlock.Text + RegisterComboBox.Text + ",  ";
                if (InstructionComboBox.Text == "POP" || InstructionComboBox.Text == "PUSH")
                {
                    code.elementsInCodeLine = 0;
                    code.LineNumber++;                      // Do linii kodu wstawiamy spacje, aby miała on przynajmniej minimalną długość.
                    CodeTextBlock.Text = CodeTextBlock.Text + "                             \n" + code.LineNumber + "  ";
                }
            }
            else if (code.elementsInCodeLine == 2)
            {
                code.LineNumber++;
                CodeTextBlock.Text = CodeTextBlock.Text + RegisterComboBox.Text + "  " + "\n" + code.LineNumber + "  ";
                code.elementsInCodeLine = 0;
            }
            else
            {
                MessageBox.Show("At first choose an instruction.", "Wrong buttton clicked.");
            }                   
        }

        private void ImmediateAddrButton_Click(object sender, RoutedEventArgs e)
        {
            if(code.elementsInCodeLine == 2)
            {
                code.LineNumber++;
                CodeTextBlock.Text = CodeTextBlock.Text + HexaDecimalTextBox.Text + "H  " + "\n" + code.LineNumber + "  ";
                code.elementsInCodeLine = 0;
            }
            else
            {
                MessageBox.Show("You can't use immediate addressing in this instruction.", "Wrong buttton clicked.");
            }
        }

        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            code.LineNumber = 1;
            code.elementsInCodeLine = 0;
            CodeTextBlock.Text = code.LineNumber + "  ";
        }

        private void SaveCodeButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Program";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Notepad Files .txt|*.txt";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                string filePath = dlg.FileName;
                try
                {
                    File.WriteAllText(filePath, CodeTextBlock.Text);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error while saving to file!");
                    throw;
                }
            }
        }

        private void LoadCodeButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Notepad Files .txt|*.txt";

            Nullable<bool> result = dlg.ShowDialog();
            string filePath = "";
            if (result == true)
            {
                filePath = dlg.FileName;
            }
            if (File.Exists(filePath))
            {
                try
                {
                    CodeTextBlock.Text = File.ReadAllText(filePath);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error while loading a file!");
                    throw;
                }
            }
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            string instrukcje = CodeTextBlock.Text;
            using (StringReader sr = new StringReader(instrukcje))
            {
                string line;
                while ((line = sr.ReadLine()) != null && line.Length > 10)
                {
                    code.ReadCode(line);
                    switch (code.OperationInProgress)
                    {
                        case "MOV ": { InstructionMOV(); break; }
                        case "ADD ": { InstructionADD(); break; }
                        case "SUB ": { InstructionSUB(); break; }
                        case "PUSH": { InstructionPUSH(); break; }
                        case "POP ": { InstructionPOP(); break; }
                        default: { MessageBox.Show("Critical error - wrong instruction!"); break; }
                    }
                    code.ClearParameters();
                }
            }
        }

        public string RegisterSwitch(string Register2)
        {                                   // Wybór rejestru na podstawie nazwy
            string tmp = "";
            switch (Register2)
            {
                case "AH": { tmp = RegisterAH.value; break; }
                case "AL": { tmp = RegisterAL.value; break; }
                case "BH": { tmp = RegisterBH.value; break; }
                case "BL": { tmp = RegisterBL.value; break; }
                case "CH": { tmp = RegisterCH.value; break; }
                case "CL": { tmp = RegisterCL.value; break; }
                case "DH": { tmp = RegisterDH.value; break; }
                case "DL": { tmp = RegisterDL.value; break; }
                default: { MessageBox.Show("Wrong register 2 value!"); break; }
            }
            return tmp;
        }

        public void InstructionMOV()
        {
            if(code.nameOfRegister2 == "")  // Jeżeli wystąpiło adresowanie natychmiastowe
            {
                switch (code.nameOfRegister1)
                {
                    case "AH": { RegisterAH.value = Conversion.HexaDecimalToBinaryString(code.hexValue); break; }
                    case "AL": { RegisterAL.value = Conversion.HexaDecimalToBinaryString(code.hexValue); break; }
                    case "BH": { RegisterBH.value = Conversion.HexaDecimalToBinaryString(code.hexValue); break; }
                    case "BL": { RegisterBL.value = Conversion.HexaDecimalToBinaryString(code.hexValue); break; }
                    case "CH": { RegisterCH.value = Conversion.HexaDecimalToBinaryString(code.hexValue); break; }
                    case "CL": { RegisterCL.value = Conversion.HexaDecimalToBinaryString(code.hexValue); break; }
                    case "DH": { RegisterDH.value = Conversion.HexaDecimalToBinaryString(code.hexValue); break; }
                    case "DL": { RegisterDL.value = Conversion.HexaDecimalToBinaryString(code.hexValue); break; }
                    default: { MessageBox.Show("Wrong register 1 value!"); break; }
                }
            }
            else   
            {
                switch (code.nameOfRegister1)
                {
                    case "AH": { RegisterAH.value = this.RegisterSwitch(code.nameOfRegister2); break; }
                    case "AL": { RegisterAL.value = this.RegisterSwitch(code.nameOfRegister2); break; }
                    case "BH": { RegisterBH.value = this.RegisterSwitch(code.nameOfRegister2); break; }
                    case "BL": { RegisterBL.value = this.RegisterSwitch(code.nameOfRegister2); break; }
                    case "CH": { RegisterCH.value = this.RegisterSwitch(code.nameOfRegister2); break; }
                    case "CL": { RegisterCL.value = this.RegisterSwitch(code.nameOfRegister2); break; }
                    case "DH": { RegisterDH.value = this.RegisterSwitch(code.nameOfRegister2); break; }
                    case "DL": { RegisterDL.value = this.RegisterSwitch(code.nameOfRegister2); break; }
                    default: { MessageBox.Show("Wrong register 1 value!"); break; }
                }
            }
        }

        public void InstructionADD()
        {
            if (code.nameOfRegister2 == "") // Jeżeli wystąpiło adresowanie natychmiastowe
            {
                switch (code.nameOfRegister1)
                {
                    case "AH": { RegisterAH.value = Conversion.Add2BinaryStrings(Conversion.HexaDecimalToBinaryString(code.hexValue), RegisterAH.value); break; }
                    case "AL": { RegisterAL.value = Conversion.Add2BinaryStrings(Conversion.HexaDecimalToBinaryString(code.hexValue), RegisterAL.value); break; }
                    case "BH": { RegisterBH.value = Conversion.Add2BinaryStrings(Conversion.HexaDecimalToBinaryString(code.hexValue), RegisterBH.value); break; }
                    case "BL": { RegisterBL.value = Conversion.Add2BinaryStrings(Conversion.HexaDecimalToBinaryString(code.hexValue), RegisterBL.value); break; }
                    case "CH": { RegisterCH.value = Conversion.Add2BinaryStrings(Conversion.HexaDecimalToBinaryString(code.hexValue), RegisterCH.value); break; }
                    case "CL": { RegisterCL.value = Conversion.Add2BinaryStrings(Conversion.HexaDecimalToBinaryString(code.hexValue), RegisterCL.value); break; }
                    case "DH": { RegisterDH.value = Conversion.Add2BinaryStrings(Conversion.HexaDecimalToBinaryString(code.hexValue), RegisterDH.value); break; }
                    case "DL": { RegisterDL.value = Conversion.Add2BinaryStrings(Conversion.HexaDecimalToBinaryString(code.hexValue), RegisterDL.value); break; }
                    default: { MessageBox.Show("ADD instruction error!"); break; }
                }
            }
            else
            {
                switch (code.nameOfRegister1)
                {
                    case "AH": { RegisterAH.value = Conversion.Add2BinaryStrings(this.RegisterSwitch(code.nameOfRegister2), RegisterAH.value); break; }
                    case "AL": { RegisterAL.value = Conversion.Add2BinaryStrings(this.RegisterSwitch(code.nameOfRegister2), RegisterAL.value); break; }
                    case "BH": { RegisterBH.value = Conversion.Add2BinaryStrings(this.RegisterSwitch(code.nameOfRegister2), RegisterBH.value); break; }
                    case "BL": { RegisterBL.value = Conversion.Add2BinaryStrings(this.RegisterSwitch(code.nameOfRegister2), RegisterBL.value); break; }
                    case "CH": { RegisterCH.value = Conversion.Add2BinaryStrings(this.RegisterSwitch(code.nameOfRegister2), RegisterCH.value); break; }
                    case "CL": { RegisterCL.value = Conversion.Add2BinaryStrings(this.RegisterSwitch(code.nameOfRegister2), RegisterCL.value); break; }
                    case "DH": { RegisterDH.value = Conversion.Add2BinaryStrings(this.RegisterSwitch(code.nameOfRegister2), RegisterDH.value); break; }
                    case "DL": { RegisterDL.value = Conversion.Add2BinaryStrings(this.RegisterSwitch(code.nameOfRegister2), RegisterDL.value); break; }
                    default: { MessageBox.Show("ADD instruction error!"); break; }
                }
            }
            
        }

        public void InstructionSUB()
        {
            if (code.nameOfRegister2 == "") // Jeżeli wystąpiło adresowanie natychmiastowe
            {
                switch (code.nameOfRegister1)
                {
                    case "AH": { RegisterAH.value = Conversion.Subtract2BinaryStrings(RegisterAH.value, Conversion.HexaDecimalToBinaryString(code.hexValue)); break; }
                    case "AL": { RegisterAL.value = Conversion.Subtract2BinaryStrings(RegisterAL.value, Conversion.HexaDecimalToBinaryString(code.hexValue)); break; }
                    case "BH": { RegisterBH.value = Conversion.Subtract2BinaryStrings(RegisterBH.value, Conversion.HexaDecimalToBinaryString(code.hexValue)); break; }
                    case "BL": { RegisterBL.value = Conversion.Subtract2BinaryStrings(RegisterBL.value, Conversion.HexaDecimalToBinaryString(code.hexValue)); break; }
                    case "CH": { RegisterCH.value = Conversion.Subtract2BinaryStrings(RegisterCH.value, Conversion.HexaDecimalToBinaryString(code.hexValue)); break; }
                    case "CL": { RegisterCL.value = Conversion.Subtract2BinaryStrings(RegisterCL.value, Conversion.HexaDecimalToBinaryString(code.hexValue)); break; }
                    case "DH": { RegisterDH.value = Conversion.Subtract2BinaryStrings(RegisterDH.value, Conversion.HexaDecimalToBinaryString(code.hexValue)); break; }
                    case "DL": { RegisterDL.value = Conversion.Subtract2BinaryStrings(RegisterDL.value, Conversion.HexaDecimalToBinaryString(code.hexValue)); break; }
                    default: { MessageBox.Show("SUB instruction error!"); break; }
                }
            }
            else
            {
                switch (code.nameOfRegister1)
                {
                    case "AH": { RegisterAH.value = Conversion.Subtract2BinaryStrings(RegisterAH.value, this.RegisterSwitch(code.nameOfRegister2)); break; }
                    case "AL": { RegisterAL.value = Conversion.Subtract2BinaryStrings(RegisterAL.value, this.RegisterSwitch(code.nameOfRegister2)); break; }
                    case "BH": { RegisterBH.value = Conversion.Subtract2BinaryStrings(RegisterBH.value, this.RegisterSwitch(code.nameOfRegister2)); break; }
                    case "BL": { RegisterBL.value = Conversion.Subtract2BinaryStrings(RegisterBL.value, this.RegisterSwitch(code.nameOfRegister2)); break; }
                    case "CH": { RegisterCH.value = Conversion.Subtract2BinaryStrings(RegisterCH.value, this.RegisterSwitch(code.nameOfRegister2)); break; }
                    case "CL": { RegisterCL.value = Conversion.Subtract2BinaryStrings(RegisterCL.value, this.RegisterSwitch(code.nameOfRegister2)); break; }
                    case "DH": { RegisterDH.value = Conversion.Subtract2BinaryStrings(RegisterDH.value, this.RegisterSwitch(code.nameOfRegister2)); break; }
                    case "DL": { RegisterDL.value = Conversion.Subtract2BinaryStrings(RegisterDL.value, this.RegisterSwitch(code.nameOfRegister2)); break; }
                    default: { MessageBox.Show("SUB instruction error!"); break; }
                }
            }
        }

        public void InstructionPUSH()
        {
            switch (code.nameOfRegister1)
            {
                case "AH": { stack.PUSH(RegisterAH.value); break; }
                case "AL": { stack.PUSH(RegisterAL.value); break; }
                case "BH": { stack.PUSH(RegisterBH.value); break; }
                case "BL": { stack.PUSH(RegisterBL.value); break; }
                case "CH": { stack.PUSH(RegisterCH.value); break; }
                case "CL": { stack.PUSH(RegisterCL.value); break; }
                case "DH": { stack.PUSH(RegisterDH.value); break; }
                case "DL": { stack.PUSH(RegisterDL.value); break; }
                default: { MessageBox.Show("PUSH instruction error!"); break; }
            }
        }

        public void InstructionPOP()
        {
            switch (code.nameOfRegister1)
            {
                case "AH": { RegisterAH.value = stack.POP(); break; }
                case "AL": { RegisterAL.value = stack.POP(); break; }
                case "BH": { RegisterBH.value = stack.POP(); break; }
                case "BL": { RegisterBL.value = stack.POP(); break; }
                case "CH": { RegisterCH.value = stack.POP(); break; }
                case "CL": { RegisterCL.value = stack.POP(); break; }
                case "DH": { RegisterDH.value = stack.POP(); break; }
                case "DL": { RegisterDL.value = stack.POP(); break; }
                default: { MessageBox.Show("POP instruction error!"); break; }
            }
        }

        private void InstructionComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        { 
            switch (InstructionComboBox.SelectedIndex)
            {
                case 0: { WhatItDoesTextBlock.Text = 
                            "MOV A, B \n\nCopies B to A. Result in A. \nA: register \nB: register or hex value"; break; }
                case 1: { WhatItDoesTextBlock.Text = 
                            "ADD A, B \n\nAdds B to A. Result in A. \nA: register \nB: register or hex value"; break; }
                case 2: { WhatItDoesTextBlock.Text = 
                            "SUB A, B \n\nSubstracts B from A. Result in A. \nA: register \nB: register or hex value"; break; }
                case 3: { WhatItDoesTextBlock.Text = 
                            "PUSH X \n\nSaves value from register X to the top of stack.\nIt doesn't clear the register, so You can still use it.\n" +
                            "Stack pointer++"; break; }
                case 4: { WhatItDoesTextBlock.Text = 
                            "POP X \n\nLoads value from the top of stack to register X.\n Stack pointer--"; break; }
                default:
                    break;
            }
        }
    }
}
