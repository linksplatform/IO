#pragma once
#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <cstdint>
#include <filesystem>
#include <stdexcept>
#include <functional>
#include <memory>
#include <type_traits>

namespace Platform::IO
{
    /// <summary>
    /// <para>Represents the set of helper methods to work with files.</para>
    /// <para>Представляет набор вспомогательных методов для работы с файлами.</para>
    /// </summary>
    class FileHelpers
    {
    public:
        /// <summary>
        /// <para>Reads all the text and returns character array from a file at the path.</para>
        /// <para>Читает весь текст и возвращает массив символов из файла находящегося в path.</para>
        /// </summary>
        static std::vector<char> ReadAllChars(const std::string& path)
        {
            std::ifstream file(path, std::ios::binary);
            if (!file.is_open()) {
                throw std::runtime_error("Cannot open file: " + path);
            }
            
            std::vector<char> chars((std::istreambuf_iterator<char>(file)),
                                   std::istreambuf_iterator<char>());
            return chars;
        }

        /// <summary>
        /// <para>Reads and returns all T structure values from a file at the path.</para>
        /// <para>Считывает и возвращает все значения структур типа T из файла находящегося в path.</para>
        /// </summary>
        template<typename T>
        static std::vector<T> ReadAll(const std::string& path)
        {
            static_assert(std::is_trivially_copyable_v<T>, "T must be trivially copyable");
            
            std::ifstream file(path, std::ios::binary);
            if (!file.is_open()) {
                throw std::runtime_error("Cannot open file: " + path);
            }

            // Get file size
            file.seekg(0, std::ios::end);
            std::size_t fileSize = file.tellg();
            file.seekg(0, std::ios::beg);

            // Calculate number of elements
            std::size_t elementSize = sizeof(T);
            if (fileSize % elementSize != 0) {
                throw std::runtime_error("File size is not aligned to element size");
            }

            std::size_t numElements = fileSize / elementSize;
            std::vector<T> result(numElements);
            
            file.read(reinterpret_cast<char*>(result.data()), fileSize);
            return result;
        }

        /// <summary>
        /// <para>Reads and returns the first T structure value from a file at the path.</para>
        /// <para>Считывает и возвращает первое значение структуры типа T из файла находящегося в path.</para>
        /// </summary>
        template<typename T>
        static T ReadFirstOrDefault(const std::string& path)
        {
            static_assert(std::is_trivially_copyable_v<T>, "T must be trivially copyable");
            
            if (!std::filesystem::exists(path)) {
                return T{};
            }

            auto fileSize = GetSize(path);
            if (fileSize < static_cast<std::int64_t>(sizeof(T))) {
                return T{};
            }

            std::ifstream file(path, std::ios::binary);
            if (!file.is_open()) {
                return T{};
            }

            T result{};
            file.read(reinterpret_cast<char*>(&result), sizeof(T));
            return result;
        }

        /// <summary>
        /// <para>Reads and returns the last T structure value from a file at the path.</para>
        /// <para>Считывает и возвращает последнее значение структуры типа T из файла находящегося в path.</para>
        /// </summary>
        template<typename T>
        static T ReadLastOrDefault(const std::string& path)
        {
            static_assert(std::is_trivially_copyable_v<T>, "T must be trivially copyable");
            
            if (!std::filesystem::exists(path)) {
                return T{};
            }

            auto fileSize = GetSize(path);
            std::size_t elementSize = sizeof(T);
            
            if (fileSize < static_cast<std::int64_t>(elementSize)) {
                return T{};
            }

            if (fileSize % elementSize != 0) {
                throw std::runtime_error("File is not aligned to elements with size " + std::to_string(elementSize));
            }

            std::ifstream file(path, std::ios::binary);
            if (!file.is_open()) {
                return T{};
            }

            // Seek to last element
            auto totalElements = fileSize / elementSize;
            auto lastElementOffset = (totalElements - 1) * elementSize;
            file.seekg(lastElementOffset);

            T result{};
            file.read(reinterpret_cast<char*>(&result), sizeof(T));
            return result;
        }

        /// <summary>
        /// <para>Writes T structure value at the beginning of a file at the path.</para>
        /// <para>Записывает значение структуры типа T в начало файла находящегося в path.</para>
        /// </summary>
        template<typename T>
        static void WriteFirst(const std::string& path, const T& value)
        {
            static_assert(std::is_trivially_copyable_v<T>, "T must be trivially copyable");
            
            std::fstream file(path, std::ios::binary | std::ios::in | std::ios::out);
            if (!file.is_open()) {
                // Create new file if it doesn't exist
                file.open(path, std::ios::binary | std::ios::out);
                if (!file.is_open()) {
                    throw std::runtime_error("Cannot create or open file: " + path);
                }
            }

            file.seekp(0);
            file.write(reinterpret_cast<const char*>(&value), sizeof(T));
        }

        /// <summary>
        /// <para>Opens or creates a file at the path and returns its ofstream with append mode.</para>
        /// <para>Открывает или создаёт файл находящийся в path и возвращает его ofstream с режимом дополнения.</para>
        /// </summary>
        static std::unique_ptr<std::ofstream> Append(const std::string& path)
        {
            auto stream = std::make_unique<std::ofstream>(path, std::ios::app);
            if (!stream->is_open()) {
                throw std::runtime_error("Cannot open file for append: " + path);
            }
            return stream;
        }

        /// <summary>
        /// <para>Returns the size of a file at the path if the file exists; otherwise 0.</para>
        /// <para>Возвращает размер файла находящегося в path если тот существует, иначе 0.</para>
        /// </summary>
        static std::int64_t GetSize(const std::string& path)
        {
            if (!std::filesystem::exists(path)) {
                return 0;
            }
            return static_cast<std::int64_t>(std::filesystem::file_size(path));
        }

        /// <summary>
        /// <para>Sets the size for a file at the path.</para>
        /// <para>Устанавливает size файлу находящемуся по пути path.</para>
        /// </summary>
        static void SetSize(const std::string& path, std::int64_t size)
        {
            std::filesystem::resize_file(path, static_cast<std::uintmax_t>(size));
        }

        /// <summary>
        /// <para>Removes all files from the directory at the path directory.</para>
        /// <para>Удаляет все файлы из директории находящейся по пути directory.</para>
        /// </summary>
        static void DeleteAll(const std::string& directory)
        {
            DeleteAll(directory, "*");
        }

        /// <summary>
        /// <para>Removes files from the directory at the path directory according to the searchPattern.</para>
        /// <para>Удаляет файлы из директории находящейся по пути directory в соотвествии с searchPattern.</para>
        /// </summary>
        static void DeleteAll(const std::string& directory, const std::string& searchPattern)
        {
            DeleteAll(directory, searchPattern, false);
        }

        /// <summary>
        /// <para>Removes files from the directory at the path directory according to the searchPattern and the recursive option.</para>
        /// <para>Удаляет файлы из директории находящейся по пути directory в соотвествии с searchPattern и рекурсивной опцией.</para>
        /// </summary>
        static void DeleteAll(const std::string& directory, const std::string& searchPattern, bool recursive)
        {
            if (!std::filesystem::exists(directory)) {
                return;
            }

            try {
                if (recursive) {
                    for (const auto& entry : std::filesystem::recursive_directory_iterator(directory)) {
                        if (entry.is_regular_file() && MatchesPattern(entry.path().filename().string(), searchPattern)) {
                            std::filesystem::remove(entry.path());
                        }
                    }
                } else {
                    for (const auto& entry : std::filesystem::directory_iterator(directory)) {
                        if (entry.is_regular_file() && MatchesPattern(entry.path().filename().string(), searchPattern)) {
                            std::filesystem::remove(entry.path());
                        }
                    }
                }
            } catch (const std::filesystem::filesystem_error& e) {
                throw std::runtime_error("Error deleting files: " + std::string(e.what()));
            }
        }

        /// <summary>
        /// <para>Truncates the file at the path.</para>
        /// <para>Очищает содержимое файла по пути path.</para>
        /// </summary>
        static void Truncate(const std::string& path)
        {
            std::ofstream file(path, std::ios::trunc);
            // File is automatically closed when going out of scope
        }

        /// <summary>
        /// <para>Appends the content to a file at the path.</para>
        /// <para>Добавляет content в конец файла по пути path.</para>
        /// </summary>
        static void AppendLine(const std::string& path, const std::string& content)
        {
            std::ofstream file(path, std::ios::app);
            if (!file.is_open()) {
                throw std::runtime_error("Cannot open file for append: " + path);
            }
            file << content << std::endl;
        }

        /// <summary>
        /// <para>Performs the action for each line of a file at the path.</para>
        /// <para>Выполняет action для каждой строчки файла по пути path.</para>
        /// </summary>
        static void EachLine(const std::string& path, const std::function<void(const std::string&)>& action)
        {
            std::ifstream file(path);
            if (!file.is_open()) {
                throw std::runtime_error("Cannot open file: " + path);
            }

            std::string line;
            while (std::getline(file, line)) {
                action(line);
            }
        }

    private:
        // Simple pattern matching for wildcards (very basic implementation)
        static bool MatchesPattern(const std::string& filename, const std::string& pattern)
        {
            if (pattern == "*") {
                return true;
            }
            
            // For simplicity, only support exact match or "*" wildcard
            // A more sophisticated implementation would support proper glob patterns
            return filename == pattern;
        }
    };
}
