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

            //foreach (var parseResult in parseResults.OrderBy(x => x.Value).ThenBy(x => x.Level))
            //{
            //    Console.WriteLine(parseResult);
            //}
            //Keep window open
            parseResult.Print();
            Console.Read();
        }
    }
}
