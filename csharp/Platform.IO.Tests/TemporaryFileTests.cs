using Xunit;
using System.IO;
using System.Diagnostics;

namespace Platform.IO.Tests
{
    public class TemporaryFileTests
    {
        [Fact]
        public void TemporaryFileTest()
        {
            using Process process = new();
            process.StartInfo.FileName = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "Platform.IO.Tests.TemporaryFileTest", "bin", "Debug", "net5", "Platform.IO.Tests.TemporaryFileTest"));
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            var path = process.StandardOutput.ReadLine();
            Assert.True(File.Exists(path));
            process.WaitForExit();
            Assert.False(File.Exists(path));
        }

        [Fact]
        public void TemporaryFileTestWithoutConsoleApp()
        {
            string fileName;
            using (TemporaryFile tempFile = new())
            {
                fileName = tempFile;
                Assert.True(File.Exists(fileName));
            }
            Assert.False(File.Exists(fileName));
        }
    }
}
