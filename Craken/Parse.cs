using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craken {

    // consider making these static on the parser class?
    public static partial class Parse {

        public static Parser<string, char> Item() =>
            new Parser<string, char>((input) =>
                input.Any()
                    ? new List<(char, string)>() { (input[0], input[1..]) }
                    : Enumerable.Empty<(char, string)>());

        public static Parser<In, A> Zero<In, A>() =>
            //new Parser<In, A>((input) => new List<(A, In)>());
            new Parser<In, A>((input) => Enumerable.Empty<(A, In)>());

        public static Parser<In, A> Result<In, A>(A item) =>
            new Parser<In, A>((input) => new List<(A, In)>() { (item, input) });

        public static Parser<string, char> Satisfy(Func<char, bool> predicate) =>
            Item().SelectMany(c => predicate(c)
                                       ? Result<string, char>(c)
                                       : Zero<string, char>());

        public static Parser<string, char> Char(char c) => Satisfy(item => item == c);

        public static Parser<string, char> Digit() => Satisfy(c => c >= '0' && c <= '9');

        public static Parser<string, char> Upper() => Satisfy(c => c >= 'A' && c <= 'Z');

        public static Parser<string, char> Lower() => Satisfy(c => c >= 'a' && c <= 'z');

        public static Parser<string, char> Letter() => Lower().Plus(Upper());

        public static Parser<string, char> AlphaNumeric() => Digit().Plus(Letter());

        public static Parser<string, string> Str(string str) => str switch {
            "" => Result<string, string>(""),
            string s => (from _x in Char(s[0])
                         from _xs in Str(s[1..])
                         select s),
        };

        // Plus on empty string is like a base case for the recursive call to Word.
        // It is kind of like saying "Letter() ? EmptyString : theLetter + Word()"
        // The difference being that, as said in the paper, it's non-deterministic
        // so it will return results for intermediate results for each call to Word
        //public static Parser<string, string> Word() =>
        //    (from x in Letter()
        //     from xs in Word()
        //     select xs.Insert(0, x.ToString())) // TODO - better way to handle this?
        //    .Plus(Result<string, string>(""));

        //public static Parser<string, string> Many(Parser<string, char> parser) =>
        //    (from x in parser
        //     from xs in Many(parser)
        //     select xs.Insert(0, x.ToString())) // TODO - better way to handle this?
        //    .Plus(Result<string, string>(""));

        public static Parser<string, IEnumerable<A>> Many<A>(Parser<string, A> parser) =>
            (from x in parser
             from xs in Many(parser)
             select xs.Prepend(x))
            .Plus(Result<string, IEnumerable<A>>(Enumerable.Empty<A>()));

        // use in place of Word for non-strings?
        public static Parser<IEnumerable<A>, IEnumerable<A>> Many<A>(Parser<IEnumerable<A>, A> parser) =>
            (from x in parser
             from xs in Many(parser)
             select xs.Prepend(x))
            .Plus(Zero<IEnumerable<A>, IEnumerable<A>>());

        //public static Parser<string, string> Word() => Many(Letter());
        public static Parser<string, IEnumerable<char>> Word() => Many(Letter());

        //public static Parser<string, string> Many1(Parser<string, char> parser) =>
        //    (from x in parser
        //     from xs in Many(parser)
        //     select xs.Insert(0, x.ToString()));

        public static Parser<string, IEnumerable<A>> Many1<A>(Parser<string, A> parser) =>
            (from x in parser
             from xs in Many(parser)
             select xs.Prepend(x));


        //public static Parser<string, string> Identifier() =>
        //    (from x in Lower()
        //     from xs in Many(AlphaNumeric())
        //     select xs.Insert(0, x.ToString()));

        public static Parser<string, IEnumerable<char>> Identifier() =>
            (from x in Lower()
             from xs in Many(AlphaNumeric())
             select xs.Prepend(x));

        // c - '0' gets the number between 9 and 0 inclusive without needing to parse the int
        // 10 * acc + c shifts the digit on each iteration so that 8 + 7 + 1 + 5 = 8715 instead of 21
        //private static int Eval(string str) => str.Aggregate(0, (acc, c) => 10 * acc + (c - '0'));
        public static Parser<string, int> Natural() =>
            Many1(Digit()).Select(/*eval*/(str) => str.Aggregate(0, (acc, c) => 10 * acc + (c - '0')));

        //public static Parser<string, int> Natural() {
        //    return Many1(Digit()).Select(Eval);
        //    static int Eval(string str) => str.Aggregate(0, (acc, c) => 10 * acc + (c - '0'));
        //}

        // not the "elegant" solution but I dont think we have a negate function in C# for int
        public static Parser<string, int> Int() =>
            (from _x in Char('-')
             from n in Natural()
             select -n)
            .Plus(Natural());
    }

    public static partial class ParserExtensions {

        public static Parser<string, IEnumerable<A>> SepBy1<A, B>(this Parser<string, A> parser, Parser<string, B> separator) =>
            (from x in parser
             from xs in Parse.Many(from _s in separator
                                   from y in parser
                                   select y)
             select xs.Prepend(x));

        // TODO - curry this?
        public static Parser<string, B> Bracket<A, B, C>(this Parser<string, B> parser, 
                                                              Parser<string, A> open, 
                                                              Parser<string, C> close) =>
            (from _open in open
             from x in parser
             from _close in close
             select x);

        public static Parser<string, IEnumerable<A>> SepBy<A, B>(this Parser<string, A> parser, Parser<string, B> separator) =>
            parser.SepBy1(separator).Plus(Parse.Zero<string, IEnumerable<A>>());
    }

    public static partial class Parse {

        public static Parser<byte[], byte> ByteItem() =>
            new Parser<byte[], byte>((input) =>
                input.Any()
                    ? new List<(byte, byte[])>() { (input[0], input[1..]) }
                    : Enumerable.Empty<(byte, byte[])>());

        // TODO - figure something out here
        public static Parser<byte[], int> Int32() =>
            (from a in ByteItem()
             from b in ByteItem()
             from c in ByteItem()
             from d in ByteItem()
             select BitConverter.ToInt32(new byte[] {a, b, c, d}, 0));
    }
}


