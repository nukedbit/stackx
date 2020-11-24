using System.Collections.Generic;

namespace StackX.ServiceInterface
{
    public static class HashSetExtensions
    {
        public static void AddRange<T>(this HashSet<T> hashSet, IEnumerable<T> range)
        {
            foreach (T t in range)
            {
                hashSet.Add(t);
            }
        }
    }
}