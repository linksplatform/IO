using System.IO;
using System.Runtime.CompilerServices;
using Platform.Unsafe;


namespace Platform.IO
{
    /// <summary>
    /// <para>Provides a set of extension methods for <see cref="Stream"/> class instances.</para>
    /// <para>Предоставляет набор методов расширения для эксземпляров класса <see cref="Stream"/>.</para>
    /// </summary>
    public static class StreamExtensions
    {
        /// <summary>
        /// <para>Writes a byte sequence that represents the <typeparamref name="T"/> <paramref name="value"/> to the <paramref name="stream"/> and moves the current position of the <paramref name="stream"/> by the number of written bytes.</para>
        /// <para>Записывает последовательность байт представляющую <paramref name="value"/> типа <typeparamref name="T"/> в поток <paramref name="stream"/> и перемещает текущую позицию в <paramref name="stream"/> вперёд на число записанных байт.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип структуры.</para>
        /// </typeparam>
        /// <param name="stream">
        /// <para>A stream to write to.</para>
        /// <para>Поток, в который осуществляется запись.</para>
        /// </param>
        /// <param name="value">
        /// <para>The <typeparam name="T"> structure value to be written to the <paramref name="stream"/>.</para>
        /// <para>Значение структуры типа <typeparam name="T"> которое будет записано в поток <paramref name="stream"/>.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this Stream stream, T value)
            where T : struct
        {
            var bytes = value.ToBytes();
            stream.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// <para>Reads a byte sequence that represents the <typeparamref name="T"/> structure value and moves the current position of the <paramref name="stream"/> by the number of read bytes.</para>
        /// <para>Считывает последовательность байт представляющих значение структуры типа <typeparamref name="T"/> и перемещает текущую позицию в потоке <paramref name="stream"/> вперёд на число прочитанных байт.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип структуры.</para>
        /// </typeparam>
        /// <param name="stream">
        /// <para>A stream containing the <typeparam name="T"> structure value.</para>
        /// <para>Поток, содержащий значение структуры типа <typeparam name="T">.</para>
        /// </param>
        /// <returns>
        /// <para>The <typeparam name="T"> structure value, if its bytes from the <paramref name="stream"/> are read; otherwise the default <typeparamref name="T"/> structure value.</para>
        /// <para>Значение структуры типа <typeparam name="T">, если её байты из потока <paramref name="stream"/> были прочитаны, иначе значение структуры типа <typeparamref name="T"/> по умолчанию.</para>
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
        /// <para>Reads and returns the  <typeparam name="T"> structure values array from the <paramref name="stream"/>.</para>
        /// <para>Прочитывает и возвращает массив всех значений структур типа <typeparam name="T"> из потока <paramref name="stream"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>The structure type.</para>
        /// <para>Тип структуры.</para>
        /// </typeparam>
        /// <param name="stream">
        /// <para>A stream containing the <typeparam name="T"> structure values.</para>
        /// <para>Поток, содержащий значения структур типа <typeparam name="T">.</para>
        /// </param>
        /// <returns>
        /// <para>The <typeparam name="T"> structure values array read from the <paramref name="stream"/>.</para>
        /// <para>Массив с значениями структур типа <typeparam name="T">, прочитанными из потока <paramref name="stream"/>.</para>
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
