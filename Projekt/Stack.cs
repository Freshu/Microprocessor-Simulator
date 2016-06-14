using System.Collections.Generic;
using System.Windows;

namespace Projekt
{
    public class Stack
    {   // Własna implementacja stosu wynika z założeń projektu
        const int stackSize = 20;
        private int startAddr = 50;
        private int stopAddr;
        private Stack<string> stack = new Stack<string>();
        private int stackPointer;

        public Stack() 
        {
            stackPointer = startAddr;
            stopAddr = startAddr - stackSize;
        }
        public void PUSH(string registerValue)
        {
            if (stackPointer > stopAddr)
            {
                stack.Push(registerValue);
                stackPointer -= 1;          // Wskaźnik stosu podczas dodawania elementów wskazuje coraz niższe adresy
            }
            else
            {
                MessageBox.Show("Stack is full.", "Impossible action!");
            }
        }
        public string POP()
        {
            if (stackPointer < startAddr)
            {
                stackPointer += 1;
                string registerValue = stack.Pop();
                return registerValue;
            }
            else
            {
                MessageBox.Show("Stack is empty.", "Impossible action!");
                return "00000000";
            }
        }
    }
}
