using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace Craken.Tests {

    public class ParserTests {

        [Fact]
        public void Query_Test() {

            var result = (from x in Parse.Item()
                          from y in Parse.Item()
                          from z in Parse.Item()
                          select x.ToString() + y.ToString() + z.ToString())
                         .Call("Hello World!");

            Assert.Equal("Hel", result.First().Item1);
            Assert.Equal("lo World!", result.First().Item2);
        }
    }
}
