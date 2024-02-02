using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication4.Extensions
{
    public static class Linq
    {



        /// <summary>
        ///   This method extends the LINQ methods to flatten a collection of 
        ///   items that have a property of children of the same type.
        /// </summary>
        /// <typeparam name = "T">Item type.</typeparam>
        /// <param name = "source">Source collection.</param>
        /// <param name = "childPropertySelector">
        ///   Child property selector delegate of each item. 
        ///   IEnumerable'T' childPropertySelector(T itemBeingFlattened)
        /// </param>
        /// <returns>Returns a one level list of elements of type T.</returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childPropertySelector)
        {
            return source
                .Flatten((itemBeingFlattened, objectsBeingFlattened) =>
                         childPropertySelector(itemBeingFlattened));
        }

        /// <summary>
        ///   This method extends the LINQ methods to flatten a collection of 
        ///   items that have a property of children of the same type.
        /// </summary>
        /// <typeparam name = "T">Item type.</typeparam>
        /// <param name = "source">Source collection.</param>
        /// <param name = "childPropertySelector">
        ///   Child property selector delegate of each item. 
        ///   IEnumerable'T' childPropertySelector
        ///   (T itemBeingFlattened, IEnumerable'T' objectsBeingFlattened)
        /// </param>
        /// <returns>Returns a one level list of elements of type T.</returns>
        public static IEnumerable<T> Flatten<T>(this IEnumerable<T> source,
            Func<T, IEnumerable<T>, IEnumerable<T>> childPropertySelector)
        {
            return source
                .Concat(source
                            .Where(item => childPropertySelector(item, source) != null)
                            .SelectMany(itemBeingFlattened =>
                                        childPropertySelector(itemBeingFlattened, source)
                                            .Flatten(childPropertySelector)));
        }



        public static IEnumerable<T> Flatten1<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childrenselector)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (childrenselector == null)
                throw new ArgumentNullException(nameof(childrenselector));

            return flatteniterator(source, childrenselector);
        }



        private static IEnumerable<T> flatteniterator<T>(this IEnumerable<T> source,Func<T, IEnumerable<T>> childrenselector)
        {
            if (source != null)
            {
                foreach (var item in source)
                {
                    yield return item;
                    var children = childrenselector(item);
                    if (children != null)
                        foreach (var child in flatteniterator(children, childrenselector))
                            yield return child;
                }
            }
        }




 


        public static IEnumerable<T> Flatten3<T>(this IEnumerable<T> sequence, Func<T, IEnumerable<T>> childFetcher)
        {
            var itemsToYield = new Queue<T>(sequence);
            while (itemsToYield.Count > 0)
            {
                var item = itemsToYield.Dequeue();
                yield return item;

                var children = childFetcher(item);
                if (children != null)
                {
                    foreach (var child in children)
                    {
                        itemsToYield.Enqueue(child);
                    }
                }
            }
        }
    }

    // 遍历的类型
    public enum TraverseType
    {
        PostOrder,
        PreOrder
    }
 }
