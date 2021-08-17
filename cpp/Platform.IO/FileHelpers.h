namespace Platform::IO
{
    class FileHelpers
    {
        public: static char ReadAllChars[](std::string path) { return File.ReadAllText(path)->ToCharArray(); }

        public: static T ReadAll[]<T>(std::string path)
            where T : struct
        {
            using auto reader = File.OpenRead(path);
            return reader.ReadAll<T>();
        }

        public: template <typename T> static T ReadFirstOrDefault(std::string path)
            where T : struct
        {
            using auto fileStream = GetValidFileStreamOrDefault<T>(path);
            return fileStream?.ReadOrDefault<T>() ?? 0;
        }

        private: template <typename TStruct> static FileStream GetValidFileStreamOrDefault(std::string path) where TStruct : struct => GetValidFileStreamOrDefault(path, Structure<TStruct>.Size);

        private: static FileStream GetValidFileStreamOrDefault(std::string path, std::int32_t elementSize)
        {
            if (!File.Exists(path))
            {
                return {};
            }
            auto fileSize = GetSize(path);
            if (fileSize % elementSize != 0)
            {
                throw std::runtime_error(std::string("File is not aligned to elements with size ").append(Platform::Converters::To<std::string>(elementSize)).append(1, '.'));
            }
            return fileSize > 0 ? File.OpenRead(path) : {};
        }

        public: template <typename T> static T ReadLastOrDefault(std::string path)
            where T : struct
        {
            auto elementSize = Structure<T>.Size;
            using auto reader = GetValidFileStreamOrDefault(path, elementSize);
            if (reader == nullptr)
            {
                return 0;
            }
            auto totalElements = reader.Length / elementSize;
            reader.Position = (totalElements - 1) * elementSize;
            return reader.ReadOrDefault<T>();
        }

        public: template <typename T> static void WriteFirst(std::string path, T value)
            where T : struct
        {
            using auto writer = File.OpenWrite(path);
            writer.Position = 0;
            writer.Write(value);
        }

        public: static FileStream Append(std::string path) { return File.Open(path, FileMode.Append, FileAccess.Write); }

        public: static std::int64_t GetSize(std::string path) { return File.Exists(path) ? FileInfo(path)->Length : 0; }

        public: static void SetSize(std::string path, std::int64_t size)
        {
            using auto fileStream = File.Open(path, FileMode.OpenOrCreate);
            if (fileStream.Length != size)
            {
                fileStream.SetLength(size);
            }
        }

        public: static void DeleteAll(std::string directory) { DeleteAll(directory, "*"); }

        public: static void DeleteAll(std::string directory, std::string searchPattern) { DeleteAll(directory, searchPattern, SearchOption.TopDirectoryOnly); }

        public: static void DeleteAll(std::string directory, std::string searchPattern, SearchOption searchOption)
        {
            foreach (auto file in Directory.EnumerateFiles(directory, searchPattern, searchOption))
            {
                File.Delete(file);
            }
        }

        public: static void Truncate(std::string path) { File.Open(path, FileMode.Truncate).Dispose(); }

        public: static void AppendLine(std::string path, std::string content)
        {
            using auto writer = File.AppendText(path);
            writer.WriteLine(content);
        }

        public: static void EachLine(std::string path, std::function<void(std::string)> action)
        {
            using auto reader = StreamReader(path);
            std::string line = 0;
            while ((line = reader.ReadLine()) != {})
            {
                action(line);
            }
        }
    };
}
