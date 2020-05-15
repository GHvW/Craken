using System;
using System.Collections.Generic;
using System.Linq;

namespace Craken {

    public class Parser<In, Out> {

        private readonly Func<In, IEnumerable<(Out, In)>> parse;

        public Parser(Func<In, IEnumerable<(Out, In)>> parse) {
            this.parse = parse;
        }

        public IEnumerable<(Out, In)> Call(In input) {
            return this.parse(input);
        }

        public Parser<In, Result> Select<Result>(Func<Out, Result> transform) {
            throw new NotImplementedException();
        }

        // Monad Bind
        public Parser<In, Result> SelectMany<Result>(Func<Out, Parser<In, Result>> transform) =>
            new Parser<In, Result>((input) => 
                this.parse(input)
                    .SelectMany(state => transform(state.Item1).Call(state.Item2)));

        // this is to enable the query expression syntax.
        // In the paper, it's mentioned (p. 12) that all combinators could be done using the `do` syntax instead of the builder expression syntax
        // that is what I'm hoping to enable here
        public Parser<In, Result> SelectMany<A, Result>(Func<Out, Parser<In, A>> transform, Func<Out, A, Result> selection) =>
            new Parser<In, Result>((input) => 
                this.parse(input) 
                    .SelectMany(state => 
                        transform(state.Item1).Call(state.Item2)
                            .SelectMany(result => Parse.Result<In, Result>(selection(state.Item1, result.Item1)).Call(result.Item2))));

        // I originally thought this was the same as Or, but its not!!
        // MonadPlus Plus
        public Parser<In, Out> Plus(Parser<In, Out> parser) =>
            new Parser<In, Out>((input) => 
                this.parse(input)
                    .Concat(parser.parse(input)));
    }


    public struct ParseError { }
    //public struct ParseState<A> {
    //    public readonly A data;
    //    public readonly IEnumerable<A> input;

    //    public ParseState(A data, IEnumerable<A> input) {
    //        this.data = data;
    //        this.input = input;
    //    }
    //}
}