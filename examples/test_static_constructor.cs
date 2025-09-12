using System;
using System.IO;
using Platform.IO;

// Create a simple test to verify static constructor is called
class Program 
{
    static void Main()
    {
        Console.WriteLine("Creating first temporary file...");
        var temp1 = TemporaryFiles.UseNew();
        Console.WriteLine($"First temp file: {temp1}");
        Console.WriteLine($"First temp file exists: {File.Exists(temp1)}");
        
        Console.WriteLine("\nCreating second temporary file...");
        var temp2 = TemporaryFiles.UseNew();
        Console.WriteLine($"Second temp file: {temp2}");
        Console.WriteLine($"Second temp file exists: {File.Exists(temp2)}");
        
        Console.WriteLine("\nCalling DeleteAllPreviouslyUsed manually...");
        TemporaryFiles.DeleteAllPreviouslyUsed();
        
        Console.WriteLine($"First temp file exists after cleanup: {File.Exists(temp1)}");
        Console.WriteLine($"Second temp file exists after cleanup: {File.Exists(temp2)}");
        
        Console.WriteLine("\nTest completed successfully!");
    }
}