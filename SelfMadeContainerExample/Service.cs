using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SelfMadeContainerExample
{
    public class Service
    {
        public static YiHuanServiceCollection ServiceCollection { get; private set; }

        static Service()
        {
            ServiceCollection = new YiHuanServiceCollection();
        }


        public static void AddTransit<Tparent, Tchild>()
        {
            Type serviceType = typeof(Tparent);
            Type implementationType = typeof(Tchild);

            ServiceCollection.Add(
                new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Transient)
            );
        }

        public static void AddSingleton<Tparent, Tchild>()
            where Tchild : class
        {
            Type serviceType = typeof(Tparent);
            Type implementationType = typeof(Tchild);

            ServiceCollection.Add(new ServiceDescriptor(serviceType, implementationType, ServiceLifetime.Singleton));
        }

        public static void AddSingleton<T>(T obj)
        {
            if (obj == null) return;

            Type serviceType = obj.GetType();
            ServiceCollection.Add(new ServiceDescriptor(serviceType, obj));
        }

        public static void AddSingleton<Tparent, Tchild>(Func<Tchild> factory)
            where Tchild : class, new()
        {
            Type serviceType = typeof(Tparent);
            Type implementationType = typeof(Tchild);

            Func<IServiceProvider, Tchild> implementationFactory = sp =>
            {
                return factory.Invoke();
            };

            ServiceCollection.Add(new ServiceDescriptor(serviceType, implementationFactory, ServiceLifetime.Singleton));
        }


        public static IServiceProvider BuildServiceProvider()
        {
            return new YiHuanServiceProvider(ServiceCollection.TypeServiceDescriptorDict);
        }

        //public static Tparent GetInstance<Tparent>()
        //{
        //    Type serviceType = typeof(Tparent);
        //    ServiceDescriptor serviceDescriptor = ServiceCollection.GetServiceDescriptor(serviceType) ??
        //        throw new ArgumentNullException("item");

        //    if (serviceDescriptor.ImplementationInstance != null)
        //    {
        //        return (Tparent)serviceDescriptor.ImplementationInstance;
        //    }

        //    if (serviceDescriptor.ImplementationFactory != null)
        //    {
        //        return serviceDescriptor.ImplementationFactory.Invoke();
        //    }

        //    if (serviceDescriptor.ImplementationType != null)
        //    {
        //        return (Tparent)Activator.CreateInstance(serviceDescriptor.ImplementationType);
        //    }

        //    return (Tparent)Activator.CreateInstance(serviceDescriptor.ServiceType);
        //}
    }
}
