using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace Craken.Tests {

    public class ParseTests {

        [Fact]
        public void Item_Test() {

            var result = Parse.Item().Call("Hello World!");

            Assert.Equal('H', result.First().Item1);
            Assert.Equal("ello World!", result.First().Item2);
        }

        [Fact]
        public void Satisfy_Test() {
            var parser = Parse.Satisfy((item) => item == 'H');

            var lowerHResult = parser.Call("hello World!");
            var upperHResult = parser.Call("Hello World!");

            Assert.True(lowerHResult.Count() == 0); // should be empty List

            Assert.Equal('H', upperHResult.First().Item1);
            Assert.Equal("ello World!", upperHResult.First().Item2);
        }

        [Fact]
        public void Upper_Test() {

            var result = Parse.Upper().Call("Hello World!");
            var badResult = Parse.Upper().Call("hello World!");

            Assert.True(badResult.Count() == 0);
            Assert.Equal('H', result.First().Item1);
            Assert.Equal("ello World!", result.First().Item2);
        }

        [Fact]
        public void Lower_Test() {

            var result = Parse.Lower().Call("hello World!");
            var badResult = Parse.Lower().Call("Hello World!");

            Assert.True(badResult.Count() == 0);
            Assert.Equal('h', result.First().Item1);
            Assert.Equal("ello World!", result.First().Item2);
        }

        [Fact]
        public void Word_Test() {

            var result = Parse.Word().Call("Hello World!");

            Assert.Equal("Hello", result.First().Item1);
            // TODO Complete this Test
        }

        [Fact]
        public void Many_Test() {

            var result = Parse.Many(Parse.Letter()).Call("Hello World!");

            Assert.Equal("Hello", result.First().Item1);
            // TODO Complete this Test
        }

        [Fact]
        public void Str_Test() {

            var result = Parse.Str("Hello").Call("Hello World!");

            Assert.Equal("Hello", result.First().Item1);
            Assert.Equal(" World!", result.First().Item2);
        }

        [Fact]
        public void Many1_Test() {

            var result = Parse.Many1(Parse.Char('a')).Call("aaab");
            var failure = Parse.Many1(Parse.Char('a')).Call("baaa");

            Assert.Equal("aaa", result.First().Item1);

            Assert.Equal(Enumerable.Empty<(string, string)>(), failure);
            // TODO Complete this Test
        }

        [Fact]
        public void Natural_Test() {

            var result = Parse.Natural().Call("100");
            var result2 = Parse.Natural().Call("100a");
            var failure1 = Parse.Natural().Call("a100");
            var failure2 = Parse.Natural().Call("-100");

            Assert.Equal(100, result.First().Item1);
            Assert.Equal(100, result2.First().Item1);

            Assert.Equal(Enumerable.Empty<(int, string)>(), failure1);
            Assert.Equal(Enumerable.Empty<(int, string)>(), failure2);
        }

        [Fact]
        public void Int_Test() {

            var result = Parse.Int().Call("100");
            var result2 = Parse.Int().Call("100a");
            var result3 = Parse.Int().Call("-100");
            var failure = Parse.Int().Call("a100");

            Assert.Equal(100, result.First().Item1);
            Assert.Equal(100, result2.First().Item1);
            Assert.Equal(-100, result3.First().Item1);

            Assert.Equal(Enumerable.Empty<(int, string)>(), failure);
        }

        [Fact]
        public void Identifier_Test() {

            var result = Parse.Identifier().Call("a100X");
            var result2 = Parse.Identifier().Call("aHelloWorld thing");
            var failure1 = Parse.Identifier().Call("100");
            var failure2 = Parse.Identifier().Call("AHelloWorld");

            Assert.Equal("a100X", result.First().Item1);
            Assert.Equal("aHelloWorld", result2.First().Item1);

            Assert.Equal(Enumerable.Empty<(string, string)>(), failure1);
            Assert.Equal(Enumerable.Empty<(string, string)>(), failure2);
        }
    }
}
