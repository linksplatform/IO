#include <iostream>
#include "../Platform.IO/TemporaryFile.h"

using namespace Platform::IO;

int main()
{
    {
        TemporaryFile tempFile;
        std::cout << tempFile.Filename << std::endl;
    }
    // TemporaryFile destructor automatically deletes the file
    return 0;
}
