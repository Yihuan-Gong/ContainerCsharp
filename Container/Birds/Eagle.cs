using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerExample.Birds
{
    internal class Eagle : ABird
    {
        public override void Eat()
        {
            Console.WriteLine("老鷹吃飯");
        }
    }
}
