namespace Platform::IO
{
    class TemporaryFiles
    {
        private: inline static std::string UserFilesListFileNamePrefix = ".used-temporary-files.txt";
        private: inline static const void *UsedFilesListLockObject = new();
        private: inline static const std::string UsedFilesListFilename = Assembly.GetExecutingAssembly().Location + UserFilesListFileNamePrefix;

        private: static void AddToUsedFilesList(std::string filename)
        {
            lock (UsedFilesListLockObject)
            {
                FileHelpers.AppendLine(UsedFilesListFilename, filename);
            }
        }

        public: static std::string UseNew()
        {
            auto filename = Path.GetTempFileName();
            AddToUsedFilesList(filename);
            return filename;
        }

        public: static void DeleteAllPreviouslyUsed()
        {
            lock (UsedFilesListLockObject)
            {
                auto listFilename = UsedFilesListFilename;
                if (File.Exists(listFilename))
                {
                    FileHelpers.EachLine(listFilename, File.Delete);
                    FileHelpers.Truncate(listFilename);
                }
            }
        }
    };
}
