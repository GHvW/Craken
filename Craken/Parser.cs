using System;
using System.Collections.Generic;
using System.Linq;

namespace Craken {

    public class Parser<In, Out> {

        Func<In, IEnumerable<(Out, In)>> parse;

        public Parser(Func<In, IEnumerable<(Out, In)>> parse) {
            this.parse = parse;
        }

        public IEnumerable<(Out, In)> Call(In input) {
            return this.parse(input);
        }

        public Parser<In, Result> Select<Result>(Func<Out, Result> transform) {
            throw new NotImplementedException();
        }

        public Parser<In, Result> SelectMany<Result>(Func<Out, Parser<In, Result>> transform) =>
            new Parser<In, Result>((input) => 
                this.parse(input)
                    .SelectMany(state => transform(state.Item1).Call(state.Item2)));

        public Parser<In, Result> SelectMany<A, Result>(Func<Out, Parser<In, A>> transform, Func<Out, A, Result> selection) =>
            new Parser<In, Result>((input) => 
                this.parse(input) 
                    .SelectMany(state => 
                        transform(state.Item1).Call(state.Item2)
                            .SelectMany(result => Parse.Result<In, Result>(selection(state.Item1, result.Item1)).Call(result.Item2))));

        // rename this to Or?
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