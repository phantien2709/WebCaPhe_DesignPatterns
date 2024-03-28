using System.Collections.Generic;
using System;

namespace doan.Controllers
{
    public class CacheDecorator
    {
        private Dictionary<string, object> cache = new Dictionary<string, object>();

        public T Execute<T>(Func<T> func, params object[] args)
        {
            string key = $"{func.Method.Name}_{string.Join("_", args)}";

            if (cache.ContainsKey(key))
            {
                Console.WriteLine($"Retrieving cached result for key: {key}");
                return (T)cache[key];
            }
            else
            {
                Console.WriteLine($"Executing function and caching result for key: {key}");
                T result = func.Invoke();
                cache[key] = result;
                return result;
            }
        }
    }
}