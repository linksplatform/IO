using System;
using System.Diagnostics;
using Platform.Collections;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.IO
{
    public static class ConsoleHelpers
    {
        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        public static string GetOrReadArgument(int index, params string[] args) => GetOrReadArgument(index, $"{index + 1} argument", args);

        public static string GetOrReadArgument(int index, string readMessage, params string[] args)
        {
            string result;
            if (args != null && args.Length > index)
            {
                result = args[index];
            }
            else
            {
                Console.Write($"{readMessage}: ");
                result = Console.ReadLine();
            }
            result = (result ?? "").Trim().TrimSingle('"').Trim();
            return result;
        }

        [Conditional("DEBUG")]
        public static void Debug(string format, params object[] args) => Console.WriteLine(format, args);
    }
}
