using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfMadeContainerExample.Birds
{
    internal abstract class ABird
    {
        public int Age;

        public abstract void Eat();
        public abstract void SayAge();
    }
}
