using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NestedListParser
{
    public class ParseResult
    {
        public string Value { get; set; }
        public int Level { get; set; }

        public override string ToString()
        {
            var listOfHyphens = Enumerable.Range(0, Level).Select((x) => "-");
            return $"{string.Join(string.Empty, listOfHyphens)} {Value}";
        }
    }

    public class NestedListParser
    {
        private readonly Regex _openParen = new Regex("^\\(", RegexOptions.None);
        private readonly Regex _wordRegex = new Regex(@"^(,?\s*)(?<value>\w+)(,?)", RegexOptions.None);
        private readonly Regex _closeParen = new Regex("^\\)", RegexOptions.None);

        private bool Matches(Regex regex, string toCheck)
        {
            var matches = regex.Match(toCheck);
            return matches.Success;
        }

        private string GetValue(string toCheck)
        {
            var match = _wordRegex.Match(toCheck);
            return match.Groups["value"].Value;
        }

        public List<ParseResult> Parse(string toParse, int level = -1)
        {
            var toReturn = new List<ParseResult>();

            if (Matches(_openParen, toParse))
            {
                var trimmed = _openParen.Replace(toParse, string.Empty);
                toReturn.AddRange(Parse(trimmed, level + 1));
            }

            if (Matches(_wordRegex, toParse))
            {
                var value = GetValue(toParse);
                var trimmed = _wordRegex.Replace(toParse, string.Empty);
                toReturn.Add(new ParseResult {
                    Value = value,
                    Level = level
                });
                toReturn.AddRange(Parse(trimmed, level));
            }

            if (Matches(_closeParen, toParse))
            {
                var trimmed = _closeParen.Replace(toParse, string.Empty);
                toReturn.AddRange(Parse(trimmed, level - 1));
            }


            return toReturn;
        } 
    }
}
