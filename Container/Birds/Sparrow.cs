using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerExample.Birds
{
    internal class Sparrow : ABird
    {
        public override void Eat()
        {
            Console.WriteLine("麻雀吃飯");
        }
    }
}
