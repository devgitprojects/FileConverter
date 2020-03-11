using CommonFileConverter.Constants;
using CommonFileConverter.Extensions;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Mappers;
using CommonFileConverter.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using XmlBinFileConverter.Constants;

namespace XmlBinFileConverter.Models
{
    [Serializable]
    [XmlRoot(XmlBinMessages.FileStructureFields.Document)]
    public class XmlBasedFileStructure : BaseFileStructure, IInitializable<BinaryBasedFileStructure>, IConvertible<XmlBasedFileStructure>
    {
        public XmlBasedFileStructure() { }
        public XmlBasedFileStructure(CarsCollection<XmlCar> cars)
        {
            cars.ThrowArgumentNullExceptionIfNull();
            Cars = cars;
        }

        [XmlElement(XmlBinMessages.FileStructureFields.Car)]
        [Required(ErrorMessage = LogMessages.CollectionShouldContainZeroOrMoreItems + XmlBinMessages.FileStructureFields.Cars)]
        public CarsCollection<XmlCar> Cars { get; set; }

        public override void Validate()
        {
            base.Validate();
            Cars.ThrowArgumentNullExceptionIfNull();
            this.Cars.Validate();
        }

        #region  IConvertible<XmlBasedFileStructure>

        TTo IConvertible<XmlBasedFileStructure>.Convert<TTo>(Mapper<XmlBasedFileStructure, TTo> mapper)
        {
            mapper.ThrowArgumentNullExceptionIfNull();
            return mapper.Convert(this);
        }

        #endregion

        #region  IInitializable<BinaryBasedFileStructure>

        void IInitializable<BinaryBasedFileStructure>.Initialize(BinaryBasedFileStructure from)
        {
            from.ThrowArgumentNullExceptionIfNull();
            Cars = new CarsCollection<XmlCar>(from.Cars.Convert(new Mapper<BinaryCar, XmlCar>()));
        }

        #endregion
    }
}
