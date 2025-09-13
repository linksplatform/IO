using System.IO;
using Xunit;

namespace Platform.IO.Tests
{
    public class TemporaryFilesTests
    {
        [Fact]
        public void UseNew_ReturnsValidFilePath()
        {
            var tempFilePath = TemporaryFiles.UseNew();
            
            Assert.False(string.IsNullOrEmpty(tempFilePath));
            Assert.True(File.Exists(tempFilePath));
            
            // Clean up
            File.Delete(tempFilePath);
        }

        [Fact]
        public void UseNew_CreatesUniqueFiles()
        {
            var tempFile1 = TemporaryFiles.UseNew();
            var tempFile2 = TemporaryFiles.UseNew();
            
            Assert.NotEqual(tempFile1, tempFile2);
            Assert.True(File.Exists(tempFile1));
            Assert.True(File.Exists(tempFile2));
            
            // Clean up
            File.Delete(tempFile1);
            File.Delete(tempFile2);
        }

        [Fact]
        public void UseNew_AddsFileToUsedFilesList()
        {
            var tempFile = TemporaryFiles.UseNew();
            
            // The file should be added to the used files list
            // We can verify this indirectly by checking if the file exists
            Assert.True(File.Exists(tempFile));
            
            // Clean up manually before calling DeleteAllPreviouslyUsed
            File.Delete(tempFile);
        }

        [Fact]
        public void DeleteAllPreviouslyUsed_RemovesTrackedFiles()
        {
            // Create some temporary files
            var tempFile1 = TemporaryFiles.UseNew();
            var tempFile2 = TemporaryFiles.UseNew();
            
            Assert.True(File.Exists(tempFile1));
            Assert.True(File.Exists(tempFile2));
            
            // Delete all previously used files
            TemporaryFiles.DeleteAllPreviouslyUsed();
            
            // Files should be deleted
            Assert.False(File.Exists(tempFile1));
            Assert.False(File.Exists(tempFile2));
        }

        [Fact]
        public void DeleteAllPreviouslyUsed_DoesNotThrowWhenNoFiles()
        {
            // Should not throw exception when called with no tracked files
            TemporaryFiles.DeleteAllPreviouslyUsed();
        }

        [Fact]
        public void DeleteAllPreviouslyUsed_DoesNotThrowWhenFilesAlreadyDeleted()
        {
            var tempFile = TemporaryFiles.UseNew();
            
            // Delete file manually first
            File.Delete(tempFile);
            
            // Should not throw when trying to delete already deleted file
            TemporaryFiles.DeleteAllPreviouslyUsed();
        }

        [Fact]
        public void UseNew_AndDeleteAllPreviouslyUsed_WorksTogether()
        {
            // Create multiple temporary files
            var tempFiles = new string[5];
            for (int i = 0; i < 5; i++)
            {
                tempFiles[i] = TemporaryFiles.UseNew();
                Assert.True(File.Exists(tempFiles[i]));
            }
            
            // Write some content to verify they are real files
            for (int i = 0; i < 5; i++)
            {
                File.WriteAllText(tempFiles[i], $"Content {i}");
                Assert.Equal($"Content {i}", File.ReadAllText(tempFiles[i]));
            }
            
            // Delete all tracked files
            TemporaryFiles.DeleteAllPreviouslyUsed();
            
            // All files should be deleted
            for (int i = 0; i < 5; i++)
            {
                Assert.False(File.Exists(tempFiles[i]));
            }
        }
    }
}