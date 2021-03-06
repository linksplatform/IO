using System.IO;
using System.Runtime.CompilerServices;
using Platform.Unsafe;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.IO
{
    public static class StreamExtensions
    {
        /// <summary>
        /// <para>Writes a sequence that represents the <paramref name="value"/> to the <paramref name="stream"/> and moves the current position of the <paramref name="stream"/> by the number of written bytes.</para>
        /// <para>Записывает последовательность байт представляющую <paramref name="value"/> в поток <paramref name="stream"/> и перемещает текущую позицию в нем вперед на число записанных байт.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип являющийся структурой.</para>
        /// </typeparam>
        /// <param name="stream">
        /// <para>A stream to write to.</para>
        /// <para>Поток, в который осуществляется запись.</para>
        /// </param>
        /// <param name="value">
        /// <para>The struct <typeparam name="T"> value to be written to the <paramref name="stream"/>.</para>
        /// <para>Значение структуры <typeparam name="T"> которое будет записано в поток <paramref name="stream"/>.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this Stream stream, T value)
            where T : struct
        {
            var bytes = value.ToBytes();
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// <para>Reads a sequence that represents the <typeparamref name="T"/> structure and moves the current position of the <paramref name="stream"/> by the number of read bytes.</para>
        /// <para>Считывает последовательность байт представляющих структуру <typeparamref name="T"/> и перемещает текущую позицию в <paramref name="stream"/> вперед на число прочитанных байт.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип являющийся структурой.</para>
        /// </typeparam>
        /// <param name="stream">
        /// <para>A stream containing the <typeparam name="T"> structure.</para>
        /// <para>Поток, содержащий структуру <typeparam name="T">.</para>
        /// </param>
        /// <returns>
        /// <para>The <typeparam name="T"> structure, if all bytes from <paramref name="stream"/> have been read, otherwise the default value of <typeparamref name="T"/> structure.</para>
        /// <para>Структура <typeparam name="T">, если её байты из потока были прочитаны, иначе значение структуры <typeparamref name="T"/> по умолчанию.</para>
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
        /// <para>Reads and return array of <typeparam name="T"> structures from the <paramref name="stream"/>.</para>
        /// <para>Прочитывает и возвращает массив всех структур <typeparam name="T"> из потока <paramref name="stream"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип являющийся структурой.</para>
        /// </typeparam>
        /// <param name="stream">
        /// <para>A stream containing the <typeparam name="T"> structure.</para>
        /// <para>Поток, содержащий структуру <typeparam name="T">.</para>
        /// </param>
        /// <returns>
        /// <para>The array with <typeparam name="T"> structures obtained from the <paramref name="stream"/>.</para>
        /// <para>Массив с структурами <typeparam name="T"> полученными из потока <paramref name="stream"/>.</para>
        /// </returns>
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
