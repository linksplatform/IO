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
        public void ReadFirstOrDefaultShouldReturnDefaultForMisalignedFile()
        {
            var temporaryFile = Path.GetTempFileName();
            // Write 5 bytes to create a file that's not aligned to ulong (8 bytes)
            File.WriteAllBytes(temporaryFile, new byte[] { 1, 2, 3, 4, 5 });
            
            // This should return default(ulong) = 0, not throw an exception
            var result = FileHelpers.ReadFirstOrDefault<ulong>(temporaryFile);
            Assert.Equal(0UL, result);
            
            File.Delete(temporaryFile);
        }

        [Fact]
        public void ReadLastOrDefaultShouldReturnDefaultForMisalignedFile()
        {
            var temporaryFile = Path.GetTempFileName();
            // Write 5 bytes to create a file that's not aligned to ulong (8 bytes)
            File.WriteAllBytes(temporaryFile, new byte[] { 1, 2, 3, 4, 5 });
            
            // This should return default(ulong) = 0, not throw an exception
            var result = FileHelpers.ReadLastOrDefault<ulong>(temporaryFile);
            Assert.Equal(0UL, result);
            
            File.Delete(temporaryFile);
        }
    }
}
