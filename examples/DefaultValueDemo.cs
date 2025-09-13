using System;
using Platform.IO;

namespace Platform.IO.Examples
{
    class DefaultValueDemo
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== ConsoleHelpers Default Value Demo ===");
            
            // Example 1: GetOrReadArgument without default value (existing functionality)
            Console.WriteLine("\n1. GetOrReadArgument(0, args) with args = ['hello', 'world']:");
            var testArgs = new[] { "hello", "world" };
            var result1 = ConsoleHelpers.GetOrReadArgument(0, testArgs);
            Console.WriteLine($"   Result: '{result1}'");

            // Example 2: GetOrReadArgument without default value for non-existent argument
            Console.WriteLine("\n2. GetOrReadArgument(3, args) - non-existent index:");
            Console.WriteLine("   (This would normally prompt for console input, skipped in demo)");

            // Example 3: GetOrReadArgumentOrDefault with existing argument
            Console.WriteLine("\n3. GetOrReadArgumentOrDefault(1, 'defaultValue', args):");
            var result3 = ConsoleHelpers.GetOrReadArgumentOrDefault(1, "defaultValue", testArgs);
            Console.WriteLine($"   Result: '{result3}' (should be 'world')");

            // Example 4: GetOrReadArgumentWithDefault with custom message and default
            Console.WriteLine("\n4. GetOrReadArgumentWithDefault with custom message:");
            Console.WriteLine("   (This would normally prompt for console input with custom message, skipped in demo)");

            Console.WriteLine("\n=== Demo completed successfully ===");
        }
    }
}