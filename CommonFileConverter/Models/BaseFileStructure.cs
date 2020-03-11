using CommonFileConverter.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CommonFileConverter.Models
{
    [Serializable]
    public abstract class BaseFileStructure
    {
        public virtual void Validate()
        {
            if (!Validate(out List<ValidationResult> errors))
            {
                throw new ValidationException(String.Format(LogMessages.ValidationFailed, String.Empty, String.Join(", ", errors.Select(er => er.ErrorMessage).ToArray())));
            }
        }

        public virtual bool Validate(out List<ValidationResult> errors)
        {
            errors = new List<ValidationResult>();
            var context = new ValidationContext(this);            
            return Validator.TryValidateObject(context.ObjectInstance, context, errors, true); ;
        }
    }
}
