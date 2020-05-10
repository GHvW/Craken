﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Craken {
    public static class Parse {

        public static Parser<string, char> Item() =>
            new Parser<string, char>((input) =>
                input.Any()
                    ? new List<(char, string)>() { (input[0], input[1..]) }
                    : new List<(char, string)>());

        public static Parser<string, A> Zero<A>() =>
            new Parser<string, A>((input) => new List<(A, string)>());

        public static Parser<string, A> Result<A>(A item) =>
            new Parser<string, A>((input) => new List<(A, string)>() { (item, input) });

        public static Parser<string, char> Satisfy<A>(Func<char, bool> predicate) =>
            Item().SelectMany(c => predicate(c)
                                       ? Result(c)
                                       : Zero<char>());
    }
}

