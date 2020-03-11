using CommonFileConverter.Constants;
using CommonFileConverter.Extensions;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CommonFileConverter.Models
{
    [Serializable]
    public abstract class SerializableKeyedCollection<TKey, TValue> : KeyedCollection<TKey, TValue> where TValue : BaseFileStructure
    {
        public SerializableKeyedCollection() : base() { }
        public SerializableKeyedCollection(IEnumerable<TValue> enumerable) : base()
        {
            AddRange(enumerable);
        }

        public SerializableKeyedCollection(IEqualityComparer<TKey> comparer) : base(comparer) { }
        public SerializableKeyedCollection(IEqualityComparer<TKey> comparer, int dictionaryCreationThreshold) : base(comparer, dictionaryCreationThreshold) { }

        public void AddRange(IEnumerable<TValue> enumerable)
        {
            foreach (var item in enumerable)
            {
                this.Add(item);
            }
        }

        public virtual IEnumerable<TConvertTo> Convert<TConvertTo>(Mapper<TValue, TConvertTo> mapper) 
            where TConvertTo : IInitializable<TValue>, new()
        {
            return this.Convert(x => mapper.Convert(x));
        }

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

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
        }
    }
}
