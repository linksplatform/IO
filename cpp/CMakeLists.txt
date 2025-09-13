cmake_minimum_required(VERSION 3.15)

# Set the project name and version
project(Platform.IO VERSION 1.0.0 LANGUAGES CXX)

# Set C++ standard
set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)
set(CMAKE_CXX_EXTENSIONS OFF)

# Platform.IO library
add_library(Platform.IO INTERFACE)

# Header files
target_include_directories(Platform.IO INTERFACE
    $<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}>
    $<INSTALL_INTERFACE:include>
)

# Target compile features
target_compile_features(Platform.IO INTERFACE cxx_std_20)

# Find required system libraries
find_package(Threads REQUIRED)
target_link_libraries(Platform.IO INTERFACE Threads::Threads)

# Enable testing
option(BUILD_TESTS "Build tests" ON)
if(BUILD_TESTS)
    enable_testing()
    
    # Find Google Test
    find_package(GTest QUIET)
    if(NOT GTest_FOUND)
        # Download and build Google Test
        include(FetchContent)
        FetchContent_Declare(
            googletest
            URL https://github.com/google/googletest/archive/release-1.12.1.zip
        )
        # For Windows: Prevent overriding the parent project's compiler/linker settings
        set(gtest_force_shared_crt ON CACHE BOOL "" FORCE)
        FetchContent_MakeAvailable(googletest)
    endif()
    
    # Test executable
    add_executable(Platform.IO.Tests
        Platform.IO.Tests/TemporaryFileTests.cpp
        Platform.IO.Tests/FileHelpersTests.cpp
    )
    
    target_link_libraries(Platform.IO.Tests
        Platform.IO
        gtest_main
        gtest
        pthread
    )
    
    target_include_directories(Platform.IO.Tests PRIVATE
        ${CMAKE_CURRENT_SOURCE_DIR}
    )
    
    # Register tests
    include(GoogleTest)
    gtest_discover_tests(Platform.IO.Tests)
endif()

# Installation
install(DIRECTORY Platform.IO/
    DESTINATION include/Platform.IO
    FILES_MATCHING PATTERN "*.h"
)

install(TARGETS Platform.IO
    EXPORT Platform.IOTargets
    LIBRARY DESTINATION lib
    ARCHIVE DESTINATION lib
    RUNTIME DESTINATION bin
    INCLUDES DESTINATION include
)

install(EXPORT Platform.IOTargets
    FILE Platform.IOTargets.cmake
    NAMESPACE Platform::
    DESTINATION lib/cmake/Platform.IO
)

# Create package config files
include(CMakePackageConfigHelpers)
write_basic_package_version_file(
    Platform.IOConfigVersion.cmake
    VERSION ${PACKAGE_VERSION}
    COMPATIBILITY AnyNewerVersion
)

configure_package_config_file(
    ${CMAKE_CURRENT_SOURCE_DIR}/Platform.IOConfig.cmake.in
    ${CMAKE_CURRENT_BINARY_DIR}/Platform.IOConfig.cmake
    INSTALL_DESTINATION lib/cmake/Platform.IO
)

install(FILES
    ${CMAKE_CURRENT_BINARY_DIR}/Platform.IOConfig.cmake
    ${CMAKE_CURRENT_BINARY_DIR}/Platform.IOConfigVersion.cmake
    DESTINATION lib/cmake/Platform.IO
)