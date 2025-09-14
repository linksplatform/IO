using System;
using System.Globalization;
using System.Threading;
using Xunit;

namespace Platform.IO.Tests
{
    public class ConsoleHelpersTests
    {
        [Fact]
        public void GetOrReadArgumentUsesLocalizedFormat()
        {
            var result = ConsoleHelpers.GetOrReadArgument(0, "arg1");
            Assert.Equal("arg1", result);
        }

        [Fact]
        public void GetOrReadArgumentWithIndexUsesLocalizedFormat()
        {
            var result = ConsoleHelpers.GetOrReadArgument(0, "arg1");
            Assert.Equal("arg1", result);
        }

        [Fact]
        public void ResourcesReturnLocalizedStrings()
        {
            var originalCulture = Thread.CurrentThread.CurrentUICulture;
            
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                Assert.Equal("Press any key to continue.", Resources.PressAnyKeyToContinue);
                Assert.Equal("{0} argument", Resources.ArgumentPrompt);
                Assert.Equal("{0}: ", Resources.InputPrompt);
                
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
                Assert.Equal("Нажмите любую клавишу для продолжения.", Resources.PressAnyKeyToContinue);
                Assert.Equal("{0} аргумент", Resources.ArgumentPrompt);
                Assert.Equal("{0}: ", Resources.InputPrompt);
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = originalCulture;
            }
        }

        [Fact]
        public void ArgumentPromptFormattingWorks()
        {
            var originalCulture = Thread.CurrentThread.CurrentUICulture;
            
            try
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en");
                var englishPrompt = string.Format(Resources.ArgumentPrompt, 1);
                Assert.Equal("1 argument", englishPrompt);
                
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("ru");
                var russianPrompt = string.Format(Resources.ArgumentPrompt, 1);
                Assert.Equal("1 аргумент", russianPrompt);
            }
            finally
            {
                Thread.CurrentThread.CurrentUICulture = originalCulture;
            }
        }
    }
}