using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.EntityFrameworkCore
{
    public static class IQueryablePageListExtensions
    {

        public static IEnumerable<T> ToPaged<T>(this IQueryable<T> source, int pageIndex, int pageSize, int indexFrom = 0, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (indexFrom > pageIndex)
            {
                throw new ArgumentException($"indexFrom: {indexFrom} > pageIndex: {pageIndex}, must indexFrom <= pageIndex");
            }

            return source.Skip((pageIndex - indexFrom) * pageSize)
                                    .Take(pageSize);
        }
    }
}
