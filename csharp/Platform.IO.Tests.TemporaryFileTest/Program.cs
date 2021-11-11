using System;
using Platform.IO;
using System.IO;
using System.Reflection;

namespace Platform.IO.Tests.TemporaryFileTest
{
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
