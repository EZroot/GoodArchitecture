using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectScare.ServiceLocator
{
    public static class ServiceLocator
    {
        private static Dictionary<Type, object> _registry = null;

        public static void ClearRegistry()
        {
            _registry.Clear();
        }

        public static void Register<T>() where T : IService
        {
            _registry ??= new Dictionary<Type, object>();

            //does service exist?
            if (_registry.ContainsKey(typeof(T))) return;

            var potentialService = ServiceLocatorUtils.GetClassFromAssemblyByInterface(typeof(T));

            if (potentialService == null)
            {
                var potentialObject = ServiceLocatorUtils.GetSceneObject<T>();

                //monobehaviour
                if (potentialObject != null)
                {
                    Debug.Log($"Service: Registered (Monobehaviour)<color=green>{typeof(T)}</color> success!");
                    _registry[typeof(T)] = potentialObject;
                }
                return;
            }
            Debug.Log($"Service: Registered (Pure C#)<color=green>{typeof(T)}</color> success!");
            var instance = Activator.CreateInstance(potentialService);
            _registry[typeof(T)] = instance;
            //Debug.Log($"Service: Registered (Pure C# Class){typeof(T)} success!");
        }

        public static T Get<T>() where T : IService
        {
            Register<T>();
            return (T)_registry[typeof(T)];
        }
    }
}