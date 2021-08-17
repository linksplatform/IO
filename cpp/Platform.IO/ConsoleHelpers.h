namespace Platform::IO
{
    class ConsoleHelpers
    {
        public: static void PressAnyKeyToContinue()
        {
            printf("Press any key to continue.\n");
            Console.ReadKey();
        }

        public: static std::string GetOrReadArgument(std::int32_t index, params std::string args[]) { return GetOrReadArgument(index, "{index + 1} argument", args); }

        public: static std::string GetOrReadArgument(std::int32_t index, std::string readMessage, params std::string args[])
        {
            if (!args.TryGetElement(index, out std::string result))
            {
                Console.Write(std::string("").append(readMessage).append(": "));
                result = Console.ReadLine();
            }
            if (std::string.IsNullOrEmpty(result))
            {
                return "";
            }
            else
            {
                return result.Trim().TrimSingle('"').Trim();
            }
        }

        public: static void Debug(std::string std::string) { Console.WriteLine(std::string); }

        public: static void Debug(std::string format, params void *args[]) { Console.WriteLine(format, args); }
    };
}
