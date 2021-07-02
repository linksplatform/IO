using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.IO;
using System.IO;
using System.Diagnostics;
using Xunit.Abstractions;
using System.Runtime.InteropServices;
using System.Reflection;

namespace Platform.IO.Tests
{
    public class TemporaryFileTests
    {
        [Fact]
        public void TemporaryFileTest()
        {
            using Process process = new Process();
            process.StartInfo.FileName = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "TemporaryFileTest", "bin", "Debug", "net5", "TemporaryFileTest"));
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            output.WriteLine(Environment.CurrentDirectory);
            output.WriteLine(Assembly.GetExecutingAssembly().Location);
            string path = process.StandardOutput.ReadLine();
            Assert.True(File.Exists(path));
            process.WaitForExit();
            Assert.False(File.Exists(path));
        }

        [Fact]
        public void TemporaryFileTestWithoutConsoleApp()
        {
            string fileName;
            using (TemporaryFile tempFile = new TemporaryFile())
            {
                fileName = tempFile.Filename;
                Assert.True(File.Exists(fileName));
            }
            output.WriteLine(Environment.CurrentDirectory);
            output.WriteLine(Assembly.GetExecutingAssembly().Location);
            Assert.False(File.Exists(fileName));
        }
    }
}