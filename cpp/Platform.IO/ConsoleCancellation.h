#pragma once
#include <atomic>
#include <chrono>
#include <thread>
#include <csignal>

namespace Platform::IO
{
    /// <summary>
    /// <para>Represents the class that simplifies the console applications implementation that can be terminated manually during execution.</para>
    /// <para>Представляет класс, упрощающий реализацию консольных приложений, выполнение которых может быть прекращено в процессе выполнения вручную.</para>
    /// </summary>
    class ConsoleCancellation
    {
    private:
        std::atomic<bool> _isCancellationRequested{false};
        static ConsoleCancellation* _currentInstance;

        static void SignalHandler(int signal)
        {
            if (_currentInstance && signal == SIGINT) {
                _currentInstance->_isCancellationRequested = true;
            }
        }

    public:
        /// <summary>
        /// <para>Gets a value that determines whether cancellation was requested.</para>
        /// <para>Возвращает значение, определяющее, запрошена ли отмена.</para>
        /// </summary>
        bool IsRequested() const
        {
            return _isCancellationRequested.load();
        }

        /// <summary>
        /// <para>Gets a value that determines whether cancellation was not requested.</para>
        /// <para>Возвращает значение, определяющее, не запрошена ли отмена.</para>
        /// </summary>
        bool NotRequested() const
        {
            return !_isCancellationRequested.load();
        }

        /// <summary>
        /// <para>Initializes a ConsoleCancellation class instance. The ConsoleCancellation subscribes to the SIGINT signal on initialization.</para>
        /// <para>Инициализирует экземпляр класса ConsoleCancellation. ConsoleCancellation подписывается на сигнал SIGINT при инициализации.</para>
        /// </summary>
        ConsoleCancellation()
        {
            _currentInstance = this;
            std::signal(SIGINT, SignalHandler);
        }

        /// <summary>
        /// <para>Copy constructor (deleted to prevent copying).</para>
        /// <para>Конструктор копирования (удален для предотвращения копирования).</para>
        /// </summary>
        ConsoleCancellation(const ConsoleCancellation&) = delete;

        /// <summary>
        /// <para>Copy assignment operator (deleted to prevent copying).</para>
        /// <para>Оператор присваивания копированием (удален для предотвращения копирования).</para>
        /// </summary>
        ConsoleCancellation& operator=(const ConsoleCancellation&) = delete;

        /// <summary>
        /// <para>Move constructor.</para>
        /// <para>Конструктор перемещения.</para>
        /// </summary>
        ConsoleCancellation(ConsoleCancellation&& other) noexcept
        {
            _isCancellationRequested.store(other._isCancellationRequested.load());
            _currentInstance = this;
            other._currentInstance = nullptr;
        }

        /// <summary>
        /// <para>Move assignment operator.</para>
        /// <para>Оператор присваивания перемещением.</para>
        /// </summary>
        ConsoleCancellation& operator=(ConsoleCancellation&& other) noexcept
        {
            if (this != &other) {
                _isCancellationRequested.store(other._isCancellationRequested.load());
                _currentInstance = this;
                other._currentInstance = nullptr;
            }
            return *this;
        }

        /// <summary>
        /// <para>Forces cancellation request.</para>
        /// <para>Принудительно запрашивает отмену.</para>
        /// </summary>
        void ForceCancellation()
        {
            _isCancellationRequested = true;
        }

        /// <summary>
        /// <para>Suspends the current thread until a cancellation is requested.</para>
        /// <para>Приостанавливает текущий поток до запроса на отмену.</para>
        /// </summary>
        void Wait()
        {
            while (NotRequested()) {
                std::this_thread::sleep_for(std::chrono::milliseconds(100));
            }
        }

        /// <summary>
        /// <para>Suspends the current thread until a cancellation is requested or timeout occurs.</para>
        /// <para>Приостанавливает текущий поток до запроса на отмену или истечения времени ожидания.</para>
        /// </summary>
        template<typename Rep, typename Period>
        bool WaitFor(const std::chrono::duration<Rep, Period>& timeout)
        {
            auto start = std::chrono::steady_clock::now();
            while (NotRequested()) {
                auto elapsed = std::chrono::steady_clock::now() - start;
                if (elapsed >= timeout) {
                    return false; // Timeout occurred
                }
                std::this_thread::sleep_for(std::chrono::milliseconds(10));
            }
            return true; // Cancellation was requested
        }

        /// <summary>
        /// <para>Unsubscribes from the SIGINT signal.</para>
        /// <para>Отписывается от сигнала SIGINT.</para>
        /// </summary>
        ~ConsoleCancellation()
        {
            if (_currentInstance == this) {
                std::signal(SIGINT, SIG_DFL); // Restore default signal handler
                _currentInstance = nullptr;
            }
        }
    };

    // Static member definition
    ConsoleCancellation* ConsoleCancellation::_currentInstance = nullptr;
}
