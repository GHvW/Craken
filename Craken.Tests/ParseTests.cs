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
    }
}
