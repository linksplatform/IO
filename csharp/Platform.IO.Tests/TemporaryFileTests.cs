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

namespace Platform.IO.Tests
{
    public class TemporaryFileTests
    {
        private readonly ITestOutputHelper output;
        public TemporaryFileTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void TemporaryFileTest()
        {
            bool isWindows = System.Runtime.InteropServices.RuntimeInformation
                                                .IsOSPlatform(OSPlatform.Windows);
            bool isLinux = System.Runtime.InteropServices.RuntimeInformation
                                               .IsOSPlatform(OSPlatform.Linux);
            using Process process = new Process();
            process.StartInfo.FileName = @"..\..\..\..\TemporaryFileTest\bin\Debug\net5.0\TemporaryFileTest.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            //process.Start();
            output.WriteLine(process.StartInfo.FileName);
            output.WriteLine($"{File.Exists(process.StartInfo.FileName)}");

            string path = process.StandardOutput.ReadLine();
            Assert.True(File.Exists(path));
            process.WaitForExit();

            Assert.False(File.Exists(path));
        }

    }
}
