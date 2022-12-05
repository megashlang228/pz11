using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz11
{
    internal class Caretaker
    {
        public Stack<State> Stetes = new Stack<State>();

        public State pop()
        {
            return Stetes.Pop();
        }

        public void push(State s)
        {
            Stetes.Push(s);
        }
    }
}
