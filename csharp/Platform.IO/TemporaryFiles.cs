using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Platform.IO
{
    /// <summary>
    /// <para>Represents the set of helper methods to work with temporary files.</para>
    /// <para>Представляет набор вспомогательных методов для работы с временными файлами.</para>
    /// </summary>
    class TemporaryFiles
    {
        private const string UserFilesListFileNamePrefix = ".used-temporary-files.txt";
        static private readonly object UsedFilesListLock = new object();

        private static string GetUsedFilesListFilename() => Assembly.GetEntryAssembly().Location + UserFilesListFileNamePrefix;

        private static void AddToUsedFilesList(string filename)
        {
            lock (UsedFilesListLock)
            {
                using var writer = File.AppendText(GetUsedFilesListFilename());
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
        public static void DeleteAllPreviouslyUsed()
        {
            lock (UsedFilesListLock)
            {
                var usedFilesListFilename = GetUsedFilesListFilename();

                if (!File.Exists(usedFilesListFilename)) return;

                using (var listFile = File.Open(usedFilesListFilename, FileMode.Open))
                using (var reader = new StreamReader(listFile))
                {
                    string tempFileToDelete;
                    while ((tempFileToDelete = reader.ReadLine()) != null)
                    {
                        if (File.Exists(tempFileToDelete)) File.Delete(tempFileToDelete);
                    }
                }

                using (File.Open(usedFilesListFilename, FileMode.Truncate)) { }
            }
        }
    }
}
