using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TwoDEngine
{
     
    public static class Registry
    {
        private static Dictionary<Type, List<object>> repository = new Dictionary<Type, List<object>>();

        public static void Register(Object obj){
            AddToRegistry(obj.GetType(), obj);
        }

        private static void AddToRegistry(Type t, Object obj)
        {
            if (!repository.ContainsKey(t))
            {
                repository.Add(t, new List<object>());
            }
            repository[t].Add(obj);
            
            foreach (Type intf in t.GetInterfaces())
            {
                AddToRegistry(intf, obj);
            }

            if (t.BaseType != null)
            {
                AddToRegistry(t.BaseType, obj);
            }
        }

        public static T[] LookupAll<T>()
        {
            if (!repository.ContainsKey(typeof(T))){
                return new T[0];
            } else {
                return repository[typeof(T)].Cast<T>().ToArray();
            }
        }

        public static T Lookup<T>()
        {
            T[] tarray = LookupAll<T>();
            if (tarray.Length == 0)
            {
                return default(T);
            }
            else
            {
                return tarray[0];
            }
        }

        public static void Require<T>()
        {
            if (!repository.ContainsKey(typeof(T)))
            {
                throw new MissingServiceException("Error: Requires a service that implements " + typeof(T).FullName +
                    " be registered with the Registry.");
            }
        }
    } 
}
