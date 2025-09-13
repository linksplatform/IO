#pragma once
#include <iostream>
#include <fstream>
#include <vector>
#include <cstdint>
#include <type_traits>

namespace Platform::IO
{
    /// <summary>
    /// <para>Represents the set of extension methods for stream class instances.</para>
    /// <para>Представляет набор методов расширения для экземпляров класса потока.</para>
    /// </summary>
    class StreamExtensions
    {
    public:
        /// <summary>
        /// <para>Writes a byte sequence that represents the T structure value to the stream and moves the current position of the stream by the number of written bytes.</para>
        /// <para>Записывает последовательность байт представляющую value структуры типа T в поток stream и перемещает текущую позицию в stream вперёд на число записанных байт.</para>
        /// </summary>
        template<typename T>
        static void Write(std::ostream& stream, const T& value)
        {
            static_assert(std::is_trivially_copyable_v<T>, "T must be trivially copyable");
            stream.write(reinterpret_cast<const char*>(&value), sizeof(T));
        }

        /// <summary>
        /// <para>Reads a byte sequence that represents the T structure value and moves the current position of the stream by the number of read bytes.</para>
        /// <para>Считывает последовательность байт представляющих значение структуры типа T и перемещает текущую позицию в потоке stream вперёд на число прочитанных байт.</para>
        /// </summary>
        template<typename T>
        static T ReadOrDefault(std::istream& stream)
        {
            static_assert(std::is_trivially_copyable_v<T>, "T must be trivially copyable");
            
            T value{};
            if (stream.read(reinterpret_cast<char*>(&value), sizeof(T))) {
                return value;
            }
            return T{};
        }

        /// <summary>
        /// <para>Reads and returns all T structure values array from the stream.</para>
        /// <para>Прочитывает и возвращает массив всех значений структур типа T из потока stream.</para>
        /// </summary>
        template<typename T>
        static std::vector<T> ReadAll(std::istream& stream)
        {
            static_assert(std::is_trivially_copyable_v<T>, "T must be trivially copyable");
            
            // Get current position
            auto currentPos = stream.tellg();
            
            // Seek to end to get total size
            stream.seekg(0, std::ios::end);
            auto totalSize = stream.tellg();
            
            // Seek back to start or current position
            stream.seekg(currentPos);
            
            // Calculate remaining bytes and number of elements
            auto remainingBytes = totalSize - currentPos;
            auto elementSize = sizeof(T);
            auto numElements = remainingBytes / elementSize;
            
            std::vector<T> elements;
            elements.reserve(static_cast<std::size_t>(numElements));
            
            for (auto i = 0; i < numElements; ++i) {
                T element{};
                if (stream.read(reinterpret_cast<char*>(&element), elementSize)) {
                    elements.push_back(element);
                } else {
                    break;
                }
            }
            
            return elements;
        }

        /// <summary>
        /// <para>Writes a byte sequence that represents the T structure value to the file stream and moves the current position of the stream by the number of written bytes.</para>
        /// <para>Записывает последовательность байт представляющую value структуры типа T в файловый поток и перемещает текущую позицию в потоке вперёд на число записанных байт.</para>
        /// </summary>
        template<typename T>
        static void Write(std::fstream& stream, const T& value)
        {
            static_assert(std::is_trivially_copyable_v<T>, "T must be trivially copyable");
            stream.write(reinterpret_cast<const char*>(&value), sizeof(T));
        }

        /// <summary>
        /// <para>Reads a byte sequence that represents the T structure value from the file stream and moves the current position of the stream by the number of read bytes.</para>
        /// <para>Считывает последовательность байт представляющих значение структуры типа T из файлового потока и перемещает текущую позицию в потоке вперёд на число прочитанных байт.</para>
        /// </summary>
        template<typename T>
        static T ReadOrDefault(std::fstream& stream)
        {
            static_assert(std::is_trivially_copyable_v<T>, "T must be trivially copyable");
            
            T value{};
            if (stream.read(reinterpret_cast<char*>(&value), sizeof(T))) {
                return value;
            }
            return T{};
        }

        /// <summary>
        /// <para>Reads and returns all T structure values array from the file stream.</para>
        /// <para>Prочитывает и возвращает массив всех значений структур типа T из файлового потока.</para>
        /// </summary>
        template<typename T>
        static std::vector<T> ReadAll(std::fstream& stream)
        {
            static_assert(std::is_trivially_copyable_v<T>, "T must be trivially copyable");
            
            // Get current position
            auto currentPos = stream.tellg();
            
            // Seek to end to get total size
            stream.seekg(0, std::ios::end);
            auto totalSize = stream.tellg();
            
            // Seek back to start or current position
            stream.seekg(currentPos);
            
            // Calculate remaining bytes and number of elements
            auto remainingBytes = totalSize - currentPos;
            auto elementSize = sizeof(T);
            auto numElements = remainingBytes / elementSize;
            
            std::vector<T> elements;
            elements.reserve(static_cast<std::size_t>(numElements));
            
            for (auto i = 0; i < numElements; ++i) {
                T element{};
                if (stream.read(reinterpret_cast<char*>(&element), elementSize)) {
                    elements.push_back(element);
                } else {
                    break;
                }
            }
            
            return elements;
        }
    };
}
