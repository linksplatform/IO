using Xunit;
using System.IO;
using System.Diagnostics;
using System.Text;

namespace Platform.IO.Tests
{
    public class TemporaryFileTests
    {
        // [Fact(Skip = "")]
        // public void TemporaryFileTest()
        // {
        //     var startInfo = new ProcessStartInfo
        //     {
        //         WorkingDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "Platform.IO.Tests.TemporaryFileTest")),
        //         UseShellExecute = false,
        //         RedirectStandardOutput = true,
        //         FileName = "dotnet",
        //         Arguments = "run --project Platform.IO.Tests.TemporaryFileTest.csproj"
        //     };
        //     using Process process = new(){StartInfo = startInfo};
        //     process.Start();
        //     var path = process.StandardOutput.ReadLine();
        //     Assert.True(File.Exists(path));
        //     process.WaitForExit();
        //     Assert.False(File.Exists(path));
        // }

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

        [Fact]
        public void TemporaryFileStreamAccess()
        {
            string testContent = "Hello, tmpfile-like functionality!";
            byte[] testData = Encoding.UTF8.GetBytes(testContent);
            
            using (TemporaryFile tempFile = new())
            {
                FileStream stream = tempFile;
                
                // Write data to the stream like C's tmpfile
                stream.Write(testData, 0, testData.Length);
                stream.Flush();
                
                // Read back the data
                stream.Seek(0, SeekOrigin.Begin);
                byte[] readBuffer = new byte[testData.Length];
                int bytesRead = stream.Read(readBuffer, 0, readBuffer.Length);
                
                Assert.Equal(testData.Length, bytesRead);
                Assert.Equal(testContent, Encoding.UTF8.GetString(readBuffer));
            }
        }

        [Fact]
        public void TemporaryFileUniqueNames()
        {
            // Test that multiple temporary files get unique names
            using var tempFile1 = new TemporaryFile();
            using var tempFile2 = new TemporaryFile();
            using var tempFile3 = new TemporaryFile();
            
            string name1 = tempFile1;
            string name2 = tempFile2;
            string name3 = tempFile3;
            
            Assert.NotEqual(name1, name2);
            Assert.NotEqual(name2, name3);
            Assert.NotEqual(name1, name3);
            
            Assert.True(File.Exists(name1));
            Assert.True(File.Exists(name2));
            Assert.True(File.Exists(name3));
        }

        [Fact]
        public void TemporaryFileSecureCreation()
        {
            using (TemporaryFile tempFile = new())
            {
                string fileName = tempFile;
                
                // Verify file exists and is accessible
                Assert.True(File.Exists(fileName));
                
                // Verify we can write and read from both filename and stream
                FileStream directStream = tempFile;
                
                byte[] testData = Encoding.UTF8.GetBytes("Security test data");
                directStream.Write(testData, 0, testData.Length);
                directStream.Flush();
                
                // Verify data integrity
                directStream.Seek(0, SeekOrigin.Begin);
                byte[] readData = new byte[testData.Length];
                int bytesRead = directStream.Read(readData, 0, readData.Length);
                
                Assert.Equal(testData.Length, bytesRead);
                Assert.Equal(testData, readData);
            }
        }

        [Fact]
        public void TemporaryFileCleanupOnDisposal()
        {
            string fileName;
            
            using (TemporaryFile tempFile = new())
            {
                fileName = tempFile;
                FileStream stream = tempFile;
                
                Assert.True(File.Exists(fileName));
                Assert.True(stream.CanWrite);
                Assert.True(stream.CanRead);
                
                // Write some data to ensure the file is actually created
                stream.WriteByte(65); // ASCII 'A'
                stream.Flush();
            }
            
            // File should be automatically deleted after disposal
            Assert.False(File.Exists(fileName));
        }

        [Fact]
        public void TemporaryFileStreamPropertiesLikeTmpfile()
        {
            using (TemporaryFile tempFile = new())
            {
                FileStream stream = tempFile;
                
                // Verify stream properties match tmpfile behavior
                Assert.True(stream.CanRead);
                Assert.True(stream.CanWrite);
                Assert.True(stream.CanSeek);
                Assert.Equal(0, stream.Position);
                Assert.Equal(0, stream.Length);
            }
        }
    }
}
