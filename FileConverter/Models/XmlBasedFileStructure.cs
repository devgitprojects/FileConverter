using FileConverter.Constants;
using FileConverter.Extensions;
using System;
using System.Collections.Generic;
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
        public XmlBasedFileStructure(LinkedList<Car> fileContent)
        {
            fileContent.ThrowArgumentNullExceptionIfNull();
            Cars = fileContent;
        }
        protected XmlBasedFileStructure(SerializationInfo info, StreamingContext context)
        {
            Cars = (LinkedList<Car>)info.GetValue("Cars", typeof(LinkedList<Car>));
        }

        [XmlElement("Car")]
        [Range(0, int.MaxValue, ErrorMessage = LogMessages.CollectionShouldContainZeroOrMoreItems + "Cars")]
        public LinkedList<Car> Cars { get; set; }         

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            this.Validate();
            info.AddValue("Cars", this.Cars, typeof(LinkedList<Car>));
        }
    }
}
