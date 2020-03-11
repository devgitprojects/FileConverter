using System;
using System.Collections.Generic;
using System.Linq;

namespace CommonFileConverter.Extensions
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

        public static IEnumerable<TTo> Convert<TFrom, TTo>(this IEnumerable<TFrom> enumerable, Func<TFrom, TTo> constructionFunc)
        {
            return enumerable.Select(constructionFunc);
        }
    }
}
