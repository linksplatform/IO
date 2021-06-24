using Platform.Disposables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platform.IO
{
    public class TemporaryFile : DisposableBase
    {
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
