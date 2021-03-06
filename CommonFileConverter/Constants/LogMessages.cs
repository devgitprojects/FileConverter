﻿namespace CommonFileConverter.Constants
{
    public class LogMessages
    {
        public const string ConvertFileException = "Exception occurs during file conversion: {0}, {1} -> {2}";
        public const string CreateFileExceptionText = "Exception occurs during file creating {0}";
        public const string ModifyFileExceptionText = "Exception occurs during file modification {0}";
        public const string ReadFileExceptionText = "Exception occurs during file reading {0}";
        public const string RemoveFileExceptionText = "Exception occurs during file removing {0}";

        public const string ValueShouldBePositive = "Value should be positive: ";
        public const string CollectionShouldContainZeroOrMoreItems = "Collection should contain zero or more items: ";
        public const string ValidationFailed = "Item is not valid: {0}. The following errors occur: {1}";
        public const string StringExceedsMaxLenght = "String exceeds max length: ";
        public const string FileExtensionIsNotValid = "File extension is not valid: {0}";

        public const string ValuesAreNotEqual = "Values are not equal: {0} == {1} and {2} == {3}";
        public const string ValueNotInRange = "Value is not in range: ";

    }
}
