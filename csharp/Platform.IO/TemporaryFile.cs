using Platform.Disposables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.IO
{
    /// <summary>
    /// <para>Represents a self-deleting temporary file.</para>
    /// <para>Представляет самоудаляющийся временный файл.</para>
    /// </summary>
    public class TemporaryFile : DisposableBase
    {
        /// <summary>
        /// <para>Gets a temp file path.</para>
        /// <para>Возвращает путь к временному файлу.</para>
        /// </summary>
        public readonly string Filename;

        public TemporaryFile()
        {
            Filename = TemporaryFiles.UseNew();
        }

        protected override void Dispose(bool manual, bool wasDisposed)
        {
            if (!wasDisposed)
            {
                File.Delete(Filename);
            }
        }
    }
}
