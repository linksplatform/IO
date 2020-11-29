using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Platform.Collections;
using Platform.Collections.Arrays;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.IO
{
	public static class ConsoleHelpers
	{

		/// <summary>
		/// <para>Displays in the console appealing to press any key for continue</para>
		/// <para>Выводит в консоли призыв нажать любую клавишу для продолжения</para>
		/// </summary>
		/// <returns>
		/// <para>Returns true if a user pressed any key.</para>
		/// <para>Возвращает истину если пользователь нажал любую клавишу</para>
		/// </returns>

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void PressAnyKeyToContinue()
		{
			Console.WriteLine("Press any key to continue.");
			Console.ReadKey();
		}

		/// <summary>
		/// <para>Calls GetOrReadArgument function with readMessage argument's value equal to "index+1 argument"</para>
		/// <para>Вызывает функцию GetOrReadArgument с значением аргумента readMessage равному "index+1 argument"</para>
		/// </summary>
		/// <param name="index">
		/// <para>Item number in the arguments array</para>
		/// <para>Номер элемента в массиве аргументов</para>
		/// </param>
		/// <param name="args">
		/// <para>Array of rest arguments</para>
		/// <para>Массив остаточных аргументов</para>
		/// </param>

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetOrReadArgument(int index, params string[] args) => GetOrReadArgument(index, $"{index + 1} argument", args);

		/// <summary>
		/// <para>Get index and message as arguments, trim whitespace and double quotes around message and return it</para>
		/// <para>Получает аргумент и сообщение как аргумент, вырезает пробелы и двойные кавычки вокруг сообщения и возвращает его</para>
        /// </summary>
		/// <param name="index">
		/// <para>Item number in the array</para>
		/// <para>Номер элемента в массиве</para>
		/// </param>
		/// <param name="readMessage">
		/// <para>Text to output in the console</para>
		/// <para>Текст для вывода в консоль</para>
		/// </param>
		/// <param name="args">
		/// <para>Arguments array with string type</para>
		/// <para>Массив аргументов типа строка</para></param>
		/// <returns>
		/// <para>Returns text passed to console with double quotes and whitespace around removed</para>
		/// <para>Возвращает текст переданный в консоль с удалёнными пробелами и двойными ковычками вокруг текста</para>
		/// </returns>

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetOrReadArgument(int index, string readMessage, params string[] args)
		{
			if (!args.TryGetElement(index, out string result))
			{
				Console.Write($"{readMessage}: ");
				result = Console.ReadLine();
			}
			if (string.IsNullOrEmpty(result))
			{
				return "";
			}
			else
			{
				return result.Trim().TrimSingle('"').Trim();
			}
		}

		/// <summary>
		/// <para>Output a input string to the console</para>
		/// <para>Выводит переданныую строку</para>
		/// </summary>
		/// <param name="string">
        /// <para>Text passed to the console</para>
        /// <para>Текст переданный в консоль</para>
        /// </param>

		[Conditional("DEBUG")]
		public static void Debug(string @string) => Console.WriteLine(@string);

		/// <summary>
		///	<para>Output composit formatted text in the console</para>
		/// <para>Выводит композитный форматированный текст в консоли</para>
		/// </summary>
		/// <param name="format">
        /// <para>A composite format string</para>
        /// <para>Строка составного формата</para>
        /// </param>
		/// <param name="args">
        /// <para>Array of rest arguments</para>
        /// <para>Массив остаточных аргументов</para>
        /// </param>

		[Conditional("DEBUG")]
		public static void Debug(string format, params object[] args) => Console.WriteLine(format, args);
	}
}
