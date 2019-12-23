using System.Collections.Generic;
using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class FieldParserTaskTests
    {
        public static void ChekTokens(string line, string[] expectedValue, int[] expectedStartIndex, int[] expectedStartLength)
        {
            var actualResult = FieldsParserTask.ParseLine(line);
            Assert.AreEqual(expectedValue.Length, actualResult.Count);
            for (int i = 0; i < expectedValue.Length; ++i)
            {
                Assert.AreEqual(expectedValue[i], actualResult[i].Value);
                Assert.AreEqual(expectedStartIndex[i], actualResult[i].Position);
                Assert.AreEqual(expectedStartLength[i], actualResult[i].Length);
            }
        }

        public static void Test(string input, string[] expectedResult)
        {
            var actualResult = FieldsParserTask.ParseLine(input);
            Assert.AreEqual(expectedResult.Length, actualResult.Count);
            for (int i = 0; i < expectedResult.Length; ++i)
            {
                Assert.AreEqual(expectedResult[i], actualResult[i].Value);
            }
        }

        [TestCase("", new string[0])]
        [TestCase(" text ", new[] { "text" })]
        [TestCase("hello  world", new[] { "hello", "world" })]
        [TestCase("hello world", new[] { "hello", "world" })]
        [TestCase("''", new[] { "" })]
        [TestCase("\"def g h", new[] { "def g h" })]
        [TestCase(@"""a 'b' 'c' d""", new[] { "a 'b' 'c' d" })]
        [TestCase(@"'""1""'", new[] { "\"1\"" })]
        [TestCase(@"a""b c d e""", new[] { "a", "b c d e" })]
        [TestCase(@"""a \""c\""""", new[] { "a \"c\"" })]
        [TestCase(@"""\\""", new[] { "\\" })]
        [TestCase(@"'\'1\''", new[] { "\'1\'" })]
        [TestCase("\"h ", new[] { "h " })]
        [TestCase(@"""hello""world", new[] { "hello", "world" })]
        [TestCase("a \"bcd ef\" 'x y'", new[] { "a", "bcd ef", "x y" })]
        [TestCase("\"\\\\\" b", new[] { "\\", "b" })]
                
        public static void RunTests(string input, string[] expectedOutput)
        {            
            Test(input, expectedOutput);
        }

        [TestCase("1 2 \\\"a b\" 3", new[] { "1", "2", "\\", "a b", "3" }, new[] { 0, 2, 4, 5, 11 }, new[] { 1, 1, 1, 5, 1 })]
        [TestCase("a \"bcd ef\" 'x y'", new[] { "a", "bcd ef", "x y"}, new[] { 0, 2, 11}, new[] { 1, 8, 5})]

        public static void ChekTokensTests(string line, string[] expectedValue, int[] expectedStartIndex, int[] expectedStartLength)
        {
            ChekTokens(line,expectedValue,expectedStartIndex,expectedStartLength);
        }
    }

    public class FieldsParserTask
    {
        public static List<Token> ParseLine(string line)
        {
            var listOfTokens = new List<Token>();
            var curTokenLength = 0;            
            var startIndex = 0;            
            for (var curPos = 0; curPos < line.Length; curPos++)
            {                
                var ch = line[curPos];                
                if (ch != '\"' && ch != '\'' && ch != ' ') curTokenLength++;
                if (ch == ' ' && curTokenLength > 0) listOfTokens.Add(ReadSimpleFieldT(line,startIndex,ref curTokenLength));                
                if (ch == ' ' && curTokenLength == 0) startIndex =curPos+1;
                if ((ch == '\"' || ch == '\'')&& (curTokenLength > 0))
                {
                     listOfTokens.Add(ReadSimpleFieldT(line, startIndex,ref curTokenLength));
                     startIndex = curPos;
                }
                if ((ch == '\"' || ch == '\'') && (curTokenLength == 0))
                {
                    var tmpToken = ReadQuotedField(line, startIndex);
                    listOfTokens.Add(tmpToken);
                    startIndex = tmpToken.GetIndexNextToToken();
                    curPos = tmpToken.GetIndexNextToToken() - 1;                    
                }
                if ((curPos+1==line.Length) && (curTokenLength > 0))
                    listOfTokens.Add(ReadSimpleFieldT(line,startIndex,ref curTokenLength));
            }
            return listOfTokens;          
        }

        public static Token ReadQuotedField(string line, int startIndex)
        {
            return QuotedFieldTask.ReadQuotedField(line, startIndex);
        }

        public static Token ReadSimpleFieldT(string line, int startIndex,ref int  curTokenLength)
        {
            var curSegment = line.Substring(startIndex, curTokenLength);
            curTokenLength = 0;
            return new Token(curSegment, startIndex, curSegment.Length);
        }
    }
}
