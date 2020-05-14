using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craken {
    public static class Parse {

        public static Parser<string, char> Item() =>
            new Parser<string, char>((input) =>
                input.Any()
                    ? new List<(char, string)>() { (input[0], input[1..]) }
                    : Enumerable.Empty<(char, string)>());

        public static Parser<In, A> Zero<In, A>() =>
            new Parser<In, A>((input) => new List<(A, In)>());

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
                //string s => Char(s[0])
                //                .SelectMany(_ =>
                //                    Str(s[1..])
                //                        .SelectMany(_ => Result<string, string>(s))),
                string s => (from _a in Char(s[0])
                             from _b in Str(s[1..])
                             select s),
            };

        public static Parser<string, string> Word() =>
            //Letter()
            //    .SelectMany(x => 
            //        Word()
            //            .SelectMany(xs => 
            //                Result<string, string>(xs.Insert(0, x.ToString())))); // TODO - do something better here
            from x in Letter()
            from xs in Word()
            select xs.Insert(0, x.ToString()); // fix this
    }
}

