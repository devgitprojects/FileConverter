using CommonFileConverter.Extensions;
using CommonFileConverter.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using XmlBinFileConverter.Constants;

namespace XmlBinFileConverter.Models
{
    [Serializable]
    public abstract class BaseCarsFile<TCar> : BaseFileStructure
        where TCar : XmlCar
    {
        public BaseCarsFile() { }
        public BaseCarsFile(CarsCollection<TCar> cars)
        {
            cars.ThrowArgumentNullExceptionIfNull();
            Cars = cars;
        }

        [XmlElement(XmlBinMessages.FileStructureFields.Car)]
        [Required(ErrorMessage = XmlBinMessages.CollectionShouldContainZeroOrMoreItems + XmlBinMessages.FileStructureFields.Cars)]
        public virtual CarsCollection<TCar> Cars { get; set; }

        public override void Validate()
        {
            base.Validate();
            Cars.ThrowArgumentNullExceptionIfNull();
            Cars.Validate();
        }
    }
}
