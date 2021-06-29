﻿using System;
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
            process.StartInfo.FileName = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "TemporaryFileTest", "bin"));
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            //process.Start();
            output.WriteLine(process.StartInfo.FileName);
            output.WriteLine($"{File.Exists(process.StartInfo.FileName)}");
            foreach (var item in Directory.GetFiles(process.StartInfo.FileName, "*.*", SearchOption.AllDirectories))
            {
                output.WriteLine(item);
            }

            string path = process.StandardOutput.ReadLine();
            Assert.True(File.Exists(path));
            process.WaitForExit();

            Assert.False(File.Exists(path));
        }

    }
}
