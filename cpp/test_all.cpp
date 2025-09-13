#include "Platform.IO/Platform.IO.h"
#include <iostream>

using namespace Platform::IO;

int main() {
    // Test ConsoleHelpers Debug
    ConsoleHelpers::Debug("Testing debug output");
    
    // Test TemporaryFile
    {
        TemporaryFile tempFile;
        std::cout << "Created temporary file: " << tempFile.Filename << std::endl;
        
        // Test FileHelpers
        const int testValue = 42;
        FileHelpers::WriteFirst(tempFile.Filename, testValue);
        auto readValue = FileHelpers::ReadFirstOrDefault<int>(tempFile.Filename);
        std::cout << "Written value: " << testValue << ", Read value: " << readValue << std::endl;
        
        // Test file size
        auto size = FileHelpers::GetSize(tempFile.Filename);
        std::cout << "File size: " << size << " bytes" << std::endl;
    }
    
    // Test ConsoleCancellation
    ConsoleCancellation cancellation;
    std::cout << "Cancellation requested: " << cancellation.IsRequested() << std::endl;
    
    std::cout << "All tests completed successfully!" << std::endl;
    return 0;
}