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
            var startInfo = new ProcessStartInfo();
            startInfo.WorkingDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "Platform.IO.Tests.TemporaryFileTest"));
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.FileName = "dotnet";
            startInfo.Arguments = "run --project Platform.IO.Tests.TemporaryFileTest.csproj";
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
