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
using System.Collections;
using DTO;
using System.Reflection;

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
            //Service.ServiceCollection.AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.ClearProviders();
            //    loggingBuilder.SetMinimumLevel(LogLevel.Trace);
            //    loggingBuilder.AddNLog(config);
            //});
            //var logger = Service.GetInstance<ILogger<Sparrow>>();
            //Service.AddSingleton<Sparrow>(new Sparrow(logger, 10));
            //var sparrow = Service.GetInstance<Sparrow>();
            //sparrow.SayAge();
            //sparrow.Eat();


            //var config = CreateConfig();
            //var serviceCollection = new ServiceCollection();
            //var serviceProvider = serviceCollection
            //    .AddSingleton<Sparrow>(sp => new Sparrow(sp.GetService<ILogger<Sparrow>>(), 10))
            //    .AddLogging(loggingBuilder =>
            //    {
            //        loggingBuilder.ClearProviders();
            //        loggingBuilder.AddNLog(config);
            //    })
            //    .BuildServiceProvider();
            //var sparrow = serviceProvider.GetService<Sparrow>();
            //sparrow.SayAge();
            //sparrow.Eat();


            //var serviceProvider = new ServiceCollection()
            //    .AddSingleton<ABird, Eagle>()
            //    .AddSingleton<ABird, Sparrow>()
            //    .BuildServiceProvider();
            //var list = serviceProvider.GetService<ABird>();


            Service.AddSingleton<ABird, Eagle>();
            Service.AddSingleton<ABird, Sparrow>();
            var list2 = Service.GetInstance<IEnumerable<ABird>>();


            //var type = typeof(IEnumerable<Eagle>);
            //var eagle = type.GetGenericArguments()[0];

            //Type type = typeof(IEnumerable<Eagle>);
            //List<object> eagle = new List<object> { new Eagle() };

            //var list = ToSpecificType<IEnumerable<Eagle>>(eagle);


            //IEnumerable<Eagle> list = (IEnumerable<Eagle>)eagle;
            //IEnumerable<Eagle> list = eagle.OfType<Eagle>();


            Console.ReadKey();
        }

        public static T ToSpecificType<T>(object obj)
        {
            if (typeof(T).IsEnumerable())
            {
                Type elementType = typeof(T).GetGenericArguments().First();
                IList list = CreateGenericList(elementType);

                foreach (var item in (IEnumerable)obj)
                {
                    AddElementToList(list, item, elementType);
                }
                return (T)list;
            }
            return (T)obj;
        }


        public static IList CreateGenericList(Type type)
        {
            // 獲取List<>類型
            Type listType = typeof(List<>);

            // 創建List<>類型，並將元素類型設置為type
            Type constructedListType = listType.MakeGenericType(type);

            // 創建List<type>的實例
            return (IList)Activator.CreateInstance(constructedListType);
        }

        public static void AddElementToList(IList list, object element, Type type)
        {
            // 獲取List<>.Add方法
            var method = list.GetType().GetMethod("Add");

            // 將element轉型成type並加入list
            //var convertedElement = Convert.ChangeType(element, type);
            method.Invoke(list, new[] { element });
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
