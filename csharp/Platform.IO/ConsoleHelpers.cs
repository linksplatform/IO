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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

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
        public static void Debug(string format, params object[] args) => Console.WriteLine(format, args);
    }
}
