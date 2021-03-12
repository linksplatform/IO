using System;
using System.IO;
using System.Runtime.CompilerServices;
using Platform.Unsafe;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.IO
{
    /// <summary>
    /// <para>Provides a set of helper methods to work with files.</para>
    /// <para>Предоставляет набор вспомогательных методов для работы с файлами.</para>
    /// </summary>
    public static class FileHelpers
    {
        /// <summary>
        /// <para>Reads all the text and returns character array from the <paramref name="path"/>.</para>
        /// <para>Читает весь текст и возвращает массив символов из <paramref name="path"/>.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to the file, from which to read the character array.</para>
        /// <para>Путь к файлу, из которого нужно прочитать массив символов.</para>
        /// </param>
        /// <returns>
        /// <para>The character array from the <paramref name="path"/>.</para>
        /// <para>Массив символов из <paramref name="path"/>.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char[] ReadAllChars(string path) => File.ReadAllText(path).ToCharArray();

        /// <summary>
        /// <para>Reads and returns all <typeparamref name="T"/> structures from <paramref name="path"/>.</para>
        /// <para>Считывает и возвращает все структуры типа <typeparamref name="T"/> из <paramref name="path"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип являющийся структурой.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to the file, from which to read array of <typeparamref name="T"/> structures.</para>
        /// <para>Путь к файлу, из которого нужно прочитать массив структур типа <typeparamref name="T"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The <typeparamref name="T"/> structures array.</para>
        /// <para>Массив структур типа <typeparamref name="T"/>.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ReadAll<T>(string path)
            where T : struct
        {
            using var reader = File.OpenRead(path);
            return reader.ReadAll<T>();
        }

        /// <summary>
        /// <para>Reads and returns <typeparamref name="T"/> struct from <paramref name="path"/>.</para>
        /// <para>Считывает и возвращает структуру типа <typeparamref name="T"/> из <paramref name="path"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип являющийся структурой.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to the file, from which to read the value of <typeparamref name="T"/> structure.</para>
        /// <para>Путь к файлу, из которого нужно прочитать значение структуры типа <typeparamref name="T"/>.</para>
        /// </param>
        /// <returns>
        /// <para>A <typeparamref name="T"/> struct value if read from <paramref name="path"/> is successfull, otherwise the default <typeparamref name="T"/> struct value.</para>
        /// <para>Значение структуры типа <typeparamref name="T"/> если чтение из <paramref name="path"/> прошло успешно, иначе значение структуры типа <typeparamref name="T"/> по умолчанию.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadFirstOrDefault<T>(string path)
            where T : struct
        {
            using var fileStream = GetValidFileStreamOrDefault<T>(path);
            return fileStream?.ReadOrDefault<T>() ?? default;
        }

        /// <summary>
        /// <para>Validates the file from <paramref name="path"/>.</para>
        /// <para>Валидирует файл по пути <paramref name="path"/>.</para>
        /// </summary>
        /// <typeparam name="TStruct">
        /// <para>The structure type.</para>
        /// <para>Тип являющийся структурой.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to the file, that has to pass the validation.</para>
        /// <para>Путь к файлу, который должен пройти валидацию.</para>
        /// </param>
        /// <returns>
        /// <para>A read only <see cref="FileStream"/> upon successfull validation, otherwise null.</para>
        /// <para><see cref="FileStream"/> доступный только для чтения в случае успешной валидации, иначе null.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static FileStream GetValidFileStreamOrDefault<TStruct>(string path) where TStruct : struct => GetValidFileStreamOrDefault(path, Structure<TStruct>.Size);

        /// <summary>
        /// <para>Validates the file from <paramref name="path"/>.</para>
        /// <para>Валидирует файл по пути <paramref name="path"/>.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to the file, that has to pass the validation.</para>
        /// <para>Путь к файлу, который должен пройти валидацию.</para>
        /// </param>
        /// <param name="elementSize">
        /// <para>The size of the file to be in <paramref name="path"/>.</para>
        /// <para>Размер файла, который должен находиться в <paramref name="path"/>.</para>
        /// </param>
        /// <returns>
        /// <para>A read only <see cref="FileStream"/> upon successfull validation, otherwise null.</para>
        /// <para><see cref="FileStream"/> доступный только для чтения в случае успешной валидации, иначе null.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static FileStream GetValidFileStreamOrDefault(string path, int elementSize)
        {
            if (!File.Exists(path))
            {
                return null;
            }
            var fileSize = GetSize(path);
            if (fileSize % elementSize != 0)
            {
                /// <exception cref="InvalidOperationException">
                /// <para>Throws the error, if the file is not aligned to elements with size <paramref name="elementSize"/>.</para>
                /// <para>Выбрасывает ошибку, если файл не соответсвует элементу размером <paramref name="elementSize"/>.</para>
                /// </exception>
                throw new InvalidOperationException($"File is not aligned to elements with size {elementSize}.");
            }
            return fileSize > 0 ? File.OpenRead(path) : null;
        }

        /// <summary>
        /// <para>Reads and returns the last <typeparamref name="T"/> structure value from <typeparamref name="T"/>.</para>
        /// <para>Считывает и возвращает последнее значение структуры типа <typeparamref name="T"/> из <paramref name="path"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип являющийся структурой.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to the <typeparamref name="T"/> structure values.</para>
        /// <para>Путь к файлу с значениями структур типа <typeparamref name="T"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The <typeparamref name="T"/> structure value in case of success, otherwise the default <typeparamref name="T"/> structure value.</para>
        /// <para>Значение структуры типа <typeparamref name="T"/> в случае успеха, иначе значение по умолчанию структуры типа <typeparamref name="T"/>.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadLastOrDefault<T>(string path)
            where T : struct
        {
            var elementSize = Structure<T>.Size;
            using var reader = GetValidFileStreamOrDefault(path, elementSize);
            if (reader == null)
            {
                return default;
            }
            var totalElements = reader.Length / elementSize;
            reader.Position = (totalElements - 1) * elementSize; // Set to last element
            return reader.ReadOrDefault<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFirst<T>(string path, T value)
            where T : struct
        {
            using var writer = File.OpenWrite(path);
            writer.Position = 0;
            writer.Write(value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FileStream Append(string path) => File.Open(path, FileMode.Append, FileAccess.Write);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetSize(string path) => File.Exists(path) ? new FileInfo(path).Length : 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSize(string path, long size)
        {
            using var fileStream = File.Open(path, FileMode.OpenOrCreate);
            if (fileStream.Length != size)
            {
                fileStream.SetLength(size);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAll(string directory) => DeleteAll(directory, "*");

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAll(string directory, string searchPattern) => DeleteAll(directory, searchPattern, SearchOption.TopDirectoryOnly);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAll(string directory, string searchPattern, SearchOption searchOption)
        {
            foreach (var file in Directory.EnumerateFiles(directory, searchPattern, searchOption))
            {
                File.Delete(file);
            }
        }
    }
}
