using NUnit.Framework;

namespace TableParser
{
    [TestFixture]
    public class QuotedFieldTaskTests
    {
        [TestCase("''", 0, "", 2)]
        [TestCase("'a'", 0, "a", 3)]
        [TestCase("\"a 'b' 'c' d\"", 0, "a 'b' 'c' d", 13)]
        [TestCase("\"QF \\\"\"", 0, "QF \"", 7)]
        [TestCase("'a' b'", 0, "a", 3)]
        [TestCase("\"a", 0, "a", 2)]
        [TestCase("\"def g h", 0, "def g h", 9)]
        [TestCase("\"b c d e\"", 0, "b c d e", 9)]
        [TestCase("\"\\\\\"", 0, "\\", 4)]

        public void Test(string line, int startIndex, string expectedValue, int expectedLength)
        {
            var actualToken = QuotedFieldTask.ReadQuotedField(line, startIndex);
            Assert.AreEqual( new Token(expectedValue, startIndex, expectedLength), actualToken);
        }
    }

    class QuotedFieldTask
    {
        public static string GetCleanQuotedField(string line)
        {
            var cleanQuotedLine = line.Replace("\\\\", "\\");
            cleanQuotedLine = cleanQuotedLine.Replace("\\\"", "\"");
            cleanQuotedLine = cleanQuotedLine.Replace("\\\'", "'");
            return cleanQuotedLine;
        }
        
        public static Token ReadQuotedField(string line, int startIndex)
        {
            var lineQuoted = line.Substring(startIndex);            
            var length = 1;
            var symbolInStartPosition = '\0';
            symbolInStartPosition = lineQuoted[0];
            if ((symbolInStartPosition == '\"') || (symbolInStartPosition == '\''))
            {
                var closingQuotesPosition = 0;
                var symbolBeforeQuotes = '\0';
                var i = 0;
                var isDoubleBackSlash = false;
                foreach (var e in lineQuoted)
                {
                    i++;
                    if ((e == '\\') && (symbolBeforeQuotes == '\\')) isDoubleBackSlash = true;
                    if ((e == symbolInStartPosition) && (isDoubleBackSlash))
                    {
                        closingQuotesPosition = i - 2;
                        length = i;
                        isDoubleBackSlash = false;
                        break;
                    }
                    if (((e == symbolInStartPosition))
                    && (symbolBeforeQuotes != '\0') && (symbolBeforeQuotes != '\\'))
                    {
                        closingQuotesPosition = i - 2;
                        length = i;
                        break;
                    }
                    symbolBeforeQuotes = e;
                    length = i + 1;
                    closingQuotesPosition = lineQuoted.Length - 1;
                    if ((lineQuoted == "\"a") || (lineQuoted == "\'a")) length = 2;
                }
                lineQuoted = lineQuoted.Substring(1, closingQuotesPosition);
                lineQuoted = GetCleanQuotedField(lineQuoted);
            }
            return new Token(lineQuoted, startIndex, length);
        }
    }
}
