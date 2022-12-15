using Xunit;
using System.IO;
using System.Diagnostics;

namespace Platform.IO.Tests
{
    public class TemporaryFileTests
    {
        [Fact(Skip = "")]
        public void TemporaryFileTest()
        {
            var startInfo = new ProcessStartInfo
            {
                WorkingDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "Platform.IO.Tests.TemporaryFileTest")),
                UseShellExecute = false,
                RedirectStandardOutput = true,
                FileName = "dotnet",
                Arguments = "run --project Platform.IO.Tests.TemporaryFileTest.csproj"
            };
            using Process process = new(){StartInfo = startInfo};
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
