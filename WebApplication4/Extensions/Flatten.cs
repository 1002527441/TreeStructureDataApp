using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication4.Extensions
{
    public static class Linq
    {

        public static IEnumerable<T> Flatten1<T>(this T source, Func<T, IEnumerable<T>> selector)
        {
            return selector(source).SelectMany(c => Flatten1(c, selector)).Concat(new[] { source });
        }

        public static IEnumerable<T> Flatten2<T>(this T source, Func<T, IEnumerable<T>> selector)
        {
            var stack = new Stack<T>();
            stack.Push(source);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                yield return current;
                foreach (var child in selector(current))
                    stack.Push(child);
            }
        }
    }
 }
