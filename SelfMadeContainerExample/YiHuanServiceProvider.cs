﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SelfMadeContainerExample
{
    internal class YiHuanServiceProvider : IServiceProvider
    {
        private readonly Dictionary<Type, List<ServiceDescriptor>> _typeServiceDescriptorDict;

        public YiHuanServiceProvider(Dictionary<Type, List<ServiceDescriptor>> typeServiceDescriptorDict)
        {
            _typeServiceDescriptorDict = typeServiceDescriptorDict;
        }

        public object GetService(Type serviceType)
        {
            List<ServiceDescriptor> serviceDescriptorList;

            if (serviceType.IsEnumerable())
            {
                var serviceTypeInsideIEnurmerable = serviceType.GetGenericArguments()[0];
                serviceDescriptorList = GetServiceDescriptorList(serviceTypeInsideIEnurmerable);

                return GetIEnumerableImplementationInstance(serviceTypeInsideIEnurmerable, serviceDescriptorList);
            }

            serviceDescriptorList = GetServiceDescriptorList(serviceType);
            if (serviceDescriptorList != null)
                return GetImplementationInstance(serviceType, serviceDescriptorList.Last());
            else
                return null;



            //ServiceDescriptor serviceDescriptor = serviceDescriptorList.Last();

            //if (serviceDescriptor == null && serviceType.IsGenericType)
            //    serviceDescriptor = GetServiceDescriptorFromGeneric(serviceType);

            //if (serviceDescriptor == null)
            //    return null;

            //if (serviceDescriptor.ImplementationInstance != null)
            //    return serviceDescriptor.ImplementationInstance;

            //object implementorInstance;
            //if (serviceDescriptor.ImplementationFactory != null)
            //{
            //    implementorInstance = serviceDescriptor.ImplementationFactory.Invoke(this);
            //}
            //else if (serviceDescriptor.ImplementationType != null)
            //{
            //    implementorInstance = CreateInstance(serviceDescriptor.ImplementationType);
            //}
            //else
            //{
            //    implementorInstance = CreateInstance(serviceType);
            //}

            //if (serviceDescriptor.Lifetime == ServiceLifetime.Singleton)
            //{
            //    _typeServiceDescriptorDict[serviceType] =
            //        ServiceDescriptor.Singleton(serviceType, implementorInstance);
            //}

            //return implementorInstance;
        }


        private object GetIEnumerableImplementationInstance(Type serviceType, List<ServiceDescriptor> serviceDescriptorList)
        {
            List<object> result = new List<object>();

            foreach (var serviceDescriptor in serviceDescriptorList)
            {
                result.Add(GetImplementationInstance(serviceType, serviceDescriptor));
            }

            return result;
        }

        private object GetImplementationInstance(Type serviceType, ServiceDescriptor serviceDescriptor)
        {
            if (serviceDescriptor == null)
                return null;

            if (serviceDescriptor.ImplementationInstance != null)
                return serviceDescriptor.ImplementationInstance;

            object implementorInstance;
            if (serviceDescriptor.ImplementationFactory != null)
            {
                implementorInstance = serviceDescriptor.ImplementationFactory.Invoke(this);
            }
            else if (serviceDescriptor.ImplementationType != null)
            {
                implementorInstance = CreateInstance(serviceDescriptor.ImplementationType);
            }
            else
            {
                implementorInstance = CreateInstance(serviceType);
            }

            if (serviceDescriptor.Lifetime == ServiceLifetime.Singleton)
            {
                var updatedServiceDescriptor = ServiceDescriptor.Singleton(serviceType, implementorInstance);

                if (_typeServiceDescriptorDict.ContainsKey(serviceType))
                {
                    int index = _typeServiceDescriptorDict[serviceType]
                        .FindIndex(sp => sp.ImplementationType == serviceDescriptor.ImplementationType);


                    //var serviceDescriptorForSpecificImplemtationInstance = _typeServiceDescriptorDict[serviceType]
                    //    .FirstOrDefault(sp => sp.ImplementationType == serviceDescriptor.ImplementationType);
                    //.Where(sp => sp.ImplementationType == serviceDescriptor.ImplementationType)?.FirstOrDefault();

                    if (index != -1)
                        _typeServiceDescriptorDict[serviceType][index] = updatedServiceDescriptor;
                    else
                        _typeServiceDescriptorDict[serviceType].Add(updatedServiceDescriptor);
                }
                //_typeServiceDescriptorDict[serviceType].Add(updatedServiceDescriptor);
                else
                    _typeServiceDescriptorDict.Add(serviceType, new List<ServiceDescriptor> { updatedServiceDescriptor });
            }

            return implementorInstance;
        }



        private List<ServiceDescriptor> GetServiceDescriptorList(Type serviceType)
        {
            _typeServiceDescriptorDict.TryGetValue(serviceType, out var serviceDescriptorList);

            if (serviceDescriptorList == null && serviceType.IsGenericType)
                serviceDescriptorList = GetServiceDescriptorListFromGeneric(serviceType);

            return serviceDescriptorList;
        }


        private List<ServiceDescriptor> GetServiceDescriptorListFromGeneric(Type serviceType)
        {
            if (!serviceType.IsGenericType)
                return null;

            var result = new List<ServiceDescriptor>();
            var genericTypeDefinition = serviceType.GetGenericTypeDefinition();
            if (_typeServiceDescriptorDict.TryGetValue(genericTypeDefinition, out var serviceDescriptorList))
            {
                foreach (var serviceDescriptor in serviceDescriptorList)
                {
                    result.Add(new ServiceDescriptor(
                        serviceType,
                        serviceDescriptor.ImplementationType.MakeGenericType(serviceType.GetGenericArguments()),
                        serviceDescriptor.Lifetime));
                }
                return result;
            }

            return null;
        }

        //private object CreateInstance(Type type)
        //{
        //    ConstructorInfo[] ctors = type.GetConstructors();

        //    // 檢查是否有無參數建構函式
        //    var noPararmCtor = ctors.FirstOrDefault(c => c.GetParameters().Length == 0);

        //    if (noPararmCtor != null)
        //    {
        //        return Activator.CreateInstance(type);
        //    }
        //    else
        //    {
        //        // 嘗試使用服務提供者解析建構函式參數
        //        var ctorParams = ctors
        //            .First()
        //            .GetParameters()
        //            .Select(param => GetService(param.ParameterType))
        //            .ToArray();

        //        return Activator.CreateInstance(type, ctorParams);
        //    }
        //}

        private object CreateInstance(Type type)
        {
            var ctors = type.GetConstructors();
            var parameterCache = new Dictionary<Type, object>();

            // 優先使用有參數的建構函式
            foreach (var ctor in ctors.OrderByDescending(c => c.GetParameters().Length))
            {
                var parameters = ctor.GetParameters();
                var args = new object[parameters.Length];
                var canResolve = true;

                for (int i = 0; i < parameters.Length; i++)
                {
                    var paramType = parameters[i].ParameterType;
                    if (!parameterCache.TryGetValue(paramType, out var resolvedParam))
                    {
                        resolvedParam = GetService(paramType);
                        if (resolvedParam != null)
                        {
                            parameterCache[paramType] = resolvedParam;
                        }
                        else
                        {
                            canResolve = false;
                            break;
                        }
                    }
                    args[i] = resolvedParam;
                }

                if (canResolve)
                {
                    return Activator.CreateInstance(type, args);
                }
            }

            // 若無法解析有參數的建構函式，則使用無參數建構函式
            var noParamCtor = ctors.FirstOrDefault(c => c.GetParameters().Length == 0);
            if (noParamCtor != null)
            {
                return Activator.CreateInstance(type);
            }

            throw new InvalidOperationException($"No suitable constructor found for type {type}");
        }
    }
}
