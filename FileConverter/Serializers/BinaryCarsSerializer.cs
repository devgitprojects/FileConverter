using CommonFileConverter.Extensions;
using CommonFileConverter.Serializers;
using System;
using System.IO;
using System.Runtime.InteropServices;
using XmlBinFileConverter.Models;

namespace XmlBinFileConverter.Serializers
{
    public class BinaryCarsSerializer : BinaryBasedSerializer<BinaryCarsFile>
    {
        public override void Serialize(Stream serializationStream, BinaryCarsFile graph)
        {
            serializationStream.ThrowArgumentNullExceptionIfNull();
            graph.ThrowArgumentNullExceptionIfNull();
            graph.Validate();

            int headerRawSize = Marshal.SizeOf(graph.Header);
            int recordsCountRawSize = Marshal.SizeOf(graph.RecordsCount);
            int carRawSize = Marshal.SizeOf<BinaryCar>();

            byte[] rawData = new byte[headerRawSize + recordsCountRawSize + carRawSize * graph.Cars.Count];

            byte[] headerRawData = new byte[headerRawSize];
            RawSerialize(graph.Header, ref headerRawData);
            Buffer.BlockCopy(headerRawData, 0, rawData, 0, headerRawData.Length);

            byte[] recordsCountRawData = new byte[recordsCountRawSize];
            RawSerialize(graph.RecordsCount, ref recordsCountRawData);
            Buffer.BlockCopy(recordsCountRawData, 0, rawData, headerRawData.Length, recordsCountRawData.Length);

            byte[] carsRawData = new byte[carRawSize * graph.Cars.Count];
            int offset = 0;
            foreach (var car in graph.Cars)
            {
                byte[] carRawData = new byte[carRawSize];
                RawSerialize(car, ref carRawData);
                Buffer.BlockCopy(carRawData, 0, carsRawData, offset, carRawData.Length);
                offset += carRawData.Length;
            }

            Buffer.BlockCopy(carsRawData, 0, rawData, headerRawData.Length + recordsCountRawData.Length, carsRawData.Length);
            serializationStream.Write(rawData, 0, rawData.Length);
        }

        public override BinaryCarsFile Deserialize(Stream serializationStream)
        {
            serializationStream.ThrowArgumentNullExceptionIfNull();
            BinaryReader reader = new BinaryReader(serializationStream);
            BinaryCarsFile file = new BinaryCarsFile();
            CarsCollection<BinaryCar> cars = new CarsCollection<BinaryCar>();

            int headerRawSize = Marshal.SizeOf(typeof(short));
            int recordsCountRawSize = Marshal.SizeOf(typeof(uint));
            int carRawSize = Marshal.SizeOf(typeof(BinaryCar));

            byte[] headerRawData = new byte[headerRawSize];
            reader.Read(headerRawData, 0, headerRawSize);
            file.headerField = (short)RawDeserialize(typeof(short), ref headerRawData);

            byte[] recordsCountRawData = new byte[recordsCountRawSize];
            reader.Read(recordsCountRawData, 0, recordsCountRawSize);
            file.RecordsCount = (uint)RawDeserialize(typeof(uint), ref recordsCountRawData);

            while (reader.PeekChar() > -1)
            {
                byte[] carRawData = new byte[carRawSize];
                reader.Read(carRawData, 0, carRawSize);
                BinaryCar car = (BinaryCar)RawDeserialize(typeof(BinaryCar), ref carRawData);
                cars.Add(car);
            }

            file.Cars = cars;            
            return file;
        }
    }
}
