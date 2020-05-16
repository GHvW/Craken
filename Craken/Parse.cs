using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craken {

    // consider making these static on the parser class?
    public static class Parse {

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
                ""       => Result<string, string>(""),
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

        public static Parser<string, string> Many(Parser<string, char> parser) =>
            (from x in parser
             from xs in Many(parser)
             select xs.Insert(0, x.ToString())) // TODO - better way to handle this?
            .Plus(Result<string, string>(""));


        // use in place of Word for non-strings?
        public static Parser<IEnumerable<A>, IEnumerable<A>> Many<A>(Parser<IEnumerable<A>, A> parser) =>
            (from x in parser
             from xs in Many(parser)
             select xs.Prepend(x))
            .Plus(Zero<IEnumerable<A>, IEnumerable<A>>());

        public static Parser<string, string> Word() => Many(Letter());

        public static Parser<string, string> Many1(Parser<string, char> parser) =>
            (from x in parser
             from xs in Many(parser)
             select xs.Insert(0, x.ToString()));

        // c - '0' gets the number between 9 and 0 inclusive without needing to parse the int
        // 10 * acc + c shifts the digit on each iteration so that 8 + 7 + 1 + 5 = 8715 instead of 21
        //private static int Eval(string str) => str.Aggregate(0, (acc, c) => 10 * acc + (c - '0'));
        public static Parser<string, int> Natural() => 
            Many1(Digit()).Select(/*eval*/(str) => str.Aggregate(0, (acc, c) => 10 * acc + (c - '0')));
    }
}

