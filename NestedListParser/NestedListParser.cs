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
        public ParseResult Parent { get; set; }
        public List<ParseResult> Children { get; set; }

        public ParseResult()
        {
            Children = new List<ParseResult>();
        }

        public override string ToString()
        {
            var listOfHyphens = Enumerable.Range(0, Level).Select((x) => "-");
            return $"{string.Join(string.Empty, listOfHyphens)} {Value}";
        }

        public void Print()
        {
            foreach(var child in Children.OrderBy(x => x.Value))
            {
                Console.WriteLine($"{child}");
                child.Print();
            }
        }
    }

    /// <summary>
    /// The idea of this class is to chomp away the string bit by bit from the front and
    /// generate a list of ParseResults as we encounter keywords, keeping track of
    /// the nesting level we are currently at. Inspired by a recursive decent parser.
    /// </summary>
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

        //Here we start at level -1 which is a bit odd but 
        //the rules say that the "first level" of nesting should get zero 
        //hyphens.
        public ParseResult Parse(string toParse, int level = -1, ParseResult parent = null)
        {
            if (string.IsNullOrWhiteSpace(toParse))
                return null;

            parent = parent ?? new ParseResult { Value = "root" };
            var toReturn = new List<ParseResult>();

            if (Matches(_openParen, toParse))
            {
                HandleOpenParen(toParse, level, toReturn, parent);
                return parent;
            }

            if (Matches(_wordRegex, toParse))
            {
                HandleKeyword(toParse, level, toReturn, parent);
                return parent;
            }

            if (Matches(_closeParen, toParse))
            {
                HandleCloseParen(toParse, level, toReturn, parent);
                return parent;
            }

            return parent;
        }

        private void HandleCloseParen(string toParse, int level, List<ParseResult> toReturn, ParseResult parent)
        {
            var trimmed = _closeParen.Replace(toParse, string.Empty);
            Parse(trimmed, level - 1, parent.Parent);
        }

        private void HandleKeyword(string toParse, int level, List<ParseResult> toReturn, ParseResult parent)
        {
            var value = GetValue(toParse);
            var trimmed = _wordRegex.Replace(toParse, string.Empty);

            var toAdd = new ParseResult
            {
                Value = value,
                Level = level,
                Parent = parent,
            };

            parent.Children.Add(toAdd);
            Parse(trimmed, level, parent);
        }

        private void HandleOpenParen(string toParse, int level, List<ParseResult> toReturn, ParseResult parent)
        {
            var trimmed = _openParen.Replace(toParse, string.Empty);
            Parse(trimmed, level + 1, parent?.Children?.LastOrDefault() ?? parent);
        }
    }
}
