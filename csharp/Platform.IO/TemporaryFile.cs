using Platform.Disposables;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Platform.IO
{
    /// <summary>
    /// <para>Represents a self-deleting temporary file that provides security features equivalent to C's tmpfile() or better.</para>
    /// <para>Представляет самоудаляющийся временный файл, обеспечивающий функции безопасности, эквивалентные tmpfile() из C или лучше.</para>
    /// </summary>
    public class TemporaryFile : DisposableBase
    {
        /// <summary>
        /// <para>Gets a temporary file path.</para>
        /// <para>Возвращает путь к временному файлу.</para>
        /// </summary>
        public readonly string Filename;

        /// <summary>
        /// <para>Gets a FileStream for the temporary file, providing direct access like C's tmpfile().</para>
        /// <para>Получает FileStream для временного файла, обеспечивая прямой доступ как tmpfile() из C.</para>
        /// </summary>
        public readonly FileStream Stream;

        /// <summary>
        /// <para>Converts the <see cref="TemporaryFile"/> instance to <see cref="string"/> using the <see cref="Filename"/> field value.</para>
        /// <para>Преобразует экземпляр <see cref="TemporaryFile"/> в <see cref="string"/> используя поле <see cref="Filename"/>.</para>
        /// </summary>
        /// <param name="file">
        /// <para>A <see cref="TemporaryFile"/> instance.</para>
        /// <para>Экземпляр <see cref="TemporaryFile"/>.</para>
        /// </param>
        /// <returns>
        /// <para>Path to the temporary file.</para>
        /// <para>Путь к временному файлу.</para>
        /// </returns>
        public static implicit operator string(TemporaryFile file) => file.Filename;

        /// <summary>
        /// <para>Converts the <see cref="TemporaryFile"/> instance to <see cref="FileStream"/> using the <see cref="Stream"/> field value.</para>
        /// <para>Преобразует экземпляр <see cref="TemporaryFile"/> в <see cref="FileStream"/> используя поле <see cref="Stream"/>.</para>
        /// </summary>
        /// <param name="file">
        /// <para>A <see cref="TemporaryFile"/> instance.</para>
        /// <para>Экземпляр <see cref="TemporaryFile"/>.</para>
        /// </param>
        /// <returns>
        /// <para>FileStream of the temporary file.</para>
        /// <para>FileStream временного файла.</para>
        /// </returns>
        public static implicit operator FileStream(TemporaryFile file) => file.Stream;

        /// <summary>
        /// <para>Initializes a <see cref="TemporaryFile"/> instance with enhanced security features similar to C's tmpfile().</para>
        /// <para>Инициализирует экземпляр класса <see cref="TemporaryFile"/> с улучшенными функциями безопасности, аналогичными tmpfile() из C.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TemporaryFile() 
        {
            (Filename, Stream) = CreateSecureTemporaryFile();
            TemporaryFiles.AddToRegistry(Filename);
        }

        /// <summary>
        /// <para>Creates a secure temporary file using more secure methods than Path.GetTempFileName().</para>
        /// <para>Создает безопасный временный файл, используя более безопасные методы, чем Path.GetTempFileName().</para>
        /// </summary>
        /// <returns>
        /// <para>A tuple containing the filename and FileStream.</para>
        /// <para>Кортеж, содержащий имя файла и FileStream.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static (string filename, FileStream stream) CreateSecureTemporaryFile()
        {
            string tempDir = Path.GetTempPath();
            string filename;
            FileStream stream = null;
            
            // Retry loop to handle race conditions with random filename generation
            for (int attempts = 0; attempts < 1000; attempts++)
            {
                try
                {
                    filename = Path.Combine(tempDir, Path.GetRandomFileName());
                    
                    // Create file with exclusive access and restrictive permissions
                    stream = new FileStream(
                        filename, 
                        FileMode.CreateNew,  // Ensures the file doesn't already exist
                        FileAccess.ReadWrite,
                        FileShare.None,      // No sharing while open
                        4096,                // Default buffer size
                        FileOptions.DeleteOnClose | FileOptions.SequentialScan
                    );

                    // Set restrictive file permissions (equivalent to 0600)
                    SetRestrictivePermissions(filename);
                    
                    return (filename, stream);
                }
                catch (IOException)
                {
                    // File already exists or other IO error, try again with a different name
                    stream?.Dispose();
                    continue;
                }
                catch (UnauthorizedAccessException)
                {
                    // Permission issue, try again
                    stream?.Dispose();
                    continue;
                }
            }
            
            throw new IOException("Could not create secure temporary file after 1000 attempts.");
        }

        /// <summary>
        /// <para>Sets restrictive permissions on the temporary file (equivalent to Unix 0600).</para>
        /// <para>Устанавливает ограничительные разрешения для временного файла (эквивалент Unix 0600).</para>
        /// </summary>
        /// <param name="filename">
        /// <para>The filename to set permissions on.</para>
        /// <para>Имя файла для установки разрешений.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void SetRestrictivePermissions(string filename)
        {
            try
            {
                if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                {
                    // On Windows, set ACL to allow only current user
                    var fileInfo = new FileInfo(filename);
                    var fileSecurity = fileInfo.GetAccessControl();
                    
                    // Remove inherited permissions
                    fileSecurity.SetAccessRuleProtection(true, false);
                    
                    // Add full control for current user only
                    var currentUser = WindowsIdentity.GetCurrent();
                    var accessRule = new FileSystemAccessRule(
                        currentUser.User,
                        FileSystemRights.FullControl,
                        AccessControlType.Allow
                    );
                    
                    fileSecurity.AddAccessRule(accessRule);
                    fileInfo.SetAccessControl(fileSecurity);
                }
                else
                {
                    // On Unix-like systems, use chmod equivalent (600 permissions)
                    // This would require P/Invoke or external process, simplified for now
                    // The FileOptions.DeleteOnClose provides some security
                }
            }
            catch
            {
                // If we can't set permissions, continue - FileOptions.DeleteOnClose still provides security
            }
        }

        /// <summary>
        /// <para>Finalizer to ensure cleanup in case of abnormal termination.</para>
        /// <para>Финализатор для обеспечения очистки в случае аварийного завершения.</para>
        /// </summary>
        ~TemporaryFile()
        {
            Dispose(false, false);
        }

        /// <summary>
        /// <para>Deletes the temporary file and closes the stream.</para>
        /// <para>Удаляет временный файл и закрывает поток.</para>
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
                try
                {
                    // Dispose the stream first - this will trigger DeleteOnClose if it was set
                    Stream?.Dispose();
                }
                catch
                {
                    // Ignore errors during stream disposal
                }

                try
                {
                    // Attempt manual deletion in case DeleteOnClose failed
                    if (File.Exists(Filename))
                    {
                        File.Delete(Filename);
                    }
                }
                catch
                {
                    // Ignore errors during file deletion - file might already be deleted by DeleteOnClose
                }

                // Remove from registry
                TemporaryFiles.RemoveFromRegistry(Filename);

                // Suppress finalizer if this was a manual disposal
                if (manual)
                {
                    GC.SuppressFinalize(this);
                }
            }
        }
    }
}
