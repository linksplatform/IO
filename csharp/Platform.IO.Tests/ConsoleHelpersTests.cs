using System;
using System.IO;
using Xunit;

namespace Platform.IO.Tests
{
    public class ConsoleHelpersTests
    {
        [Fact]
        public void GetOrReadArgumentTest_WithValidIndex()
        {
            var args = new[] { "arg1", "arg2", "arg3" };
            var result = ConsoleHelpers.GetOrReadArgument(1, args);
            Assert.Equal("arg2", result);
        }

        [Fact]
        public void GetOrReadArgumentTest_WithValidIndexAndMessage()
        {
            var args = new[] { "arg1", "arg2", "arg3" };
            var result = ConsoleHelpers.GetOrReadArgument(2, "Custom message", args);
            Assert.Equal("arg3", result);
        }

        [Fact]
        public void GetOrReadArgumentTest_WithInvalidIndex()
        {
            var args = new[] { "arg1", "arg2" };
            
            // Simulate user input by redirecting stdin
            var input = "user_input\n";
            var inputStream = new StringReader(input);
            Console.SetIn(inputStream);

            var result = ConsoleHelpers.GetOrReadArgument(5, args);
            Assert.Equal("user_input", result);
        }

        [Fact]
        public void GetOrReadArgumentTest_WithEmptyInput()
        {
            var args = new string[] { };
            
            // Simulate empty user input
            var input = "\n";
            var inputStream = new StringReader(input);
            Console.SetIn(inputStream);

            var result = ConsoleHelpers.GetOrReadArgument(0, args);
            Assert.Equal("", result);
        }

        [Fact]
        public void GetOrReadArgumentTest_TrimsWhitespace()
        {
            var args = new[] { "  trimmed  " };
            var result = ConsoleHelpers.GetOrReadArgument(0, args);
            Assert.Equal("trimmed", result);
        }

        [Fact]
        public void GetOrReadArgumentTest_TrimsQuotes()
        {
            var args = new[] { "\"quoted\"" };
            var result = ConsoleHelpers.GetOrReadArgument(0, args);
            Assert.Equal("quoted", result);
        }

        [Fact]
        public void GetOrReadArgumentTest_TrimsQuotesAndWhitespace()
        {
            var args = new[] { "  \"quoted and trimmed\"  " };
            var result = ConsoleHelpers.GetOrReadArgument(0, args);
            Assert.Equal("quoted and trimmed", result);
        }

        [Fact]
        public void DebugTest_CallableWithoutException()
        {
            // Debug method should not throw exceptions
            ConsoleHelpers.Debug("Test debug message");
            ConsoleHelpers.Debug("Formatted message: {0}", "value");
        }
    }
}