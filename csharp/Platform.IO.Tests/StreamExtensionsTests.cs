using System.IO;
using Xunit;

namespace Platform.IO.Tests
{
    public class StreamExtensionsTests
    {
        [Fact]
        public void WriteAndReadTest_Int32()
        {
            using var stream = new MemoryStream();
            var originalValue = 42;
            
            stream.Write(originalValue);
            stream.Position = 0;
            
            var readValue = stream.ReadOrDefault<int>();
            Assert.Equal(originalValue, readValue);
        }

        [Fact]
        public void WriteAndReadTest_UInt64()
        {
            using var stream = new MemoryStream();
            var originalValue = 12345678901234UL;
            
            stream.Write(originalValue);
            stream.Position = 0;
            
            var readValue = stream.ReadOrDefault<ulong>();
            Assert.Equal(originalValue, readValue);
        }

        [Fact]
        public void WriteAndReadTest_Double()
        {
            using var stream = new MemoryStream();
            var originalValue = 3.14159;
            
            stream.Write(originalValue);
            stream.Position = 0;
            
            var readValue = stream.ReadOrDefault<double>();
            Assert.Equal(originalValue, readValue);
        }

        [Fact]
        public void ReadOrDefault_ReturnsDefaultWhenNotEnoughData()
        {
            using var stream = new MemoryStream(new byte[] { 1, 2 }); // Only 2 bytes, int needs 4
            
            var readValue = stream.ReadOrDefault<int>();
            Assert.Equal(0, readValue); // Should return default(int)
        }

        [Fact]
        public void ReadOrDefault_ReturnsDefaultWhenStreamEmpty()
        {
            using var stream = new MemoryStream();
            
            var readValue = stream.ReadOrDefault<long>();
            Assert.Equal(0L, readValue); // Should return default(long)
        }

        [Fact]
        public void ReadAll_ReturnsAllValues()
        {
            using var stream = new MemoryStream();
            var values = new[] { 10, 20, 30, 40 };
            
            // Write all values
            foreach (var value in values)
            {
                stream.Write(value);
            }
            
            stream.Position = 0;
            var readValues = stream.ReadAll<int>();
            
            Assert.Equal(values.Length, readValues.Length);
            for (int i = 0; i < values.Length; i++)
            {
                Assert.Equal(values[i], readValues[i]);
            }
        }

        [Fact]
        public void ReadAll_ReturnsEmptyArrayWhenStreamEmpty()
        {
            using var stream = new MemoryStream();
            
            var readValues = stream.ReadAll<int>();
            
            Assert.Empty(readValues);
        }

        [Fact]
        public void ReadAll_HandlesPartialStructures()
        {
            using var stream = new MemoryStream();
            
            // Write 2 complete ints and 2 extra bytes (incomplete third int)
            stream.Write(100);
            stream.Write(200);
            stream.WriteByte(1);
            stream.WriteByte(2);
            
            stream.Position = 0;
            var readValues = stream.ReadAll<int>();
            
            // Should only return the 2 complete ints
            Assert.Equal(2, readValues.Length);
            Assert.Equal(100, readValues[0]);
            Assert.Equal(200, readValues[1]);
        }

        [Fact]
        public void WriteAndReadAll_MultipleValues()
        {
            using var stream = new MemoryStream();
            var originalValues = new[] { 1.1f, 2.2f, 3.3f, 4.4f, 5.5f };
            
            // Write all values
            foreach (var value in originalValues)
            {
                stream.Write(value);
            }
            
            stream.Position = 0;
            var readValues = stream.ReadAll<float>();
            
            Assert.Equal(originalValues.Length, readValues.Length);
            for (int i = 0; i < originalValues.Length; i++)
            {
                Assert.Equal(originalValues[i], readValues[i], 5); // Allow some floating point precision
            }
        }
    }
}