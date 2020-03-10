using FileConverter.Constants;
using FileConverter.Converters;
using FileConverter.Extensions;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace FileConverter.Models
{
    [Serializable]
    [XmlRoot("Document")]
    public class XmlBasedFileStructure : BaseFileStructure, IInitializable<BinaryBasedFileStructure>/*, IConvertible<XmlBasedFileStructure>*/
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

        void IInitializable<BinaryBasedFileStructure>.Initialize(BinaryBasedFileStructure from)
        {
            from.ThrowArgumentNullExceptionIfNull();
            Cars = new CarsCollection<XmlCar>(from.Cars.Convert(new Mapper<BinaryCar, XmlCar>()));
        }

        //TTo IConvertible<XmlBasedFileStructure>.Convert<TTo>(Mapper<XmlBasedFileStructure, TTo> mapper)
        //{
        //    mapper.ThrowArgumentNullExceptionIfNull();
        //    return mapper.Convert(this);
        //}
    }
}
