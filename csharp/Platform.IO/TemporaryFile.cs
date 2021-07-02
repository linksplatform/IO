using Platform.Disposables;
using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Platform.IO
{
    /// <summary>
    /// <para>Represents a self-deleting temporary file.</para>
    /// <para>Представляет самоудаляющийся временный файл.</para>
    /// </summary>
    public class TemporaryFile : DisposableBase
    {
        /// <summary>
        /// <para>Gets a temporary file path.</para>
        /// <para>Возвращает путь к временному файлу.</para>
        /// </summary>
        public readonly string Filename;

        /// <summary>
        /// <para>Converts the <see cref="TemporaryFile"/> instance to <see cref="string"/> using the <see cref="Filename"/> field value.</para>
        /// <para>Преобразует экземпляр <see cref="TemporaryFile"/> в <see cref="string"/> используя поле <see cref="Filename"/>.</para>
        /// </summary>
        /// <param name="file"></param>
        public static implicit operator string(TemporaryFile file) => file.Filename;

        /// <summary>
        /// <para>Initializes a <see cref="TemporaryFile"/> instance.</para>
        /// <para>Инициализирует экземпляр класса <see cref="TemporaryFile"/>.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TemporaryFile() => Filename = TemporaryFiles.UseNew();

        /// <summary>
        /// <para>Deletes the temporary file.</para>
        /// <para>Удаляет временный файл.</para>
        /// </summary>
        /// <param name="manual">
        /// <para>A <see cref="Boolean"/> value that determines whether the disposal was triggered manually (by the developer's code) or was executed automatically without an explicit indication from a developer.</para>
        /// <para>Значение типа <see cref="Boolean"/>, определяющие было ли высвобождение вызвано вручную (кодом разработчика) или же выполнилось автоматически без явного указания со стороны разработчика.</para>
        /// </param>
        /// <param name="wasDisposed">
        /// <para>A <see cref="Boolean"/> value that determines whether the <see cref="TemporaryFile"/> was released before a call to this method.</para>
        /// <para>Значение типа <see cref="Boolean"/>, определяющие были ли освобождены ресурсы, используемые <see cref="TemporaryFile"/> до вызова данного метода.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void Dispose(bool manual, bool wasDisposed)
        {
            if (!wasDisposed)
            {
                File.Delete(Filename);
            }
        }
    }
}
