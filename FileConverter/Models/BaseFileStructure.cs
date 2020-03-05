using FileConverter.Extensions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;

namespace FileConverter.Models
{
    public abstract class BaseFileStructure : ISerializable
    {
        public abstract void GetObjectData(SerializationInfo info, StreamingContext context);
        protected virtual void Validate()
        {
            var errors = new List<ValidationResult>();
            var context = new ValidationContext(this);
            if (!Validator.TryValidateObject(context.ObjectInstance, context, errors, true))
            {
                errors.Select(x => new ValidationException(x.ErrorMessage)).ToList().ThrowAggregateExceptionIfInnerExceptionPresent();
            }
        }
    }
}
