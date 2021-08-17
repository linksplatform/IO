namespace Platform::IO
{
    class ConsoleCancellation : public DisposableBase
    {
        public: const CancellationTokenSource Source;

        public: const CancellationToken Token;

        public: bool IsRequested()
        {
            return Source.IsCancellationRequested;
        }

        public: bool NotRequested()
        {
            return !Source.IsCancellationRequested;
        }

        public: ConsoleCancellation()
        {
            Source = this->CancellationTokenSource();
            Token = Source.Token;
            Console.CancelKeyPress += OnCancelKeyPress;
        }

        public: void ForceCancellation() { Source.Cancel(); }

        public: void Wait()
        {
            while (NotRequested)
            {
                ThreadHelpers.Sleep();
            }
        }

        protected: void Dispose(bool manual, bool wasDisposed) override
        {
            if (!wasDisposed)
            {
                Console.CancelKeyPress -= OnCancelKeyPress;
                Source.DisposeIfPossible();
            }
        }

        private: void OnCancelKeyPress(void *sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            if (NotRequested)
            {
                Source.Cancel();
            }
        }
    };
}
