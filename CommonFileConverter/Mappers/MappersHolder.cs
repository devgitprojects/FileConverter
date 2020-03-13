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
        public void AddOrUpdate<TMapper, TFrom, TTo>(TMapper value) 
            where TMapper : Mapper<TFrom, TTo>, new()
            where TFrom : BaseModel
            where TTo : IInitializable<TFrom>, new()
        {
            dictionary.AddOrUpdate(typeof(TMapper), value, (p, f) => value);
        }

        /// <summary>
        /// Attempts to fetch a value from the dictionary, throwing a
        /// KeyNotFoundException if the specified type argument has no
        /// entry in the dictionary.
        /// </summary>
        public TMapper GetOrAdd<TMapper, TFrom, TTo>()
            where TMapper : Mapper<TFrom, TTo>, new()
            where TFrom : BaseModel
            where TTo : IInitializable<TFrom>, new()
        {
            return (TMapper)dictionary.GetOrAdd(typeof(TMapper), new TMapper());
        }

        /// <summary>
        /// Attempts to fetch a value from the dictionary, returning false and
        /// setting the output parameter to the default value for T if it
        /// fails, or returning true and setting the output parameter to the
        /// fetched value if it succeeds.
        /// </summary>
        public bool TryGet<TMapper, TFrom, TTo>(out TMapper value)
            where TMapper : Mapper<TFrom, TTo>
            where TFrom : BaseModel
            where TTo : IInitializable<TFrom>, new()
        {
            if (dictionary.TryGetValue(typeof(TMapper), out object tmp))
            {
                value = (TMapper)tmp;
                return true;
            }
            value = default(TMapper);
            return false;
        }
    }
}
