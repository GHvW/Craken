using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace Craken.Tests {

    public class ParseTests {

        [Fact]
        public void Item_Test() {

            var result = Parse.Item<string, char>(Take.OneChar)("Hello World!");

            Assert.Equal('H', result.First().Item1);
            Assert.Equal("ello World!", result.First().Item2);
        }

        [Fact]
        public void Satisfy_Test() {
            var parser = Parse.Satisfy((char item) => item == 'H');

            var lowerHResult = parser("hello World!");
            var upperHResult = parser("Hello World!");

            Assert.True(lowerHResult.Count() == 0); // should be empty List

            Assert.Equal('H', upperHResult.First().Item1);
            Assert.Equal("ello World!", upperHResult.First().Item2);
        }

        [Fact]
        public void Upper_Test() {

            var result = Parse.Upper()("Hello World!");
            var badResult = Parse.Upper()("hello World!");

            Assert.True(badResult.Count() == 0);
            Assert.Equal('H', result.First().Item1);
            Assert.Equal("ello World!", result.First().Item2);
        }

        [Fact]
        public void Lower_Test() {

            var result = Parse.Lower()("hello World!");
            var badResult = Parse.Lower()("Hello World!");

            Assert.True(badResult.Count() == 0);
            Assert.Equal('h', result.First().Item1);
            Assert.Equal("ello World!", result.First().Item2);
        }

        [Fact]
        public void Word_Test() {

            var result = Parse.Word()("Hello World!");

            Assert.Equal("Hello", result.First().Item1);
            // TODO Complete this Test
        }

        [Fact]
        public void Many_Test() {

            var result = Parse.Many(Parse.Letter())("Hello World!");

            Assert.Equal("Hello", result.First().Item1);
            // TODO Complete this Test
        }

        [Fact]
        public void Str_Test() {

            var result = Parse.Str("Hello")("Hello World!");

            Assert.Equal("Hello", result.First().Item1);
            Assert.Equal(" World!", result.First().Item2);
        }

        [Fact]
        public void Many1_Test() {

            var result = Parse.Many1(Parse.Char('a'))("aaab");
            var failure = Parse.Many1(Parse.Char('a'))("baaa");

            Assert.Equal("aaa", result.First().Item1);

            Assert.Equal(Enumerable.Empty<(IEnumerable<char>, string)>(), failure);
            // TODO Complete this Test
        }

        [Fact]
        public void Natural_Test() {

            var result = Parse.Natural()("100");
            var result2 = Parse.Natural()("100a");
            var failure1 = Parse.Natural()("a100");
            var failure2 = Parse.Natural()("-100");

            Assert.Equal(100, result.First().Item1);
            Assert.Equal(100, result2.First().Item1);

            Assert.Equal(Enumerable.Empty<(int, string)>(), failure1);
            Assert.Equal(Enumerable.Empty<(int, string)>(), failure2);
        }

        [Fact]
        public void Int_Test() {

            var result = Parse.Int()("100");
            var result2 = Parse.Int()("100a");
            var result3 = Parse.Int()("-100");
            var failure = Parse.Int()("a100");

            Assert.Equal(100, result.First().Item1);
            Assert.Equal(100, result2.First().Item1);
            Assert.Equal(-100, result3.First().Item1);

            Assert.Equal(Enumerable.Empty<(int, string)>(), failure);
        }

        [Fact]
        public void Identifier_Test() {

            var result = Parse.Identifier()("a100X");
            var result2 = Parse.Identifier()("aHelloWorld thing");
            var failure1 = Parse.Identifier()("100");
            var failure2 = Parse.Identifier()("AHelloWorld");

            Assert.Equal("a100X", result.First().Item1);
            Assert.Equal("aHelloWorld", result2.First().Item1);

            Assert.Equal(Enumerable.Empty<(IEnumerable<char>, string)>(), failure1);
            Assert.Equal(Enumerable.Empty<(IEnumerable<char>, string)>(), failure2);
        }

        [Fact]
        public void BracketedBy_Test() {

            var result = 
                Parse.Int().SepBy1(Parse.Char(','))
                    .BracketedBy(Parse.Char('['), Parse.Char(']'))
                    ("[100,200,300]");

            var result2 = 
                Parse.Int().SepBy1(Parse.Char(','))
                    .BracketedBy(Parse.SquareBrackets())
                    ("[100,200,300]");

            var result3 =
                Parse.Int()
                     .SepBy1(Parse.Char(','))
                     .BracketedBy(Parse.SquareBrackets())
                     .SepBy1(Parse.Char(','))
                     .BracketedBy(Parse.SquareBrackets())("[[100,200],[300,400]]");

            Assert.Equal(100, result.First().Item1.First());
            Assert.Equal(100, result2.First().Item1.First());
            // TODO - Complete this
        }

        //[Fact]
        //public void ChainLeft1_Test() {

        //    var result = Parse.Chain

        //    Assert.Equal(100, result.First().Item1.First());
        //    // TODO - Complete this
        //}

        //[Fact]
        //public void AddOp_Test() {

        //    var result = Parse.AddOp(/);

        //    //Assert.Equal(10, result);
        //    Assert.True(true);
        //    // TODO - Complete this
        //}

        [Fact]
        public void ArithmeticExpression_Test() {

            //var result = Parse.Expr()("10+2+3+4");
            //var result = Parse.Expr()("10+(2-1)");
            var result = Parse.Expr()("10");
            var result2 = Parse.Expr()("10+20+3+4");

            Assert.Equal(19, result.First().Item1);
            Assert.Equal(37, result2.First().Item1);
            // TODO - Complete this
        }
    }
}
