using ContainerExample.Birds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerExample
{
    internal class FeedBird
    {
        readonly ABird bird;

        public FeedBird()
        {
            bird = Container.GetInstance<ABird>();
        }

        public void Feed()
        {
            bird.Eat();
        }
    }
}
