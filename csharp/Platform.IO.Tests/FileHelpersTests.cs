using System.IO;
using Xunit;

namespace Platform.IO.Tests
{
    /// <summary>
    /// <para>
    /// Represents the file helpers tests.
    /// </para>
    /// <para></para>
    /// </summary>
    public class FileHelpersTests
    {
        /// <summary>
        /// <para>
        /// Tests that write read test.
        /// </para>
        /// <para></para>
        /// </summary>
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
    }
}
