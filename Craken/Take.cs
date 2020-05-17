using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Craken {
    public static class Take {

        public static IEnumerable<(char, string)> OneChar(string input) => 
            input.Any() 
                ? new List<(char, string)>() { (input[0], input[1..]) } 
                : Enumerable.Empty<(char, string)>();

        public static IEnumerable<(byte, byte[])> OneByte(byte[] input) =>
            input.Any() 
                ? new List<(byte, byte[])>() { (input[0], input[1..]) } 
                : Enumerable.Empty<(byte, byte[])>();
    }
}
