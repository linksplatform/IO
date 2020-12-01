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
        /// <para>Displays in the console appealing to press any key and wait user's input.</para>
        /// <para>Выводит в консоль призыв нажать любую клавишу и ожидает ввода от пользователя.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// <para>Get value of argument with the specified index number from the argument array, if it's absent request input of this value in the console from the user.</para>
        /// <para>Получает значение аргумента с указанным порядковым номером из массива аргументов, если он отсутствует запрашивает ввод этого значения в консоли у пользователя.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The index number of the argument in the array.</para>
        /// <para>Порядковый номер аргумента в массиве.</para>
        /// </param>
        /// <param name="readMessage">
        /// <para>The text of the message to the user describing which argument is being entered at the moment. This message is only used if the arguments array doesn't contain argument with the specified number.</para>
        /// <para>Текст сообщения пользователю описывающее какой аргумент вводится в данный момент. Это сообщение используется только если массив аргументов не содержит аргумента с указанным номером.</para>
        /// </param>
        /// <param name="args">
        /// <para>Arguments array received at application start.</para>
        /// <para>Массив аргументов полученных при запуске приложения.</para>
        /// </param>
        /// <returns>
        /// <para>The value of the argument with the specified index number extracted from the argument array or entered by the user in the console.</para>
        /// <para>Значение аргумента с указанным порядковым номером извлечённое из массива аргументов либо введённое пользователем в консоли.</para>
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetOrReadArgument(int index, params string[] args) => GetOrReadArgument(index, $"{index + 1} argument", args);

        /// <summary>
        /// <para>Get value of argument with the specified index number from the argument array, if it's absent request input of this value in the console from the user.</para>
        /// <para>Получает значение аргумента с указанным порядковым номером из массива аргументов, если он отсутствует запрашивает ввод этого значения в консоли у пользователя.</para>
        /// </summary>
        /// <param name="index">
        /// <para>The index number of the argument in the array.</para>
        /// <para>Порядковый номер аргумента в массиве.</para>
        /// </param>
        /// <param name="readMessage">
        /// <para>The text of the message to the user describing which argument is being entered at the moment. This message is only used if the arguments array doesn't contain argument with the specified number.</para>
        /// <para>Текст сообщения пользователю описывающее какой аргумент вводится в данный момент. Это сообщение используется только если массив аргументов не содержит аргумента с указанным номером.</para>
        /// </param>
        /// <param name="args">
        /// <para>Arguments array received at application start.</para>
        /// <para>Массив аргументов полученных при запуске приложения.</para>
        /// </param>
        /// <returns>
        /// <para>The value of the argument with the specified index number extracted from the argument array or entered by the user in the console.</para>
        /// <para>Значение аргумента с указанным порядковым номером извлечённое из массива аргументов либо введённое пользователем в консоли.</para>
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
        /// <para>Output a input string to the console. The method is only executed if the application has been compiled with the DEBUG directive.</para>
        /// <para>Выводит переданную строку в консоль. Метод выполняется только в том случае, если приложение было скомпилировано с директивой DEBUG.</para>
        /// </summary>
        /// <param name="string">
        /// <para>The string, that you want to output to the console.</para>
        /// <para>Строка, которую нужно вывести в консоль.</para>
        /// </param>
        [Conditional("DEBUG")]
        public static void Debug(string @string) => Console.WriteLine(@string);

        /// <summary>
        ///	<para>Output composit formatted text in the console.</para>
        /// <para>Выводит строку составного формата в консоли.</para>
        /// </summary>
        /// <param name="format">
        /// <para>A composite format string.</para>
        /// <para>Строка составного формата.</para>
        /// </param>
        /// <param name="args">
        /// <para>An array of objects to write using format.</para>
        /// <para>Массив объектов для записи с использованием format.</para>
        /// </param>
        [Conditional("DEBUG")]
        public static void Debug(string format, params object[] args) => Console.WriteLine(format, args);
    }
}
