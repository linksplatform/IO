#pragma once
#include <string>
#include <filesystem>
#include <mutex>
#include <random>
#include <sstream>
#include "FileHelpers.h"

namespace Platform::IO
{
    /// <summary>
    /// <para>Represents the set of helper methods to work with temporary files.</para>
    /// <para>Представляет набор вспомогательных методов для работы с временными файлами.</para>
    /// </summary>
    class TemporaryFiles
    {
    private:
        inline static const std::string UserFilesListFileNamePrefix = ".used-temporary-files.txt";
        inline static std::mutex UsedFilesListMutex;

        static std::string GetUsedFilesListFilename()
        {
            // Get current executable path
            auto executablePath = std::filesystem::current_path();
            return executablePath.string() + "/" + UserFilesListFileNamePrefix;
        }

        static void AddToUsedFilesList(const std::string& filename)
        {
            std::lock_guard<std::mutex> lock(UsedFilesListMutex);
            auto listFilename = GetUsedFilesListFilename();
            FileHelpers::AppendLine(listFilename, filename);
        }

        static std::string GenerateTempFileName()
        {
            // Generate a unique temporary file name
            static std::random_device rd;
            static std::mt19937 gen(rd());
            static std::uniform_int_distribution<> dis(100000, 999999);
            
            std::ostringstream oss;
            oss << std::filesystem::temp_directory_path().string() 
                << "/tmp" << dis(gen) << ".tmp";
            return oss.str();
        }

    public:
        /// <summary>
        /// <para>Gets a temporary file and adds it to the used files list.</para>
        /// <para>Получает временный файл и добавляет его в список использованных файлов.</para>
        /// </summary>
        /// <returns>
        /// <para>The temporary file path.</para>
        /// <para>Путь временного файла.</para>
        /// </returns>
        static std::string UseNew()
        {
            auto filename = GenerateTempFileName();
            
            // Create the file to ensure it exists
            std::ofstream file(filename);
            file.close();
            
            AddToUsedFilesList(filename);
            return filename;
        }

        /// <summary>
        /// <para>Deletes all previously used temporary files and clears the files list.</para>
        /// <para>Удаляет все ранее использованные временные файлы и очищает список файлов.</para>
        /// </summary>
        static void DeleteAllPreviouslyUsed()
        {
            std::lock_guard<std::mutex> lock(UsedFilesListMutex);
            auto listFilename = GetUsedFilesListFilename();
            
            if (std::filesystem::exists(listFilename))
            {
                // Delete each file listed in the used files list
                FileHelpers::EachLine(listFilename, [](const std::string& line) {
                    if (!line.empty() && std::filesystem::exists(line)) {
                        try {
                            std::filesystem::remove(line);
                        } catch (const std::filesystem::filesystem_error&) {
                            // Ignore errors when deleting individual files
                        }
                    }
                });
                
                // Clear the list file
                FileHelpers::Truncate(listFilename);
            }
        }
    };
}
