using CommonFileConverter.Interfaces;
using System.IO;
using XmlBinFileConverter.Models;

namespace FileConverterTests.ConvertersMocks
{
    internal class SerializerMock<TFile, TCar> : ISerializer<TFile> 
        where TFile : BaseCarsFile<TCar>, new()
        where TCar : XmlCar, new()
    {
        public TFile Deserialize(Stream serializationStream)
        {
            return TestingHelper.CreateCarFile<TFile, TCar>();             
        }

        public void Serialize(Stream serializationStream, TFile graph)
        {
            graph.Validate();
        }
    }
}
