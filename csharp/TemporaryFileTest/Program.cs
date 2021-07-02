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
            Console.WriteLine(tempFile.Filename);
            Console.WriteLine(Assembly.GetEntryAssembly().Location);
            Console.WriteLine(Directory.GetCurrentDirectory());
            Console.WriteLine(Environment.CurrentDirectory);
            Console.WriteLine(Assembly.GetExecutingAssembly().Location);
        }
    }
}
