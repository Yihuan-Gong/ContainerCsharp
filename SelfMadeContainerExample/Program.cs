using SelfMadeContainerExample.Birds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfMadeContainerExample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Transit => 每一次從容器拿出都是全新的物件
            // Scope => 這整個API Request 操作的生命週期都會拿到相同的物件
            // Singleton => 無論何時何地 只要從容器拿出都是同一個物件
            Service.AddSingleton<ABird, Sparrow>(x => x.Age = 5);

            var feedbird = new FeedBird();
            feedbird.Feed();
            feedbird.SayAge();

            Console.ReadKey();
        }
    }
}
