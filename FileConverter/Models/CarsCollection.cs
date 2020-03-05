using System;

namespace FileConverter.Models
{
    [Serializable]
    public class CarsCollection<V> : SerializableKeyedCollection<string, V> where V : XmlCar
    {
        protected override string GetKeyForItem(V item)
        {
            return item.BrandName;
        }
    }
}
