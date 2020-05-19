using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Craken {
    public static class IEnumerableExtensions {

        // simulate foldr 
        public static A AggregateRight<A>(this IEnumerable<A> enumerable, Func<A, A, A> fn) {

            var stack = new Stack<A>();
            foreach(var item in enumerable) {
                stack.Push(item);
            }

            return stack.Aggregate((result, item) => fn(result, item));
        }

        public static A AggregateR<A>(this IEnumerable<A> enumerable, Func<A, A, A> fn) {
            var it = enumerable.GetEnumerator();

            return Agg(it, fn);

            static A Agg(IEnumerator<A> enumerator, Func<A, A, A> fn) {
                var previous = enumerator.Current;
                return !enumerator.MoveNext()
                    ? previous // this might be a bug
                    : fn(enumerator.Current, Agg(enumerator, fn));
            }
        }
    }
}
