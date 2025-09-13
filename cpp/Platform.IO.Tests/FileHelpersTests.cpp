#include <gtest/gtest.h>
#include <filesystem>
#include <cstdint>
#include "../Platform.IO/FileHelpers.h"
#include "../Platform.IO/TemporaryFile.h"

using namespace Platform::IO;

namespace Platform::IO::Tests
{
    class FileHelpersTests : public ::testing::Test
    {
    protected:
        void SetUp() override {}
        void TearDown() override {}
    };

    TEST_F(FileHelpersTests, WriteReadTest)
    {
        TemporaryFile temporaryFile;
        const std::uint64_t originalValue = 42UL;
        
        FileHelpers::WriteFirst(temporaryFile.Filename, originalValue);
        auto readValue = FileHelpers::ReadFirstOrDefault<std::uint64_t>(temporaryFile.Filename);
        
        EXPECT_EQ(readValue, originalValue);
    }

    TEST_F(FileHelpersTests, WriteReadMultipleValuesTest)
    {
        TemporaryFile temporaryFile;
        const std::vector<std::uint32_t> originalValues = {1, 2, 3, 4, 5};
        
        // Write values to file
        std::ofstream file(temporaryFile.Filename, std::ios::binary);
        for (const auto& value : originalValues) {
            file.write(reinterpret_cast<const char*>(&value), sizeof(value));
        }
        file.close();
        
        // Read all values back
        auto readValues = FileHelpers::ReadAll<std::uint32_t>(temporaryFile.Filename);
        
        EXPECT_EQ(readValues.size(), originalValues.size());
        for (std::size_t i = 0; i < originalValues.size(); ++i) {
            EXPECT_EQ(readValues[i], originalValues[i]);
        }
        
        // Test reading first and last
        auto firstValue = FileHelpers::ReadFirstOrDefault<std::uint32_t>(temporaryFile.Filename);
        auto lastValue = FileHelpers::ReadLastOrDefault<std::uint32_t>(temporaryFile.Filename);
        
        EXPECT_EQ(firstValue, originalValues.front());
        EXPECT_EQ(lastValue, originalValues.back());
    }

    TEST_F(FileHelpersTests, FileSizeTest)
    {
        TemporaryFile temporaryFile;
        
        // Initially should be empty
        EXPECT_EQ(FileHelpers::GetSize(temporaryFile.Filename), 0);
        
        const std::string testData = "Hello, World!";
        std::ofstream file(temporaryFile.Filename);
        file << testData;
        file.close();
        
        EXPECT_EQ(FileHelpers::GetSize(temporaryFile.Filename), testData.length());
    }

    TEST_F(FileHelpersTests, AppendLineTest)
    {
        TemporaryFile temporaryFile;
        
        const std::string line1 = "First line";
        const std::string line2 = "Second line";
        
        FileHelpers::AppendLine(temporaryFile.Filename, line1);
        FileHelpers::AppendLine(temporaryFile.Filename, line2);
        
        std::vector<std::string> lines;
        FileHelpers::EachLine(temporaryFile.Filename, [&lines](const std::string& line) {
            lines.push_back(line);
        });
        
        EXPECT_EQ(lines.size(), 2);
        EXPECT_EQ(lines[0], line1);
        EXPECT_EQ(lines[1], line2);
    }
}
