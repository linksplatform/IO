﻿using Platform.Disposables;
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

        /// <summary>
        /// <para>Gets a <see cref="TemporaryFile"/> instance.</para>
        /// <para>Возвращает экземпляр класса <see cref="TemporaryFile"/>.</para>
        /// </summary>
        public TemporaryFile()
        {
            Filename = TemporaryFiles.UseNew();
        }

        /// <summary>
        /// <para>Deletes the temp file.</para>
        /// <para>Удаляет временный файл.</para>
        /// </summary>
        /// <param name="manual">
        /// <para>A <see cref="Boolean"/> value that determines whether the disposal was triggered manually (by the developer's code) or was executed automatically without an explicit indication from a developer.</para>
        /// <para>Значение типа <see cref="Boolean"/>, определяющие было ли высвобождение вызвано вручную (кодом разработчика) или же выполнилось автоматически без явного указания со стороны разработчика.</para>
        /// </param>
        /// <param name="wasDisposed">
        /// <para>A <see cref="Boolean"/> value that determines whether the <see cref="ConsoleCancellation"/> was released before a call to this method.</para>
        /// <para>Значение типа <see cref="Boolean"/>, определяющие были ли освобождены ресурсы, используемые <see cref="ConsoleCancellation"/> до вызова данного метода.</para>
        /// </param>
        protected override void Dispose(bool manual, bool wasDisposed)
        {
            if (!wasDisposed)
            {
                File.Delete(Filename);
            }
        }
    }
}
