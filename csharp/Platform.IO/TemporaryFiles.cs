using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Platform.IO
{
    class TemporaryFiles
    {
        private const string UserFilesListFileNamePrefix = ".used-temporary-files.txt";
        static private readonly object UsedFilesListLock = new object();

        private static string GetUsedFilesListFilename()
        {
            return Assembly.GetEntryAssembly().Location + UserFilesListFileNamePrefix;
        }

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
