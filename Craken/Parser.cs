using System;
using System.Collections.Generic;

namespace Craken {

    public struct ParseState<A> {
        public readonly A data;
        public readonly IEnumerable<A> input;

        public ParseState(A data, IEnumerable<A> input) {
            this.data = data;
            this.input = input;
        }
    }

    public class Parser<A> {

        Func<IEnumerable<A>, ParseState<A>> fn;

        public Parser(Func<IEnumerable<A>, ParseState<A>> fn) {
            this.fn = fn;
        }

        public ParseState<A> apply(IEnumerable<A> input) {
            return this.fn(input);
        }
    }
}