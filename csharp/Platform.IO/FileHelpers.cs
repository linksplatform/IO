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
        /// <para>Reads all the text and returns character array from the file at <paramref name="path"/>.</para>
        /// <para>Читает весь текст и возвращает массив символов из файла по пути<paramref name="path"/>.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to the file, from which to read the character array.</para>
        /// <para>Путь к файлу, из которого нужно прочитать массив символов.</para>
        /// </param>
        /// <returns>
        /// <para>The character array from the file at <paramref name="path"/>.</para>
        /// <para>Массив символов из файла по пути <paramref name="path"/>.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char[] ReadAllChars(string path) => File.ReadAllText(path).ToCharArray();

        /// <summary>
        /// <para>Reads and returns all <typeparamref name="T"/> structure values from the file at <paramref name="path"/>.</para>
        /// <para>Считывает и возвращает все значения структур типа <typeparamref name="T"/> из файла по пути <paramref name="path"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип структуры.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to the file, from which to read array of <typeparamref name="T"/> structure values.</para>
        /// <para>Путь к файлу, из которого нужно прочитать массив значений структур типа <typeparamref name="T"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The <typeparamref name="T"/> structure values array.</para>
        /// <para>Массив значений структур типа <typeparamref name="T"/>.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ReadAll<T>(string path)
            where T : struct
        {
            using var reader = File.OpenRead(path);
            return reader.ReadAll<T>();
        }

        /// <summary>
        /// <para>Reads and returns the <typeparamref name="T"/> structure value from the file at <paramref name="path"/>.</para>
        /// <para>Считывает и возвращает значение структуры типа <typeparamref name="T"/> из файла по пути <paramref name="path"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип структуры.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to the file, from which to read the <typeparamref name="T"/> structure value.</para>
        /// <para>Путь к файлу, из которого нужно прочитать значение структуры типа <typeparamref name="T"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The <typeparamref name="T"/> structure value if read from the file at <paramref name="path"/> is successfull, otherwise the default <typeparamref name="T"/> structure value.</para>
        /// <para>Значение структуры типа <typeparamref name="T"/> если чтение из файла по пути<paramref name="path"/> прошло успешно, иначе значение структуры типа <typeparamref name="T"/> по умолчанию.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadFirstOrDefault<T>(string path)
            where T : struct
        {
            using var fileStream = GetValidFileStreamOrDefault<T>(path);
            return fileStream?.ReadOrDefault<T>() ?? default;
        }

        /// <summary>
        /// <para>Returns the <see cref="FileStream"/> with opened file from the file at <paramref name="path"/> if its size is a multiple of the <typeparamref name="TStruct"/> structure value size, otherwise <see langword="null"/>.</para>
        /// <para>Возвращает <see cref="FileStream"/> с открытым файлом из файла по пути <paramref name="path"/>, если его размер кратен размеру значения структуры типа <typeparamref name="TStruct"/>, а иначе <see langword="null"/>.</para>
        /// </summary>
        /// <typeparam name="TStruct">
        /// <para>The structure type.</para>
        /// <para>Тип структуры.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to the file to validate.</para>
        /// <para>Путь к проверяемому файлу.</para>
        /// </param>
        /// <returns>
        /// <para>A read-only <see cref="FileStream"/> in the case of successful check, otherwise <see langword="null"/>.</para>
        /// <para><see cref="FileStream"/> открытый для чтения в случае успешной проверки, а иначе <see langword="null"/>.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static FileStream GetValidFileStreamOrDefault<TStruct>(string path) where TStruct : struct => GetValidFileStreamOrDefault(path, Structure<TStruct>.Size);

        /// <summary>
        /// <para>Returns the <see cref="FileStream"/> opened for reading from the file at <paramref name="path"/> if its size is a multiple of the required <paramref name="elementSize"/>, otherwise <see langword="null"/>.</para>
        /// <para>Возвращает открытый для чтения <see cref="FileStream"/> из файла по пути<paramref name="path"/>, если его размер кратен требуемому размеру элементов <paramref name="elementSize"/>, а иначе <see langword="null"/>.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to the file to be scanned.</para>
        /// <para>Путь к проверяемому файлу.</para>
        /// </param>
        /// <param name="elementSize">
        /// <para>Required size of elements located in the file at <paramref name="path"/>.</para>
        /// <para>Требуемый размер элементов, находящихся в файле по пути <paramref name="path"/>.</para>
        /// </param>
        /// <returns>
        /// <para>A read-only <see cref="FileStream"/> in the case of successful check, otherwise <see langword="null"/>.</para>
        /// <para><see cref="FileStream"/> открытый для чтения в случае успешной проверки, а иначе <see langword="null"/>.</para>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// <para>The size of the file at <paramref name="path"/> is not a multiple of the required <paramref name="elementSize"/>.</para>
        /// <para>Размер файла по пути <paramref name="path"/> не кратен требуемому размеру элемента <paramref name="elementSize"/>.</para>
        /// </exception>
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
                throw new InvalidOperationException($"File is not aligned to elements with size {elementSize}.");
            }
            return fileSize > 0 ? File.OpenRead(path) : null;
        }

        /// <summary>
        /// <para>Reads and returns the last <typeparamref name="T"/> structure value from the file at <paramref name="path"/>.</para>
        /// <para>Считывает и возвращает последнее значение структуры типа <typeparamref name="T"/> из файла по пути <paramref name="path"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип структуры.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to the <typeparamref name="T"/> structure values.</para>
        /// <para>Путь к файлу с значениями структур типа <typeparamref name="T"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The last <typeparamref name="T"/> structure value from the file at <paramref name="path"/> in the case of success, otherwise the default <typeparamref name="T"/> structure value.</para>
        /// <para>Значение структуры типа <typeparamref name="T"/> из файла по пути <paramref name="path"/> в случае успеха, иначе значение по умолчанию структуры типа <typeparamref name="T"/>.</para>
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

        /// <summary>
        /// <para>Writes <typeparamref name="T"/> structure values at the beginning of the file at <paramref name="path"/>.</para>
        /// <para>Записывает значения структур типа <typeparamref name="T"/> в начало файла по пути <paramref name="path"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип структуры.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to the file to be changed or created.</para>
        /// <para>Путь к файлу для изменения или создания.</para>
        /// </param>
        /// <param name="value">
        /// <para><typeparamref name="T"/> structure values to be written at the beginning of the file at <paramref name="path"/>.</para>
        /// <para>Значения структур типа <typeparamref name="T"/>, записываемых в начало файла по пути <paramref name="path"/>.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteFirst<T>(string path, T value)
            where T : struct
        {
            using var writer = File.OpenWrite(path);
            writer.Position = 0;
            writer.Write(value);
        }

        /// <summary>
        /// <para>Opens or creates the file at <paramref name="path"/> and returns its <see cref="FileStream"/> with append mode and write access.</para>
        /// <para>Открывает или создает файл по пути <paramref name="path"/> и возвращает его <see cref="FileStream"/> с модом добавления в конец и доступом записи.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to the file to open or create.</para>
        /// <para>Путь к файлу, который нужно открыть или создать.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="FileStream"/> with append mode and write access.</para>
        /// <para><see cref="FileStream"/> с модом добавления в конец и доступом записи.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FileStream Append(string path) => File.Open(path, FileMode.Append, FileAccess.Write);

        /// <summary>
        /// <para>Returns the size of file at <paramref name="path"/> if file exists, otherwise 0.</para>
        /// <para>Возвращает размер файла по пути <paramref name="path"/> если тот существует, иначе 0.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to the file to get size.</para>
        /// <para>Путь к файлу, размер которого нужно получить.</para>
        /// </param>
        /// <returns>
        /// <para>Size of file at <paramref name="path"/> if it exists, otherwise 0.</para>
        /// <para>Размер файла если файл по пути <paramref name="path"/> существует, либо 0.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetSize(string path) => File.Exists(path) ? new FileInfo(path).Length : 0;

        /// <summary>
        /// <para>Sets the <paramref name="size"/> for the file at <paramref name="path"/>.</para>
        /// <para>Устанавливает размер <paramref name="size"/> файлу по пути <paramref name="path"/>.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to the file to be resized.</para>
        /// <para>Путь к файлу, размер которого нужно изменить.</para>
        /// </param>
        /// <param name="size">
        /// <para>The size to assign to the file at <paramref name="path"/>.</para>
        /// <para>Размер который будет присвоен файлу по пути <paramref name="path"/>.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetSize(string path, long size)
        {
            using var fileStream = File.Open(path, FileMode.OpenOrCreate);
            if (fileStream.Length != size)
            {
                fileStream.SetLength(size);
            }
        }

        /// <summary>
        /// <para>Removes all files from the <paramref name="directory"/>.</para>
        /// <para>Удаляет все файлы из директории <paramref name="directory"/>.</para>
        /// </summary>
        /// <param name="directory">
        /// <para>The directory to be cleaned.</para>
        /// <para>Директория для очистки.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAll(string directory) => DeleteAll(directory, "*");

        /// <summary>
        /// <para>Removes all files from the <paramref name="directory"/> according to the <paramref name="searchPattern"/>.</para>
        /// <para>Удаляет все файлы из директории <paramref name="directory"/> в соотвествии с шаблоном поиска <paramref name="searchPattern"/>.</para>
        /// </summary>
        /// <param name="directory">
        /// <para>The directory to be cleaned.</para>
        /// <para>Директория для очистки.</para>
        /// </param>
        /// <param name="searchPattern">
        /// <para>Search pattern for files in the <paramref name="directory"/>.</para>
        /// <para>Шаблон поиска для файлов в директории <paramref name="directory"/>.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAll(string directory, string searchPattern) => DeleteAll(directory, searchPattern, SearchOption.TopDirectoryOnly);

        /// <summary>
        /// <para>Removes all files from the directory <paramref name="directory"/> according to the <paramref name="searchPattern"/> and the <paramref name="searchOption"/>.</para>
        /// <para>Удаляет все файлы из директории <paramref name="directory"/> в соотвествии с шаблоном поиска <paramref name="searchPattern"/> и настройкой поиска <paramref name="searchOption"/>.</para>
        /// </summary>
        /// <param name="directory">
        /// <para>The directory to be cleaned.</para>
        /// <para>Директория для очистки.</para>
        /// </param>
        /// <param name="searchPattern">
        /// <para>Search pattern for files in the <paramref name="directory"/>.</para>
        /// <para>Шаблон поиска для файлов в директории <paramref name="directory"/>.</para>
        /// </param>
        /// <param name="searchOption">
        /// <para>Specifies whether to search only in the current <paramref name="directory"/>, or also in subdirectories.</para>
        /// <para>Указывает следует ли искать только в текущей директории <paramref name="directory"/>, или также в субдиректориях.</para>
        /// </param>
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
