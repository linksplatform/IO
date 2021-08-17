namespace Platform::IO::Tests
{
    TEST_CLASS(TemporaryFileTests)
    {
        public: TEST_METHOD(TemporaryFileTest)
        {
            using Process process = new();
            process.StartInfo.FileName = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "..", "Platform.IO.Tests.TemporaryFileTest", "bin", "Debug", "net5", "Platform.IO.Tests.TemporaryFileTest"));
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.Start();
            auto path = process.StandardOutput.ReadLine();
            Assert::IsTrue(File.Exists(path));
            process.WaitForExit();
            Assert::IsFalse(File.Exists(path));
        }

        public: TEST_METHOD(TemporaryFileTestWithoutConsoleApp)
        {
            std::string fileName = 0;
            using (TemporaryFile tempFile = new())
            {
                fileName = tempFile;
                Assert::IsTrue(File.Exists(fileName));
            }
            Assert::IsFalse(File.Exists(fileName));
        }
    };
}
