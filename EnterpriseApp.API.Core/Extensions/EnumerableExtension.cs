using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EnterpriseApp.API.Core.Extensions
{
    public static class EnumerableExtension
    {
        public static bool IsEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || source.Count() == 0;
        }
    }
}
