using System;
using System.Collections.Generic;
using System.Linq;

namespace Craken {

    public delegate IEnumerable<(Out, In)> Parser<In, Out>(In input);

    public static partial class ParserExtensions {
        
        public static Parser<In, Out> Id<In, Out>(this Parser<In, Out> it) => it;

        public static Parser<In, Result> Select<In, Out, Result>(this Parser<In, Out> parse, Func<Out, Result> transform) =>
            (input) => parse(input).Select(state => (transform(state.Item1), state.Item2));

        // Monad Bind
        public static Parser<In, Result> SelectMany<In, Out, Result>(this Parser<In, Out> parse, Func<Out, Parser<In, Result>> transform) =>
            (input) => parse(input).SelectMany(state => transform(state.Item1)(state.Item2));

        // this is to enable the query expression syntax.
        // In the paper, it's mentioned (p. 12) that all combinators could be done using the `do` syntax instead of the builder expression syntax
        // that is what I'm hoping to enable here
        public static Parser<In, Result> SelectMany<In, Out, A, Result>(this Parser<In, Out> parse, Func<Out, Parser<In, A>> transform, Func<Out, A, Result> selection) =>
            (input) => parse(input)
                           .SelectMany(state => 
                                transform(state.Item1)(state.Item2)
                                    .SelectMany(result => Parse.Result<In, Result>(selection(state.Item1, result.Item1))(result.Item2)));

        // I originally thought this was the same as Or, but its not!!
        // MonadPlus Plus
        public static Parser<In, Out> Plus<In, Out>(this Parser<In, Out> parse, Parser<In, Out> parser) =>
            (input) => parse(input).Concat(parser(input));
    }

    //public class Parser<In, Out> {

    //    private readonly Func<In, IEnumerable<(Out, In)>> parse;

    //    public Parser(Func<In, IEnumerable<(Out, In)>> parse) {
    //        this.parse = parse;
    //    }

    //    public IEnumerable<(Out, In)> Call(In input) {
    //        return this.parse(input);
    //    }

    //    public Parser<In, Out> Id() => this;

    //    public Parser<In, Result> Select<Result>(Func<Out, Result> transform) => 
    //        new Parser<In, Result>((input) => 
    //            this.parse(input)
    //                .Select(state => (transform(state.Item1), state.Item2)));


    //    // Monad Bind
    //    public Parser<In, Result> SelectMany<Result>(Func<Out, Parser<In, Result>> transform) =>
    //        new Parser<In, Result>((input) => 
    //            this.parse(input)
    //                .SelectMany(state => transform(state.Item1).Call(state.Item2)));

    //    // this is to enable the query expression syntax.
    //    // In the paper, it's mentioned (p. 12) that all combinators could be done using the `do` syntax instead of the builder expression syntax
    //    // that is what I'm hoping to enable here
    //    public Parser<In, Result> SelectMany<A, Result>(Func<Out, Parser<In, A>> transform, Func<Out, A, Result> selection) =>
    //        new Parser<In, Result>((input) => 
    //            this.parse(input) 
    //                .SelectMany(state => 
    //                    transform(state.Item1).Call(state.Item2)
    //                        .SelectMany(result => Parse.Result<In, Result>(selection(state.Item1, result.Item1)).Call(result.Item2))));

    //    // I originally thought this was the same as Or, but its not!!
    //    // MonadPlus Plus
    //    public Parser<In, Out> Plus(Parser<In, Out> parser) =>
    //        new Parser<In, Out>((input) => 
    //            this.parse(input)
    //                .Concat(parser.parse(input)));
    //}
}