using FileConverter.Constants;
using FileConverter.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace FileConverter.Models
{
    [Serializable]
    [XmlRoot("Document")]
    public class XmlBasedFileStructure : BaseFileStructure
    {
        public XmlBasedFileStructure() { }
        public XmlBasedFileStructure(CarsCollection<XmlCar> cars)
        {
            cars.ThrowArgumentNullExceptionIfNull();
            Cars = cars;
        }

        [XmlElement("Car")]
        [Required(ErrorMessage = LogMessages.CollectionShouldContainZeroOrMoreItems + "Cars")]
        public CarsCollection<XmlCar> Cars { get; set; }

        public override void Validate()
        {
            base.Validate();
            this.Cars.Validate();
        }
    }
}
