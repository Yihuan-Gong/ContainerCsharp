using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfMadeContainerExample
{
    internal class Service
    {
        static readonly Dictionary<string, ContainerModel> keyValuePairs;


        static Service()
        {
            keyValuePairs = new Dictionary<string, ContainerModel>();
        }


        public static void AddTransit<Tparent, Tchild>()
        {
            string parentType = typeof(Tparent).Name;
            string childType = typeof(Tchild).Name;

            var containerModel = new ContainerModel
            {
                Mode = "Transit",
                ChildType = childType
            };

            keyValuePairs.Add(parentType, containerModel);
        }

        public static void AddSingleton<Tparent, Tchild>()
            where Tchild : class, new()
        {
            string parentType = typeof(Tparent).Name;

            var containerModel = new ContainerModel
            {
                Mode = "Singleton",
                Child = new Tchild()
            };

            keyValuePairs.Add(parentType, containerModel);
        }

        public static void AddSingleton<Tparent, Tchild>(Action<Tchild> assignInitValue)
            where Tchild : class, new()
        {
            string parentType = typeof(Tparent).Name;

            Tchild childInstance = new Tchild();
            assignInitValue.Invoke(childInstance);

            var containerModel = new ContainerModel
            {
                Mode = "Singleton",
                Child = childInstance
            };

            keyValuePairs.Add(parentType, containerModel);
        }

        public static Tparent GetInstance<Tparent>()
        {
            string parentType = typeof(Tparent).Name;
            ContainerModel child = keyValuePairs[parentType];

            if (child.Mode == "Transit")
            {
                var allClasses = Assembly.GetExecutingAssembly();
                foreach (var x in allClasses.GetTypes())
                {
                    if (x.Name == child.ChildType)
                    {
                        var instance = (Tparent)Assembly.GetExecutingAssembly()
                            .CreateInstance(x.FullName);

                        return instance;
                    }
                }
            }

            return (Tparent)child.Child;
        }

        public static Tparent GetInstance<Tparent>(Action<Tparent> assignInitValue)
        {
            string parentType = typeof(Tparent).Name;
            ContainerModel child = keyValuePairs[parentType];

            if (child.Mode == "Transit")
            {
                var allClasses = Assembly.GetExecutingAssembly();
                foreach (var x in allClasses.GetTypes())
                {
                    if (x.Name == child.ChildType)
                    {
                        var instance = (Tparent)Assembly.GetExecutingAssembly()
                            .CreateInstance(x.FullName);

                        assignInitValue.Invoke(instance);
                        return instance;
                    }
                }
            }

            return (Tparent)child.Child;
        }
    }
}
