using System;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.IO;
using System.IO;
using System.Diagnostics;

namespace Platform.IO.Tests
{
    public class TemporaryFileTests
    {
        [Fact]
        public void TemporaryFileExistsTest()
        {
            using Process process = new Process();
            process.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();

            Console.WriteLine(process.StandardOutput.ReadToEnd());

            process.WaitForExit();
        }

    }
}
