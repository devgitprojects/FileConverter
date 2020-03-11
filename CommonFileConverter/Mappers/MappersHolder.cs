using CommonFileConverter.Interfaces;
using CommonFileConverter.Models;
using System;
using System.Collections.Concurrent;

namespace CommonFileConverter.Mappers
{
    /// <summary>
    /// Holds pairs of type of mapper and its instance
    /// </summary>
    public sealed class MappersHolder
    {
        private readonly ConcurrentDictionary<Type, object> dictionary = new ConcurrentDictionary<Type, object>();
        
        /// <summary>
        /// Maps the specified type argument to the given value. If
        /// the type argument already has a value within the dictionary, updates the value
        /// </summary>
        public void AddOrUpdate<T, TFrom, TTo>(T value) 
            where T : Mapper<TFrom, TTo>, new()
            where TFrom : BaseFileStructure
            where TTo : IInitializable<TFrom>, new()
        {
            dictionary.AddOrUpdate(typeof(T), value, (p, f) => value);
        }

        /// <summary>
        /// Attempts to fetch a value from the dictionary, throwing a
        /// KeyNotFoundException if the specified type argument has no
        /// entry in the dictionary.
        /// </summary>
        public T GetOrAdd<T, TFrom, TTo>()
            where T : Mapper<TFrom, TTo>, new()
            where TFrom : BaseFileStructure
            where TTo : IInitializable<TFrom>, new()
        {
            return (T)dictionary.GetOrAdd(typeof(T), new T());
        }

        /// <summary>
        /// Attempts to fetch a value from the dictionary, returning false and
        /// setting the output parameter to the default value for T if it
        /// fails, or returning true and setting the output parameter to the
        /// fetched value if it succeeds.
        /// </summary>
        public bool TryGet<T, TFrom, TTo>(out T value)
            where T : Mapper<TFrom, TTo>
            where TFrom : BaseFileStructure
            where TTo : IInitializable<TFrom>, new()
        {
            if (dictionary.TryGetValue(typeof(T), out object tmp))
            {
                value = (T)tmp;
                return true;
            }
            value = default(T);
            return false;
        }
    }
}
