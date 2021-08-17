namespace Platform::IO
{
    class TemporaryFile : public DisposableBase
    {
        public: std::string Filename = 0;

        public: operator std::string() const { return this->Filename; }

        public: TemporaryFile() { Filename = TemporaryFiles.UseNew(); }

        protected: void Dispose(bool manual, bool wasDisposed) override
        {
            if (!wasDisposed)
            {
                File.Delete(Filename);
            }
        }
    };
}
