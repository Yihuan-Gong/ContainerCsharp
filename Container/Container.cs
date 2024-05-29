using ContainerExample.Birds;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ContainerExample
{
    internal class Container
    {
        static readonly ServiceCollection services = new ServiceCollection();


        public static void Register()
        {
            services.AddScoped<ABird, Sparrow>();

        }


        public static T GetInstance<T>()
        {
            ServiceProvider provider = services.BuildServiceProvider();
            T instance = provider.GetService<T>();

            return instance;
        }
    }
}
