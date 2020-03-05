using System;
using System.Collections.Generic;

namespace FileConverter.Extensions
{
    public static class TypeExtensions
    {
        public static void DisposeIfNotNull(this IDisposable toDispose)
        {
            if (toDispose != null)
            {
                toDispose.Dispose();
            }
        }

        public static void ThrowArgumentNullExceptionIfNull(this object objToChek)
        {
            if (objToChek == null)
            {
                throw new ArgumentNullException();
            }
        }

        public static void ThrowAggregateExceptionIfInnerExceptionPresent(this IReadOnlyCollection<Exception> innerExceptions)
        {
            if (innerExceptions != null && innerExceptions.Count > 0)
            {
                throw new AggregateException(innerExceptions);
            }
        }
    }
}
