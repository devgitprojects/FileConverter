using CommonFileConverter.Models;
using System;
using System.Collections.Generic;

namespace XmlBinFileConverter.Models
{
    [Serializable]
    public class CarsCollection<T> : SerializableKeyedCollection<string, T>
        where T : XmlCar
    {
        public CarsCollection() : base() { }
        public CarsCollection(IEnumerable<T> enumerable) : base(enumerable) { }

        protected override string GetKeyForItem(T item)
        {
            return item.BrandName;
        }      
    }
}
