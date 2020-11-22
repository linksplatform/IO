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
		/// <para>Displays in console appealing to press any key for continue. Checks if a user pressed any key.</para>
		/// <para>Выводит в консоли призыв нажать любую клавишу для продолжения. Проверяет нажал ли пользователь любую кнопку.</para>
		/// </summary>
		/// <returns>
		/// <para>Returns true if a user pressed any key.</para>
		/// <para>Возвращает истину если пользователь нажал любую клавишу.</para>
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
		/// <param name="index">Index integer</param>
		/// <param name="args"></param>
		/// <returns></returns>

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static string GetOrReadArgument(int index, params string[] args) => GetOrReadArgument(index, $"{index + 1} argument", args);

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

		[Conditional("DEBUG")]
		public static void Debug(string @string) => Console.WriteLine(@string);

		[Conditional("DEBUG")]
		public static void Debug(string format, params object[] args) => Console.WriteLine(format, args);
	}
}
