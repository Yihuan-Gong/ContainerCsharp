using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfMadeContainerExample.Birds
{
    public class Sparrow : ABird
    {
        public override void Eat()
        {
            Console.WriteLine("麻雀吃飯");
        }

        public override void SayAge()
        {
            Console.WriteLine($"我{Age}歲");
        }
    }
}
