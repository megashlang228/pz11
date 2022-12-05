using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pz11
{
    internal class Originator
    {
        public State State { get; set; } = new State();
        public void SetMomento(Momento momento)
        {
            State = momento.State;
        }
        public Momento CreateMomento()
        {
            return new Momento(State);
        }

    }
}
