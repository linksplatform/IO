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
            TemporaryFile tempFile = new TemporaryFile();
            Console.WriteLine(tempFile);
        }
    }
}
