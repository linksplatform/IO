using System;
using System.IO;
using System.Runtime.CompilerServices;
using Platform.Unsafe;

namespace Platform.IO
{
    /// <summary>
    /// <para>Represents the set of helper methods to work with files.</para>
    /// <para>Представляет набор вспомогательных методов для работы с файлами.</para>
    /// </summary>
    public static class FileHelpers
    {
        /// <summary>
        /// <para>Reads all the text and returns character array from a file at the <paramref name="path"/>.</para>
        /// <para>Читает весь текст и возвращает массив символов из файла находящегося в <paramref name="path"/>.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to a file, from which to read the character array.</para>
        /// <para>Путь к файлу, из которого нужно прочитать массив символов.</para>
        /// </param>
        /// <returns>
        /// <para>The character array from a file at the <paramref name="path"/>.</para>
        /// <para>Массив символов из файла находящегося в <paramref name="path"/>.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char[] ReadAllChars(string path) => File.ReadAllText(path).ToCharArray();

        /// <summary>
        /// <para>Reads and returns all <typeparamref name="T"/> structure values from a file at the <paramref name="path"/>.</para>
        /// <para>Считывает и возвращает все значения структур типа <typeparamref name="T"/> из файла находящегося в <paramref name="path"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип структуры.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to a file, from which to read <typeparamref name="T"/> structure values array.</para>
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
        /// <para>Reads and returns the first <typeparamref name="T"/> structure value from a file at the <paramref name="path"/>.</para>
        /// <para>Считывает и возвращает первое значение структуры типа <typeparamref name="T"/> из файла находящегося в <paramref name="path"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип структуры.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to a file, from which to read the first <typeparamref name="T"/> structure value.</para>
        /// <para>Путь к файлу, из которого нужно прочитать значение первой структуры типа <typeparamref name="T"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The <typeparamref name="T"/> structure value if read from a file at the <paramref name="path"/> is successful; otherwise the default <typeparamref name="T"/> structure value.</para>
        /// <para>Значение структуры типа <typeparamref name="T"/> если чтение из файла находящегося в <paramref name="path"/> прошло успешно, иначе значение структуры типа <typeparamref name="T"/> по умолчанию.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadFirstOrDefault<T>(string path)
            where T : struct
        {
            using var fileStream = GetValidFileStreamOrDefault<T>(path);
            return fileStream?.ReadOrDefault<T>() ?? default;
        }

        /// <summary>
        /// <para>Returns the <see cref="FileStream"/> opened for reading from a file at the <paramref name="path"/> if the file exists, not empty and its size is a multiple of the <typeparamref name="TStruct"/> structure size; otherwise <see langword="null"/>.</para>
        /// <para>Возвращает <see cref="FileStream"/>, открытый для чтения из файла находящегося в <paramref name="path"/>, если файл существует, не пуст и его размер кратен размеру структуры типа <typeparamref name="TStruct"/>, а иначе <see langword="null"/>.</para>
        /// </summary>
        /// <typeparam name="TStruct">
        /// <para>The structure type.</para>
        /// <para>Тип структуры.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to a file to validate.</para>
        /// <para>Путь к проверяемому файлу.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="FileStream"/> opened for reading in the case of successful check; otherwise <see langword="null"/>.</para>
        /// <para><see cref="FileStream"/>, открытый для чтения в случае успешной проверки, а иначе <see langword="null"/>.</para>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// <para>The size of a file at the <paramref name="path"/> is not a multiple of the required <typeparamref name="TStruct"/> structure size.</para>
        /// <para>Размер файла находящегося в <paramref name="path"/> не кратен требуемому размеру структуры типа <typeparamref name="TStruct"/>.</para>
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static FileStream GetValidFileStreamOrDefault<TStruct>(string path) where TStruct : struct => GetValidFileStreamOrDefault(path, Structure<TStruct>.Size);

        /// <summary>
        /// <para>Returns the <see cref="FileStream"/> opened for reading from a file at the <paramref name="path"/> if the file exists, not empty and its size is a multiple of the required <paramref name="elementSize"/>; otherwise <see langword="null"/>.</para>
        /// <para>Возвращает <see cref="FileStream"/>, открытый для чтения из файла находящегося в <paramref name="path"/>, если файл существует, не пуст и его размер кратен <paramref name="elementSize"/>, а иначе <see langword="null"/>.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to a file to validate.</para>
        /// <para>Путь к проверяемому файлу.</para>
        /// </param>
        /// <param name="elementSize">
        /// <para>Required size of elements located in a file at the <paramref name="path"/>.</para>
        /// <para>Требуемый размер элементов, находящихся в файле находящегося в <paramref name="path"/>.</para>
        /// </param>
        /// <returns>
        /// <para>A <see cref="FileStream"/> opened for reading in the case of successful check; otherwise <see langword="null"/>.</para>
        /// <para><see cref="FileStream"/>, открытый для чтения в случае успешной проверки, а иначе <see langword="null"/>.</para>
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// <para>The size of a file at the <paramref name="path"/> is not a multiple of the required <paramref name="elementSize"/>.</para>
        /// <para>Размер файла находящегося в <paramref name="path"/> не кратен требуемому <paramref name="elementSize"/>.</para>
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
        /// <para>Reads and returns the last <typeparamref name="T"/> structure value from a file at the <paramref name="path"/>.</para>
        /// <para>Считывает и возвращает последнее значение структуры типа <typeparamref name="T"/> из файла находящегося в <paramref name="path"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип структуры.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to a file, from which to read the last <typeparamref name="T"/> structure value.</para>
        /// <para>Путь к файлу, из которого нужно прочитать значение последней структуры типа <typeparamref name="T"/>.</para>
        /// </param>
        /// <returns>
        /// <para>The <typeparamref name="T"/> structure value if read from a file at the <paramref name="path"/> is successful; otherwise the default <typeparamref name="T"/> structure value.</para>
        /// <para>Значение структуры типа <typeparamref name="T"/> из файла находящегося в <paramref name="path"/> в случае успешного чтения, иначе значение по умолчанию структуры типа <typeparamref name="T"/>.</para>
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
        /// <para>Writes <typeparamref name="T"/> structure value at the beginning of a file at the <paramref name="path"/>.</para>
        /// <para>Записывает значение структуры типа <typeparamref name="T"/> в начало файла находящегося в <paramref name="path"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип структуры.</para>
        /// </typeparam>
        /// <param name="path">
        /// <para>The path to a file to be changed or created.</para>
        /// <para>Путь к файлу, который будет изменён или создан.</para>
        /// </param>
        /// <param name="value">
        /// <para>The <typeparamref name="T"/> structure value to be written at the beginning of a file at the <paramref name="path"/>.</para>
        /// <para>Значение структуры типа <typeparamref name="T"/>, записываемое в начало файла находящегося в <paramref name="path"/>.</para>
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
        /// <para>Opens or creates a file at the <paramref name="path"/> and returns its <see cref="FileStream"/> with append mode and write access.</para>
        /// <para>Открывает или создаёт файл находящийся в <paramref name="path"/> и возвращает его <see cref="FileStream"/> с режимом дополнения и доступом на запись.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to a file to open or create.</para>
        /// <para>Путь к файлу, который нужно открыть или создать.</para>
        /// </param>
        /// <returns>
        /// <para>The <see cref="FileStream"/> with append mode and write access.</para>
        /// <para><see cref="FileStream"/> с режимом дополнения и доступом на запись.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static FileStream Append(string path) => File.Open(path, FileMode.Append, FileAccess.Write);

        /// <summary>
        /// <para>Returns the size of a file at the <paramref name="path"/> if the file exists; otherwise 0.</para>
        /// <para>Возвращает размер файла находящегося в <paramref name="path"/> если тот существует, иначе 0.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to a file to get size.</para>
        /// <para>Путь к файлу, размер которого нужно получить.</para>
        /// </param>
        /// <returns>
        /// <para>Size of a file at the <paramref name="path"/> if it exists; otherwise 0.</para>
        /// <para>Размер файла если файл находящийся в <paramref name="path"/> существует, иначе 0.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long GetSize(string path) => File.Exists(path) ? new FileInfo(path).Length : 0;

        /// <summary>
        /// <para>Sets the <paramref name="size"/> for a file at the <paramref name="path"/>.</para>
        /// <para>Устанавливает <paramref name="size"/> файлу находящемуся по пути <paramref name="path"/>.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to a file to be resized.</para>
        /// <para>Путь к файлу, размер которого нужно изменить.</para>
        /// </param>
        /// <param name="size">
        /// <para>The size to assign to a file at the <paramref name="path"/>.</para>
        /// <para>Размер который будет присвоен файлу находящемуся по пути <paramref name="path"/>.</para>
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
        /// <para>Removes all files from the directory at the path <paramref name="directory"/>.</para>
        /// <para>Удаляет все файлы из директории находящейся по пути <paramref name="directory"/>.</para>
        /// </summary>
        /// <param name="directory">
        /// <para>The path to the directory to be cleaned.</para>
        /// <para>Путь к директории для очистки.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAll(string directory) => DeleteAll(directory, "*");

        /// <summary>
        /// <para>Removes files from the directory at the path <paramref name="directory"/> according to the <paramref name="searchPattern"/>.</para>
        /// <para>Удаляет файлы из директории находящейся по пути <paramref name="directory"/> в соотвествии с <paramref name="searchPattern"/>.</para>
        /// </summary>
        /// <param name="directory">
        /// <para>The path to the directory to be cleaned.</para>
        /// <para>Путь к директории для очистки.</para>
        /// </param>
        /// <param name="searchPattern">
        /// <para>The search pattern for files to be deleted in the directory at the path <paramref name="directory"/>.</para>
        /// <para>Шаблон поиска для удаляемых файлов в директории находящейся по пути <paramref name="directory"/>.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAll(string directory, string searchPattern) => DeleteAll(directory, searchPattern, SearchOption.TopDirectoryOnly);

        /// <summary>
        /// <para>Removes files from the directory at the path <paramref name="directory"/> according to the <paramref name="searchPattern"/> and the <paramref name="searchOption"/>.</para>
        /// <para>Удаляет файлы из директории находящейся по пути <paramref name="directory"/> в соотвествии с <paramref name="searchPattern"/> и <paramref name="searchOption"/>.</para>
        /// </summary>
        /// <param name="directory">
        /// <para>The path to the directory to be cleaned.</para>
        /// <para>Путь к директории для очистки.</para>
        /// </param>
        /// <param name="searchPattern">
        /// <para>The search pattern for files to be deleted in the directory at the path <paramref name="directory"/>.</para>
        /// <para>Шаблон поиска для удаляемых файлов в директории находящейся по пути <paramref name="directory"/> .</para>
        /// </param>
        /// <param name="searchOption">
        /// <para>The <see cref="SearchOption"/> value that determines whether to search only in the current the directory at the path <paramref name="directory"/>, or also in all subdirectories.</para>
        /// <para>Значение <see cref="SearchOption"/> определяющее искать ли только в текущей директории находящейся по пути <paramref name="directory"/>, или также во всех субдиректориях.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAll(string directory, string searchPattern, SearchOption searchOption)
        {
            foreach (var file in Directory.EnumerateFiles(directory, searchPattern, searchOption))
            {
                File.Delete(file);
            }
        }

        /// <summary>
        /// <para>Trincates the <paramref name="filename"/>.</para>
        /// <para>Очищает содержимое <paramref name="filename"/>.</para>
        /// </summary>
        /// <param name="filename">
        /// <para>A filename to be truncated.</para>
        /// <para>Путь к файлу для очистки содержимого.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Truncate(string filename) => File.Open(filename, FileMode.Truncate).Dispose();

        /// <summary>
        /// <para>Appends the <paramref name="content"/> to the <paramref name="path"/>.</para>
        /// <para>Добавляет <paramref name="content"/> в конец <paramref name="path"/>.</para>
        /// </summary>
        /// <param name="path">
        /// <para>The path to a file to be appended by the <paramref name="content"/>.</para>
        /// <para>Путь к файлу для добавления содержимого <paramref name="content"/> в конец файла.</para>
        /// </param>
        /// <param name="content">
        /// <para>A content to be appended to the file at the <paramref name="path"/>.</para>
        /// <para>Содержимое для добавления в конец файла по пути <paramref name="path"/>.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void AppendLine(string path, string content)
        {
            using var writer = File.AppendText(path);
            writer.WriteLine(content);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="action">
        /// <para></para>
        /// <para></para>
        /// </param>
        public static void EachLine(string path, Action<string> action)
        {
            using var reader = new StreamReader(path);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                action(line);
            }
        }
    }
}
