#include <stop_token>

namespace Platform::IO
{
    class ConsoleCancellation
    {
    private:
        const std::stop_source _source;

    private:
        const std::stop_token _token;

    public:
        auto Token
        -> std::stop_token { return _token; }

    public:
        auto Source
        -> std::stop_source { return _source; }

    public:
        bool IsRequested()
        {
            return _source.stop_requested;
        }

    public:
        bool NotRequested()
        {
            return !_source.stop_requested;
        }

    public:
        ConsoleCancellation()
                : _source{std::stop_source{}}

        : _token { _source.get_token() }
        {
            Source = this->CancellationTokenSource();
            Token = Source.Token;
            Console.CancelKeyPress += OnCancelKeyPress;
        }

    public:
        ~ConsoleCancellation()
        {
            // ToDo
        }

    public:
        void ForceCancellation()
        { _source.request_stop; }

    public:
        void Wait()
        {
            while (NotRequested) {
                ThreadHelpers.Sleep();
            }
        }

//    private:
//        void OnCancelKeyPress(void *sender, ConsoleCancelEventArgs e)
//        {
//            e.Cancel = true;
//            if (NotRequested) {
//                Source.Cancel();
//            }
//        }
//
//    private:
//        void my_handler(int s)
//        {
//            printf("Caught signal %d\n", s);
//            exit(1);
//
//        }
    };
}
