namespace Platform::IO::Tests
{
    TEST_CLASS(FileHelpersTests)
    {
        public: TEST_METHOD(WriteReadTest)
        {
            auto temporaryFile = Path.GetTempFileName();
            auto originalValue = 42UL;
            FileHelpers.WriteFirst(temporaryFile, originalValue);
            auto readValue = FileHelpers.ReadFirstOrDefault<std::uint64_t>(temporaryFile);
            Assert::AreEqual(readValue, originalValue);
            File.Delete(temporaryFile);
        }
    };
}
