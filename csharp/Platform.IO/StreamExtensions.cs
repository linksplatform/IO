using System.IO;
using System.Runtime.CompilerServices;
using Platform.Unsafe;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.IO
{
    public static class StreamExtensions
    {
        /// <summary>
        /// <para>Writes a sequence of bytes derived from <paramref name="value"/> to the current stream and advances the current position within this stream by the number of bytes written.</para>
        /// <para>Записывает последовательность байтов, полученных из <paramref name="value"/> в текущий поток и перемещает текущую позицию в нем вперед на число записанных байтов.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>Struct.</para>
        /// <para>Структура.</para>
        /// </typeparam>
        /// <param name="stream">
        /// <para>An abstract class that provides an overview of a sequence of bytes.</para>
        /// <para>Абстрактный класс предоставляющий универсальное представление последовательности байтов.</para>
        /// </param>
        /// <param name="value"></param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this Stream stream, T value)
            where T : struct
        {
            var bytes = value.ToBytes();
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// <para>Reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.</para>
        /// <para></para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>Struct.</para>
        /// <para>Структура.</para>
        /// </typeparam>
        /// <param name="stream">
        /// <para>An abstract class that provides an overview of a sequence of bytes.</para>
        /// <para>Абстрактный класс предоставляющий универсальное представление последовательности байтов.</para>
        /// </param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadOrDefault<T>(this Stream stream)
            where T : struct
        {
            var size = Structure<T>.Size;
            var buffer = new byte[size];
            return stream.Read(buffer, 0, size) == size ? buffer.ToStructure<T>() : default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ReadAll<T>(this Stream stream)
            where T : struct
        {
            var size = Structure<T>.Size;
            var buffer = new byte[size];
            var elementsLength = stream.Length / size;
            var elements = new T[elementsLength];
            for (var i = 0; i < elementsLength; i++)
            {
                stream.Read(buffer, 0, size);
                elements[i] = buffer.ToStructure<T>();
            }
            return elements;
        }
    }
}
