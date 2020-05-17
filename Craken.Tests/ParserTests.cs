using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Xunit;

namespace Craken.Tests {

    public class ParserTests {

        [Fact]
        public void Query_Test() {

            var result = (from x in Parse.Item<string, char>(Take.OneChar)
                          from y in Parse.Item<string, char>(Take.OneChar)
                          from z in Parse.Item<string, char>(Take.OneChar)
                          select x.ToString() + y.ToString() + z.ToString())
                         ("Hello World!");

            Assert.Equal("Hel", result.First().Item1);
            Assert.Equal("lo World!", result.First().Item2);
        }
    }
}
