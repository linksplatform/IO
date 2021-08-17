namespace Platform::IO
{
    class StreamExtensions
    {
        public: template <typename T> static void Write(Stream stream, T value)
            where T : struct
        {
            auto bytes = value.ToBytes();
            stream.Write(bytes, 0, bytes.Length);
        }

        public: template <typename T> static T ReadOrDefault(Stream stream)
            where T : struct
        {
            auto size = Structure<T>.Size;
            auto buffer = std::uint8_t[size];
            return stream.Read(buffer, 0, size) == size ? buffer.ToStructure<T>() : 0;
        }

        public: static T ReadAll[]<T>(Stream stream)
            where T : struct
        {
            auto size = Structure<T>.Size;
            auto buffer = std::uint8_t[size];
            auto elementsLength = stream.Length / size;
            T elements[elementsLength] = { {0} };
            for (auto i = 0; i < elementsLength; i++)
            {
                stream.Read(buffer, 0, size);
                elements[i] = buffer.ToStructure<T>();
            }
            return elements;
        }
    };
}
