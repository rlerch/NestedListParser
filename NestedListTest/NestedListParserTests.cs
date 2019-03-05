using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NestedListParser;

namespace NestedListTest
{
    [TestClass]
    public class NestedListParserTests
    {
        private NestedListParser.NestedListParser _parser;

        [TestInitialize]
        public void TestInit()
        {
            _parser = new NestedListParser.NestedListParser();
        }

        [TestMethod]
        public void WhenCalledWithEmptyString_ReturnsNull()
        {
            var toCheck = _parser.Parse(string.Empty);

            Assert.AreEqual(null, toCheck);
        }

        [TestMethod]
        public void WhenCalledWithASingleWord_ThatWordIsReturnedWithZeroLevel()
        {
            var toParse = "SingleWord";
            var toCheck = _parser.Parse(toParse);

            Assert.AreEqual(1, toCheck.Children.Count);
            Assert.AreEqual(toParse, toCheck.Children.First().Value);
        }

        [TestMethod]
        public void WhenCalledWithWordsInMultipleLevels_ThoseWordsAreReturnedWithTheirRespectiveLevels()
        {
            var toParse = "(zero(one(two))))";
            var toCheck = _parser.Parse(toParse);

            var firstResult = toCheck.Children[0];
            Assert.AreEqual(0, firstResult.Level);
            Assert.AreEqual("zero", firstResult.Value);

            var secondResult = firstResult.Children[0];
            Assert.AreEqual(1, secondResult.Level);
            Assert.AreEqual("one", secondResult.Value);

            var thirdResult = secondResult.Children[0];
            Assert.AreEqual(2, thirdResult.Level);
            Assert.AreEqual("two", thirdResult.Value);
        }

        [TestMethod]
        public void WhenCalledWithWordsWithMultipleItemsAtTheSameLevel_ThoseWordsAreReturnedWithTheirRespectiveLevels()
        {
            var toParse = "(zero, one(two), zero)";
            var toCheck = _parser.Parse(toParse);

            var firstResult = toCheck.Children[0];
            Assert.AreEqual(0, firstResult.Level);
            Assert.AreEqual("zero", firstResult.Value);

            var secondResult = toCheck.Children[1];
            Assert.AreEqual(0, secondResult.Level);
            Assert.AreEqual("one", secondResult.Value);

            var thirdResult = secondResult.Children[0];
            Assert.AreEqual(1, thirdResult.Level);
            Assert.AreEqual("two", thirdResult.Value);

            var fourthResult = toCheck.Children[2];
            Assert.AreEqual(0, fourthResult.Level);
            Assert.AreEqual("zero", fourthResult.Value);
        }
    }
}
