using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Platform.IO
{
    /// <summary>
    /// <para>Represents the set of helper methods to work with temporary files.</para>
    /// <para>Представляет набор вспомогательных методов для работы с временными файлами.</para>
    /// </summary>
    public class TemporaryFiles
    {
        private const string UserFilesListFileNamePrefix = ".used-temporary-files.txt";
        private static readonly object UsedFilesListLock = new object();
        private static readonly string UsedFilesListFilename = Assembly.GetExecutingAssembly().Location + UserFilesListFileNamePrefix;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void AddToUsedFilesList(string filename)
        {
            lock (UsedFilesListLock)
            {
                using var writer = File.AppendText(UsedFilesListFilename);
                writer.WriteLine(filename);
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
                if (!File.Exists(listFilename))
                {
                    return;
                }
                using (var listFile = File.Open(listFilename, FileMode.Open))
                using (var reader = new StreamReader(listFile))
                {
                    string temporaryFile;
                    while ((temporaryFile = reader.ReadLine()) != null)
                    {
                        File.Delete(temporaryFile);
                    }
                }
                FileHelpers.Truncate(listFilename);
            }
        }
    }
}
