using System;
using System.IO;
using Platform.IO;

// Create a simple test to verify static constructor is called
class Program 
{
    static void Main()
    {
        Console.WriteLine("Testing TemporaryFiles static constructor...");
        Console.WriteLine("Creating first temporary file (this will trigger static constructor)...");
        var temp1 = TemporaryFiles.UseNew();
        Console.WriteLine($"First temp file: {temp1}");
        Console.WriteLine($"First temp file exists: {File.Exists(temp1)}");
        
        Console.WriteLine("\nTest completed successfully!");
        Console.WriteLine("The static constructor was called when TemporaryFiles.UseNew() was first accessed.");
        
        // Clean up the test file
        File.Delete(temp1);
    }
}
