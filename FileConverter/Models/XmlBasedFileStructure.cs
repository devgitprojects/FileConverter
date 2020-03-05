using FileConverter.Constants;
using FileConverter.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace FileConverter.Models
{
    [Serializable]
    [XmlRoot("Document")]
    public class XmlBasedFileStructure : BaseFileStructure, ISerializable
    {
        public XmlBasedFileStructure() { }
        public XmlBasedFileStructure(Cars cars)
        {
            cars.ThrowArgumentNullExceptionIfNull();
            Cars = cars;
        }
        protected XmlBasedFileStructure(SerializationInfo info, StreamingContext context)
        {
            Cars = (Cars)info.GetValue("Cars", typeof(Cars));
        }

        [XmlElement("Car")]
        [Required(ErrorMessage = LogMessages.CollectionShouldContainZeroOrMoreItems + "Cars")]
        public Cars Cars { get; set; }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Cars", this.Cars, typeof(Cars));
        }

        public override void Validate()
        {
            base.Validate();
            this.Cars.Validate();
        }
    }
}
