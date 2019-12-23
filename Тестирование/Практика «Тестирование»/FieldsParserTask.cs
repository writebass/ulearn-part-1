[TestCase("", new string[0])]
[TestCase(" text ", new[] {"text"})]
[TestCase("hello  world", new[] {"hello", "world"})]
[TestCase("hello world", new[] {"hello", "world"})]
[TestCase("''", new[] {""})]
[TestCase("\"def g h", new[] {"def g h"})]
[TestCase(@"""a 'b' 'c' d""", new[] {"a 'b' 'c' d"})]
[TestCase(@"'""1""'", new[] { "\"1\"" })]
[TestCase(@"a""b c d e""", new[] { "a","b c d e" })]
[TestCase(@"""a \""c\""""", new[] { "a \"c\"" })]
[TestCase(@"""\\""", new[] { "\\"})]
[TestCase(@"'\'1\''", new[] { "\'1\'" })]
[TestCase("\"h ", new[] {"h "})]
[TestCase(@"""hello""world", new[] { "hello", "world" })]


// Вставляйте сюда свои тесты
public static void RunTests(string input, string[] expectedOutput)
{
    // Тело метода изменять не нужно
	
    Test(input, expectedOutput);
}
