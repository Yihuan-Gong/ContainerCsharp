using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfMadeContainerExample
{
    internal static class IServiceProviderExtension
    {
        public static Tparent GetService<Tparent>(this IServiceProvider serviceProvider)
        {
            return (Tparent)serviceProvider.GetService(typeof(Tparent));
        }
    }
}
