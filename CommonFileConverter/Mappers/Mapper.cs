﻿using CommonFileConverter.Constants;
using CommonFileConverter.Interfaces;
using CommonFileConverter.Models;
using System;
using System.IO;

namespace CommonFileConverter.Mappers
{
    /// <summary>
    /// Provides mapping of fields between <typeparamref name="TFrom"/> and <typeparamref name="TTo"/> classes
    /// </summary>
    /// <typeparam name="TFrom"></typeparam>
    /// <typeparam name="TTo"></typeparam>
    public class Mapper<TFrom, TTo>
        where TFrom : BaseFileStructure
        where TTo : IInitializable<TFrom>, new()
    {
        public Mapper() { }
        public Mapper(string convertTofileExtension)
        {
            if (!Path.HasExtension(convertTofileExtension))
            {
                throw new ArgumentException(String.Format(LogMessages.FileExtensionIsNotValid, convertTofileExtension));
            }
            ConvertedFileExtension = convertTofileExtension;
        }

        public string ConvertedFileExtension { get; private set; }

        public virtual TTo Convert(TFrom from)
        {
            var converted = new TTo();
            converted.Initialize(from);
            return converted;
        }
    }

}
