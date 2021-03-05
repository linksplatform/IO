﻿using System.IO;
using System.Runtime.CompilerServices;
using Platform.Unsafe;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.IO
{
    public static class StreamExtensions
    {
        /// <summary>
        /// <para>Writes a sequence of bytes representing <paramref name = "value"/> to the stream <paramref name = "stream"/>.</para>
        /// <para>Записывает последовательность байт представляющую <paramref name="value"/> в  поток <paramref name="stream"/>.</para>
        /// </summary>
        /// <typeparam name="T">
        /// <para>Struct.</para>
        /// <para>Структура.</para>
        /// </typeparam>
        /// <param name="stream">
        /// <para>An abstract class that provides an overview of a sequence of bytes.</para>
        /// <para>Абстрактный класс предоставляющий универсальное представление последовательности байтов.</para>
        /// </param>
        /// <param name="value">
        /// <para>Struct whose value pass to the stream <paramref name="stream"/>.</para>
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
        /// <para>Reads a sequence of bytes representing.</para>
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
