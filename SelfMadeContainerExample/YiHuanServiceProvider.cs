using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfMadeContainerExample
{
    internal class YiHuanServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, ServiceDescriptor> _typeServiceDescriptorDict;

        public YiHuanServiceProvider(Dictionary<Type, ServiceDescriptor> typeServiceDescriptorDict)
        {
            _typeServiceDescriptorDict = typeServiceDescriptorDict;
        }

        public object GetService(Type serviceType)
        {
            ServiceDescriptor serviceDescriptor = _typeServiceDescriptorDict[serviceType];

            if (serviceDescriptor.ImplementationInstance != null)
            {
                return serviceDescriptor.ImplementationInstance;
            }

            if (serviceDescriptor.ImplementationFactory != null)
            {
                return serviceDescriptor.ImplementationFactory.Invoke(this);
            }

            if (serviceDescriptor.ImplementationType != null)
            {
                return Activator.CreateInstance(serviceDescriptor.ImplementationType);
            }

            return Activator.CreateInstance(serviceDescriptor.ServiceType);
        }
    }
}
