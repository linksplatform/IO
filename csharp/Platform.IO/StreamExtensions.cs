using System.IO;
using System.Runtime.CompilerServices;
using Platform.Unsafe;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.IO
{
    public static class StreamExtensions
    {
        /// <summary>
        /// <para>Writes a sequence of bytes representing <paramref name="value"/> to the stream <paramref name="stream"/> and moves the current position within the <paramref name="stream"/> by the number of written bytes.</para>
        /// <para>Записывает последовательность байт представляющую <paramref name="value"/> в  поток <paramref name="stream"/> и перемещает текущую позицию в нем вперед на число записанных байт.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>Type being structure.</para>
        /// <para>Тип являющийся структурой.</para>
        /// </typeparam>
        /// <param name="stream">
        /// <para>A stream to record.</para>
        /// <para>Поток, в который осуществляется запись.</para>
        /// </param>
        /// <param name="value">
        /// <para>The struct value to be written to the <paramref name="stream"/>..</para>
        /// <para>Структура, значение которой записывается в поток <paramref name="stream"/>.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this Stream stream, T value)
            where T : struct
        {
            var bytes = value.ToBytes();
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// <para>Reads a sequence of bytes representing the structure and moves the current position within the <paramref name="stream"/> by the number of bytes read.</para>
        /// <para>Прочитывает последовательность байт представляющих структуру и перемещает текущую позицию в <paramref name="stream"/> вперед на число прочитанных байт..</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>Type being structure.</para>
        /// <para>Тип являющийся структурой.</para>
        /// </typeparam>
        /// <param name="stream">
        /// <para>A stream containing the structure.</para>
        /// <para>Поток, содержащий структуру.</para>
        /// </param>
        /// <returns>
        /// <para>The structure, if all bytes from stream have been read, otherwise the structure with default fields values.</para>
        /// <para>Структура, если все байты из потока были прочитаны, иначе структура с значениями полей по умолчанию.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ReadOrDefault<T>(this Stream stream)
            where T : struct
        {
            var size = Structure<T>.Size;
            var buffer = new byte[size];
            return stream.Read(buffer, 0, size) == size ? buffer.ToStructure<T>() : default;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream"></param>
        /// <returns></returns>
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
