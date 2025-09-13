using System;
using System.IO;
using Xunit;

namespace Platform.IO.Tests
{
    public class FileHelpersTests
    {
        [Fact]
        public void WriteReadTest()
        {
            var temporaryFile = Path.GetTempFileName();
            var originalValue = 42UL;
            FileHelpers.WriteFirst(temporaryFile, originalValue);
            var readValue = FileHelpers.ReadFirstOrDefault<ulong>(temporaryFile);
            Assert.Equal(readValue, originalValue);
            File.Delete(temporaryFile);
        }

        [Fact]
        public void DeleteFilesTest()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            
            var testFile1 = Path.Combine(tempDir, "test1.txt");
            var testFile2 = Path.Combine(tempDir, "test2.txt");
            var testFile3 = Path.Combine(tempDir, "test3.log");
            
            File.WriteAllText(testFile1, "test content");
            File.WriteAllText(testFile2, "test content");
            File.WriteAllText(testFile3, "test content");
            
            Assert.True(File.Exists(testFile1));
            Assert.True(File.Exists(testFile2));
            Assert.True(File.Exists(testFile3));
            
            FileHelpers.DeleteFiles(tempDir, "*.txt");
            
            Assert.False(File.Exists(testFile1));
            Assert.False(File.Exists(testFile2));
            Assert.True(File.Exists(testFile3));
            
            FileHelpers.DeleteFiles(tempDir);
            
            Assert.False(File.Exists(testFile3));
            
            Directory.Delete(tempDir);
        }
    }
}
