using System.IO;
using System.Linq;
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
        public void ReadAllChars_ReturnsCorrectCharArray()
        {
            var temporaryFile = Path.GetTempFileName();
            var content = "Hello World!";
            File.WriteAllText(temporaryFile, content);
            
            var chars = FileHelpers.ReadAllChars(temporaryFile);
            
            Assert.Equal(content.ToCharArray(), chars);
            File.Delete(temporaryFile);
        }

        [Fact]
        public void ReadAll_ReturnsAllStructures()
        {
            var temporaryFile = Path.GetTempFileName();
            var values = new[] { 10, 20, 30, 40, 50 };
            
            using (var stream = File.OpenWrite(temporaryFile))
            {
                foreach (var value in values)
                {
                    stream.Write(value);
                }
            }
            
            var readValues = FileHelpers.ReadAll<int>(temporaryFile);
            Assert.Equal(values, readValues);
            
            File.Delete(temporaryFile);
        }

        [Fact]
        public void ReadFirstOrDefault_ReturnsFirstValue()
        {
            var temporaryFile = Path.GetTempFileName();
            var values = new[] { 100L, 200L, 300L };
            
            using (var stream = File.OpenWrite(temporaryFile))
            {
                foreach (var value in values)
                {
                    stream.Write(value);
                }
            }
            
            var firstValue = FileHelpers.ReadFirstOrDefault<long>(temporaryFile);
            Assert.Equal(100L, firstValue);
            
            File.Delete(temporaryFile);
        }

        [Fact]
        public void ReadFirstOrDefault_ReturnsDefaultWhenFileDoesNotExist()
        {
            var nonExistentFile = Path.Combine(Path.GetTempPath(), "non_existent_file.tmp");
            var result = FileHelpers.ReadFirstOrDefault<int>(nonExistentFile);
            Assert.Equal(0, result);
        }

        [Fact]
        public void ReadFirstOrDefault_ReturnsDefaultWhenFileEmpty()
        {
            var temporaryFile = Path.GetTempFileName();
            // File exists but is empty
            
            var result = FileHelpers.ReadFirstOrDefault<int>(temporaryFile);
            Assert.Equal(0, result);
            
            File.Delete(temporaryFile);
        }

        [Fact]
        public void ReadLastOrDefault_ReturnsLastValue()
        {
            var temporaryFile = Path.GetTempFileName();
            var values = new[] { 100.5, 200.5, 300.5 };
            
            using (var stream = File.OpenWrite(temporaryFile))
            {
                foreach (var value in values)
                {
                    stream.Write(value);
                }
            }
            
            var lastValue = FileHelpers.ReadLastOrDefault<double>(temporaryFile);
            Assert.Equal(300.5, lastValue);
            
            File.Delete(temporaryFile);
        }

        [Fact]
        public void ReadLastOrDefault_ReturnsDefaultWhenFileDoesNotExist()
        {
            var nonExistentFile = Path.Combine(Path.GetTempPath(), "non_existent_file.tmp");
            var result = FileHelpers.ReadLastOrDefault<int>(nonExistentFile);
            Assert.Equal(0, result);
        }

        [Fact]
        public void WriteFirst_CreatesFileAndWritesValue()
        {
            var temporaryFile = Path.GetTempFileName();
            File.Delete(temporaryFile); // Ensure it doesn't exist initially
            
            var value = 999;
            FileHelpers.WriteFirst(temporaryFile, value);
            
            Assert.True(File.Exists(temporaryFile));
            var readValue = FileHelpers.ReadFirstOrDefault<int>(temporaryFile);
            Assert.Equal(value, readValue);
            
            File.Delete(temporaryFile);
        }

        [Fact]
        public void WriteFirst_OverwritesExistingValue()
        {
            var temporaryFile = Path.GetTempFileName();
            
            // Write initial value
            FileHelpers.WriteFirst(temporaryFile, 100);
            Assert.Equal(100, FileHelpers.ReadFirstOrDefault<int>(temporaryFile));
            
            // Overwrite with new value
            FileHelpers.WriteFirst(temporaryFile, 200);
            Assert.Equal(200, FileHelpers.ReadFirstOrDefault<int>(temporaryFile));
            
            File.Delete(temporaryFile);
        }

        [Fact]
        public void Append_ReturnsWritableStream()
        {
            var temporaryFile = Path.GetTempFileName();
            
            using (var stream = FileHelpers.Append(temporaryFile))
            {
                Assert.True(stream.CanWrite);
                stream.WriteByte(42);
            }
            
            var bytes = File.ReadAllBytes(temporaryFile);
            Assert.Contains((byte)42, bytes);
            
            File.Delete(temporaryFile);
        }

        [Fact]
        public void GetSize_ReturnsCorrectSize()
        {
            var temporaryFile = Path.GetTempFileName();
            var content = "Hello World!";
            File.WriteAllText(temporaryFile, content);
            
            var size = FileHelpers.GetSize(temporaryFile);
            Assert.Equal(content.Length, size);
            
            File.Delete(temporaryFile);
        }

        [Fact]
        public void GetSize_ReturnsZeroForNonExistentFile()
        {
            var nonExistentFile = Path.Combine(Path.GetTempPath(), "non_existent_file.tmp");
            var size = FileHelpers.GetSize(nonExistentFile);
            Assert.Equal(0, size);
        }

        [Fact]
        public void SetSize_ResizesFile()
        {
            var temporaryFile = Path.GetTempFileName();
            
            FileHelpers.SetSize(temporaryFile, 1024);
            Assert.Equal(1024, FileHelpers.GetSize(temporaryFile));
            
            FileHelpers.SetSize(temporaryFile, 512);
            Assert.Equal(512, FileHelpers.GetSize(temporaryFile));
            
            File.Delete(temporaryFile);
        }

        [Fact]
        public void SetSize_CreatesFileIfNotExists()
        {
            var temporaryFile = Path.Combine(Path.GetTempPath(), "new_file.tmp");
            if (File.Exists(temporaryFile))
                File.Delete(temporaryFile);
            
            FileHelpers.SetSize(temporaryFile, 100);
            
            Assert.True(File.Exists(temporaryFile));
            Assert.Equal(100, FileHelpers.GetSize(temporaryFile));
            
            File.Delete(temporaryFile);
        }

        [Fact]
        public void DeleteAll_RemovesAllFilesInDirectory()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "test_dir_" + Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);
            
            // Create test files
            File.WriteAllText(Path.Combine(tempDir, "file1.txt"), "content1");
            File.WriteAllText(Path.Combine(tempDir, "file2.txt"), "content2");
            File.WriteAllText(Path.Combine(tempDir, "file3.log"), "content3");
            
            FileHelpers.DeleteAll(tempDir);
            
            Assert.Empty(Directory.GetFiles(tempDir));
            
            Directory.Delete(tempDir);
        }

        [Fact]
        public void DeleteAll_WithPattern_RemovesMatchingFiles()
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "test_dir_" + Path.GetRandomFileName());
            Directory.CreateDirectory(tempDir);
            
            // Create test files
            File.WriteAllText(Path.Combine(tempDir, "file1.txt"), "content1");
            File.WriteAllText(Path.Combine(tempDir, "file2.txt"), "content2");
            File.WriteAllText(Path.Combine(tempDir, "file3.log"), "content3");
            
            FileHelpers.DeleteAll(tempDir, "*.txt");
            
            var remainingFiles = Directory.GetFiles(tempDir);
            Assert.Single(remainingFiles);
            Assert.Contains("file3.log", remainingFiles[0]);
            
            Directory.Delete(tempDir, true);
        }

        [Fact]
        public void Truncate_EmptiesFile()
        {
            var temporaryFile = Path.GetTempFileName();
            File.WriteAllText(temporaryFile, "Some content to be removed");
            
            Assert.True(FileHelpers.GetSize(temporaryFile) > 0);
            
            FileHelpers.Truncate(temporaryFile);
            
            Assert.Equal(0, FileHelpers.GetSize(temporaryFile));
            
            File.Delete(temporaryFile);
        }

        [Fact]
        public void AppendLine_AddsLineToFile()
        {
            var temporaryFile = Path.GetTempFileName();
            
            FileHelpers.AppendLine(temporaryFile, "First line");
            FileHelpers.AppendLine(temporaryFile, "Second line");
            
            var content = File.ReadAllText(temporaryFile);
            Assert.Contains("First line", content);
            Assert.Contains("Second line", content);
            
            var lines = File.ReadAllLines(temporaryFile);
            Assert.Equal(2, lines.Length);
            Assert.Equal("First line", lines[0]);
            Assert.Equal("Second line", lines[1]);
            
            File.Delete(temporaryFile);
        }

        [Fact]
        public void EachLine_ExecutesActionForEachLine()
        {
            var temporaryFile = Path.GetTempFileName();
            var lines = new[] { "Line 1", "Line 2", "Line 3" };
            File.WriteAllLines(temporaryFile, lines);
            
            var processedLines = new System.Collections.Generic.List<string>();
            FileHelpers.EachLine(temporaryFile, line => processedLines.Add(line));
            
            Assert.Equal(lines.Length, processedLines.Count);
            for (int i = 0; i < lines.Length; i++)
            {
                Assert.Equal(lines[i], processedLines[i]);
            }
            
            File.Delete(temporaryFile);
        }

        [Fact]
        public void EachLine_HandlesEmptyFile()
        {
            var temporaryFile = Path.GetTempFileName();
            // File exists but is empty
            
            var callCount = 0;
            FileHelpers.EachLine(temporaryFile, line => callCount++);
            
            Assert.Equal(0, callCount);
            
            File.Delete(temporaryFile);
        }
    }
}
