using FileConverter.Constants;
using FileConverter.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace FileConverter.Models
{
    [Serializable]
    public abstract class SerializableKeyedCollection<K, V> : KeyedCollection<K, V> where V : BaseFileStructure
    {
        public SerializableKeyedCollection() : base() { }
        public SerializableKeyedCollection(IEqualityComparer<K> comparer) : base(comparer) { }
        public SerializableKeyedCollection(IEqualityComparer<K> comparer, int dictionaryCreationThreshold) : base(comparer, dictionaryCreationThreshold) { }

        public virtual void Validate()
        {
            var exceptions = new Queue<Exception>();
            foreach (var item in this)
            {
                if (!item.Validate(out List<ValidationResult> errors))
                {
                    exceptions.Enqueue(new ValidationException(
                        String.Format(LogMessages.ValidationFailed, this.GetKeyForItem(item), String.Join(", ", errors.Select(er => er.ErrorMessage).ToArray()))));
                }
                
            }           

            exceptions.ThrowAggregateExceptionIfInnerExceptionPresent();
        }
    }
}
