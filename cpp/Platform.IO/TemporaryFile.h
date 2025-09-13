#pragma once
#include <string>
#include <filesystem>
#include "TemporaryFiles.h"

namespace Platform::IO
{
    /// <summary>
    /// <para>Represents a self-deleting temporary file.</para>
    /// <para>Представляет самоудаляющийся временный файл.</para>
    /// </summary>
    class TemporaryFile
    {
    public:
        /// <summary>
        /// <para>Gets a temporary file path.</para>
        /// <para>Возвращает путь к временному файлу.</para>
        /// </summary>
        const std::string Filename;

        /// <summary>
        /// <para>Converts the TemporaryFile instance to string using the Filename field value.</para>
        /// <para>Преобразует экземпляр TemporaryFile в string используя поле Filename.</para>
        /// </summary>
        operator const std::string&() const 
        { 
            return Filename; 
        }

        /// <summary>
        /// <para>Gets the filename as a C-style string.</para>
        /// <para>Возвращает имя файла как C-style строку.</para>
        /// </summary>
        const char* c_str() const 
        { 
            return Filename.c_str(); 
        }

        /// <summary>
        /// <para>Initializes a TemporaryFile instance.</para>
        /// <para>Инициализирует экземпляр класса TemporaryFile.</para>
        /// </summary>
        TemporaryFile() : Filename(TemporaryFiles::UseNew()) 
        {
        }

        /// <summary>
        /// <para>Copy constructor (deleted to prevent copying).</para>
        /// <para>Конструктор копирования (удален для предотвращения копирования).</para>
        /// </summary>
        TemporaryFile(const TemporaryFile&) = delete;

        /// <summary>
        /// <para>Copy assignment operator (deleted to prevent copying).</para>
        /// <para>Оператор присваивания копированием (удален для предотвращения копирования).</para>
        /// </summary>
        TemporaryFile& operator=(const TemporaryFile&) = delete;

        /// <summary>
        /// <para>Move constructor.</para>
        /// <para>Конструктор перемещения.</para>
        /// </summary>
        TemporaryFile(TemporaryFile&& other) noexcept : Filename(std::move(other.Filename))
        {
            // The moved-from object should not delete the file
            const_cast<std::string&>(other.Filename).clear();
        }

        /// <summary>
        /// <para>Move assignment operator.</para>
        /// <para>Оператор присваивания перемещением.</para>
        /// </summary>
        TemporaryFile& operator=(TemporaryFile&& other) noexcept
        {
            if (this != &other) {
                // Delete current file if it exists
                if (!Filename.empty() && std::filesystem::exists(Filename)) {
                    try {
                        std::filesystem::remove(Filename);
                    } catch (...) {
                        // Ignore deletion errors in move assignment
                    }
                }
                
                // Take ownership of the other file
                const_cast<std::string&>(Filename) = std::move(other.Filename);
                const_cast<std::string&>(other.Filename).clear();
            }
            return *this;
        }

        /// <summary>
        /// <para>Deletes the temporary file.</para>
        /// <para>Удаляет временный файл.</para>
        /// </summary>
        ~TemporaryFile()
        {
            if (!Filename.empty() && std::filesystem::exists(Filename)) {
                try {
                    std::filesystem::remove(Filename);
                } catch (...) {
                    // Ignore deletion errors in destructor
                }
            }
        }
    };
}
