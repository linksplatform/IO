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
    /// <para>Represents the set of helper methods to work with temp files.</para>
    /// <para>Представляет набор вспомогательных методов для работы с временными файлами.</para>
    /// </summary>
    class TemporaryFiles
    {
        private const string UserFilesListFileNamePrefix = ".used-temporary-files.txt";
        static private readonly object UsedFilesListLock = new object();

        /// <summary>
        /// <para>Returns used files list file location.</para>
        /// <para>Возвращает местоположение файла содержащего список использованных файлов.</para>
        /// </summary>
        /// <returns>
        /// <para>Used files list file location.</para>
        /// <para>Местоположение файла содержащего список использованных файлов.</para>
        /// </returns>
        private static string GetUsedFilesListFilename()
        {
            return Assembly.GetEntryAssembly().Location + UserFilesListFileNamePrefix;
        }

        /// <summary>
        /// <para>Adds a filename to the used files list.</para>
        /// <para>Добавляет путь к файлу в список использованных файлов.</para>
        /// </summary>
        /// <param name="filename">
        /// <para>The filename to be added to the used files list.</para>
        /// <para>Путь к файлу для добавления в список использованных файлов.</para>
        /// </param>
        private static void AddToUsedFilesList(string filename)
        {
            lock (UsedFilesListLock)
            {
                using var writer = File.AppendText(GetUsedFilesListFilename());
                writer.WriteLine(filename);
            }
        }

        public static string UseNew()
        {
            var filename = Path.GetTempFileName();
            AddToUsedFilesList(filename);
            return filename;
        }

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
