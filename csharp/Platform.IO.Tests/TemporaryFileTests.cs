using Xunit;
using System.IO;
using System.Diagnostics;

namespace Platform.IO.Tests
{
    /// <summary>
    /// <para>
    /// Represents the temporary file tests.
    /// </para>
    /// <para></para>
    /// </summary>
    public class TemporaryFileTests
    {
        /// <summary>
        /// <para>
        /// Tests that temporary file test.
        /// </para>
        /// <para></para>
        /// </summary>
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

        /// <summary>
        /// <para>
        /// Tests that temporary file test without console app.
        /// </para>
        /// <para></para>
        /// </summary>
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
