using System;
using Platform.IO;
using System.IO;
using System.Reflection;

namespace Platform.IO.Tests.TemporaryFileTest
{
    /// <summary>
    /// <para>
    /// Represents the program.
    /// </para>
    /// <para></para>
    /// </summary>
    class Program
    {
        /// <summary>
        /// <para>
        /// Main.
        /// </para>
        /// <para></para>
        /// </summary>
        static void Main()
        {
            using TemporaryFile tempFile = new();
            Console.WriteLine(tempFile);
        }
    }
}
