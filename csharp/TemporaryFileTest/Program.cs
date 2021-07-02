using System;
using Platform.IO;
using System.IO;
using System.Reflection;

namespace TemporaryFileTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using TemporaryFile tempFile = new();
            Console.WriteLine(tempFile);
        }
    }
}
