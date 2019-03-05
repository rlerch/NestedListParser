using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NestedListParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var toParse = "(id,created,employee(id,firstname,employeeType(id), lastname),location)";
            var parser = new NestedListParser();
            var parseResult = parser.Parse(toParse);

            parseResult.Print();
            //Keep window open
            Console.Read();
        }
    }
}
