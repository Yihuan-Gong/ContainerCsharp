using SelfMadeContainerExample.Birds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using NLog.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace SelfMadeContainerExample
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Transit => 每一次從容器拿出都是全新的物件
            // Scope => 這整個API Request 操作的生命週期都會拿到相同的物件
            // Singleton => 無論何時何地 只要從容器拿出都是同一個物件
            //Service.AddSingleton<ABird, Sparrow>(x => x.Age = 5);

            //var feedbird = new FeedBird();
            //feedbird.Feed();
            //feedbird.SayAge();

            //Service.ExtensionCollection.Add(ServiceDescriptor.Singleton(new Sparrow { Age = 10 }));



            //var config = CreateConfig();
            //Service.ExtensionCollection.AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.ClearProviders();
            //    loggingBuilder.AddNLog(config);
            //});
            //Service.AddSingleton<Sparrow>(new Sparrow { Age = 10 });

            //var sparrow = Service.GetInstance<Sparrow>();
            //sparrow.SayAge();
            //sparrow.Eat();


            var config = CreateConfig();
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.AddNLog(config);
                })
                .BuildServiceProvider();



            Console.ReadKey();
        }

        private static IConfiguration CreateConfig()
        {
            var config = new ConfigurationBuilder()
                         .SetBasePath(System.IO.Directory.GetCurrentDirectory())
                         .AddJsonFile("appsettings.json", true, true)
                         .Build();
            return config;
        }
    }
}
