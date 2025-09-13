#pragma once
#include <iostream>
#include <string>
#include <vector>
#include <cstdint>
#include <sstream>
#include <algorithm>
#include <cctype>

namespace Platform::IO
{
    /// <summary>
    /// <para>Represents the set of helper methods to work with the console.</para>
    /// <para>Представляет набор вспомогательных методов для работы с консолью.</para>
    /// </summary>
    class ConsoleHelpers
    {
    public:
        /// <summary>
        /// <para>Requests and expects a user to press any key in the console.</para>
        /// <para>Запрашивает и ожидает нажатие любой клавиши пользователем в консоли.</para>
        /// </summary>
        static void PressAnyKeyToContinue()
        {
            std::cout << "Press any key to continue." << std::endl;
            std::cin.get();
        }

        /// <summary>
        /// <para>Gets an argument's value with the specified index from the args array and if it's absent requests a user to input it in the console.</para>
        /// <para>Получает значение аргумента с указанным index из массива args, a если оно отсутствует запрашивает его ввод в консоли у пользователя.</para>
        /// </summary>
        static std::string GetOrReadArgument(std::int32_t index, const std::vector<std::string>& args)
        {
            return GetOrReadArgument(index, std::to_string(index + 1) + " argument", args);
        }

        /// <summary>
        /// <para>Gets an argument's value with the specified index from the args array and if it's absent requests a user to input it in the console.</para>
        /// <para>Получает значение аргумента с указанным index из массива args, a если оно отсутствует запрашивает его ввод в консоли у пользователя.</para>
        /// </summary>
        static std::string GetOrReadArgument(std::int32_t index, const std::string& readMessage, const std::vector<std::string>& args)
        {
            std::string result;
            if (static_cast<std::size_t>(index) >= args.size() || args[index].empty())
            {
                std::cout << readMessage << ": ";
                std::getline(std::cin, result);
            }
            else
            {
                result = args[index];
            }
            
            if (result.empty())
            {
                return "";
            }
            
            // Trim whitespace and quotes
            result = Trim(result);
            result = TrimSingle(result, '"');
            result = Trim(result);
            return result;
        }

        /// <summary>
        /// <para>Outputs the string to the console.</para>
        /// <para>Выводит string в консоль.</para>
        /// </summary>
        /// <remarks>
        /// <para>The method is only executed if the application was compiled with the DEBUG directive.</para>
        /// <para>Метод выполняется только если приложение было скомпилировано с директивой DEBUG.</para>
        /// </remarks>
        static void Debug(const std::string& str)
        {
#ifndef NDEBUG
            std::cout << str << std::endl;
#endif
        }

        /// <summary>
        /// <para>Writes text representations of the specified args using the specified format, followed by the current line terminator.</para>
        /// <para>Записывает текстовые представления объектов заданного массива args, в стандартный выходной поток с использованием заданного format, за которым следует текущий признак конца строки.</para>
        /// </summary>
        /// <remarks>
        /// <para>The method is only executed if the application was compiled with the DEBUG directive.</para>
        /// <para>Метод выполняется только если приложение было скомпилировано с директивой DEBUG.</para>
        /// </remarks>
        template<typename... Args>
        static void Debug(const std::string& format, Args&&... args)
        {
#ifndef NDEBUG
            std::ostringstream oss;
            FormatString(oss, format, std::forward<Args>(args)...);
            std::cout << oss.str() << std::endl;
#endif
        }

    private:
        // Helper function to trim whitespace from both ends
        static std::string Trim(const std::string& str)
        {
            auto start = str.begin();
            while (start != str.end() && std::isspace(*start))
                start++;

            auto end = str.end();
            do {
                end--;
            } while (std::distance(start, end) > 0 && std::isspace(*end));

            return std::string(start, end + 1);
        }

        // Helper function to trim a single character from both ends
        static std::string TrimSingle(const std::string& str, char ch)
        {
            if (str.empty()) return str;
            
            std::size_t start = 0;
            if (str[start] == ch) start++;
            
            std::size_t end = str.length();
            if (end > start && str[end - 1] == ch) end--;
            
            return str.substr(start, end - start);
        }

        // Helper function for string formatting (simple implementation)
        template<typename T>
        static void FormatString(std::ostringstream& oss, const std::string& format, T&& value)
        {
            oss << format << " " << std::forward<T>(value);
        }

        template<typename T, typename... Args>
        static void FormatString(std::ostringstream& oss, const std::string& format, T&& value, Args&&... args)
        {
            oss << std::forward<T>(value) << " ";
            FormatString(oss, format, std::forward<Args>(args)...);
        }
    };
}
