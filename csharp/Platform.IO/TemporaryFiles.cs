using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Platform.IO
{
    /// <summary>
    /// <para>Represents the set of helper methods to work with temporary files.</para>
    /// <para>Представляет набор вспомогательных методов для работы с временными файлами.</para>
    /// </summary>
    public class TemporaryFiles
    {
        private const string UserFilesListFileNamePrefix = ".used-temporary-files.txt";
        private static readonly object UsedFilesListLock = new();
        private static readonly string UsedFilesListFilename = Assembly.GetExecutingAssembly().Location + UserFilesListFileNamePrefix;

        /// <summary>
        /// <para>
        /// Adds the to used files list using the specified filename.
        /// </para>
        /// <para></para>
        /// </summary>
        /// <param name="filename">
        /// <para>The filename.</para>
        /// <para></para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AddToUsedFilesList(string filename)
        {
            lock (UsedFilesListLock)
            {
                FileHelpers.AppendLine(UsedFilesListFilename, filename);
            }
        }

        /// <summary>
        /// <para>Gets a temporary file and adds it to the used files list.</para>
        /// <para>Получает временный файл и добавляет его в список использованных файлов.</para>
        /// </summary>
        /// <returns>
        /// <para>The temporary file path.</para>
        /// <para>Путь временного файла.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string UseNew()
        {
            var filename = Path.GetTempFileName();
            AddToUsedFilesList(filename);
            return filename;
        }

        /// <summary>
        /// <para>Deletes all previously used temporary files and clears the files list.</para>
        /// <para>Удаляет все ранее использованные временные файлы и очищает список файлов.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void DeleteAllPreviouslyUsed()
        {
            lock (UsedFilesListLock)
            {
                var listFilename = UsedFilesListFilename;
                if (File.Exists(listFilename))
                {
                    FileHelpers.EachLine(listFilename, File.Delete);
                    FileHelpers.Truncate(listFilename);
                }
            }
        }
    }
}
