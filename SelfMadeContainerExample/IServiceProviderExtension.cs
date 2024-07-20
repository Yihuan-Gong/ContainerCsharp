using System;
using System.Collections;
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
            object result = serviceProvider.GetService(typeof(Tparent));

            return ToSpecificType<Tparent>(result);
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
    }
}
