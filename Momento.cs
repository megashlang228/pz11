using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz11
{
    internal class Momento
    {
        public State State { get; set; }

        public Momento(State state)
        {
            State = new State(state.Text, state.FontSize, state.IsBold, state.IsItalics, state.IsUnderline);
        }

    }
}
