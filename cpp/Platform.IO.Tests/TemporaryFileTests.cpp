#include <gtest/gtest.h>
#include <filesystem>
#include "../Platform.IO/TemporaryFile.h"

using namespace Platform::IO;

namespace Platform::IO::Tests
{
    class TemporaryFileTests : public ::testing::Test
    {
    protected:
        void SetUp() override {}
        void TearDown() override {}
    };

    TEST_F(TemporaryFileTests, TemporaryFileTestWithoutConsoleApp)
    {
        std::string fileName;
        {
            TemporaryFile tempFile;
            fileName = tempFile.Filename;
            EXPECT_TRUE(std::filesystem::exists(fileName));
        }
        // After the TemporaryFile goes out of scope, the file should be deleted
        EXPECT_FALSE(std::filesystem::exists(fileName));
    }

    TEST_F(TemporaryFileTests, TemporaryFileMoveSemanticsTest)
    {
        std::string fileName;
        {
            TemporaryFile tempFile1;
            fileName = tempFile1.Filename;
            EXPECT_TRUE(std::filesystem::exists(fileName));
            
            // Test move constructor
            TemporaryFile tempFile2 = std::move(tempFile1);
            EXPECT_EQ(tempFile2.Filename, fileName);
            EXPECT_TRUE(std::filesystem::exists(fileName));
        }
        // After both TemporaryFile objects go out of scope, the file should be deleted
        EXPECT_FALSE(std::filesystem::exists(fileName));
    }
}
