using ContainerExample.Birds;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Container.Register();

            var feedbird = new FeedBird();
            feedbird.Feed();

            Console.ReadKey();
        }
    }
}
