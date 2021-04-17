using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Platform.Disposables;
using Platform.Threading;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.IO
{
    /// <summary>
    /// <para>Represent the set of helper methods to work with tasks cancellation through console.</para>
    /// <para>Представляет набор вспомогательных методов для работы с отменой задач через консоль.</para>
    /// </summary>
    public class ConsoleCancellation : DisposableBase
    {
        /// <summary>
        /// <para>Gets the <see cref="CancellationTokenSource"/> class instance.</para>
        /// <para>Возвращает экземпляр класса <see cref="CancellationTokenSource"/>.</para>
        /// </summary>
        public CancellationTokenSource Source
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>Gets the <see cref="CancellationToken"/> class instance.</para>
        /// <para>Возвращает экземпляр класса <see cref="CancellationToken"/>.</para>
        /// </summary>
        public CancellationToken Token
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>Gets a <see cref="Boolean"/> value that determines whether cancellation was requested for the <see cref="CancellationTokenSource"/>.</para>
        /// <para>Возвращает значение типа <see cref="Boolean"/>, определяющее, запрошена ли отмена для <see cref="CancellationTokenSource"/>.</para>
        /// </summary>
        public bool IsRequested
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Source.IsCancellationRequested;
        }

        /// <summary>
        /// <para>Gets a <see cref="Boolean"/> value that determines whether cancellation was requested for the <see cref="CancellationTokenSource"/>.</para>
        /// <para>Возвращает значение типа <see cref="Boolean"/>, определяющее, не запрошена ли отмена для <see cref="CancellationTokenSource"/>.</para>
        /// </summary>
        public bool NotRequested
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => !Source.IsCancellationRequested;
        }

        /// <summary>
        /// <para>Initializes the <see cref="ConsoleCancellation"/> class instance, using an <see cref="CancellationTokenSource"/> and its token. The <see cref="ConsoleCancellation"/> subscribes to the <see cref="Console.CancelKeyPress"/> event on initialization.</para>
        /// <para>Инициализирует экземпляр класса <see cref="ConsoleCancellation"/>, используя <see cref="CancellationTokenSource"/> и его токен. <see cref="ConsoleCancellation"/> подписывается на событие <see cref="Console.CancelKeyPress"/> при инициализации.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConsoleCancellation()
        {
            Source = new CancellationTokenSource();
            Token = Source.Token;
            Console.CancelKeyPress += OnCancelKeyPress;
        }

        /// <summary>
        /// <para>Requests force cancellation.</para>
        /// <para>Принудительно запрашивает отмену.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ForceCancellation() => Source.Cancel();

        /// <summary>
        /// <para>Suspends the current thread until a cancellation is requested.</para>
        /// <para>Приостанавливает текущий поток до запроса на отмену.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Wait()
        {
            while (NotRequested)
            {
                ThreadHelpers.Sleep();
            }
        }

        /// <summary>
        /// <para>Unsubscribes from the <see cref="Console.CancelKeyPress"/> event and attempts to dispose the <see cref="CancellationTokenSource"/>.</para>
        /// <para>Отписывается от события <see cref="Console.CancelKeyPress"/> и пытается высвободить ресурсы, используемые <see cref="CancellationTokenSource"/>.</para>
        /// </summary>
        /// <param name="manual">
        /// <para>A <see cref="Boolean"/> value that determines whether the disposal was triggered manually (by the developer's code) or was executed automatically without an explicit indication from the developer.</para>
        /// <para>Значение типа <see cref="Boolean"/>, определяющие было ли высвобождение вызвано вручную (кодом разработчика) или же выполнилось автоматически без явного указания со стороны разработчика.</para>
        /// </param>
        /// <param name="wasDisposed">
        /// <para>A <see cref="Boolean"/> value that determines whether the <see cref="ConsoleCancellation"/> was released before the call to this method.</para>
        /// <para>Значение типа <see cref="Boolean"/>, определяющие были ли освобождены ресурсы, используемые <see cref="ConsoleCancellation"/> до вызова данного метода.</para>
        /// </param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected override void Dispose(bool manual, bool wasDisposed)
        {
            if (!wasDisposed)
            {
                Console.CancelKeyPress -= OnCancelKeyPress;
                Source.DisposeIfPossible();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            if (NotRequested)
            {
                Source.Cancel();
            }
        }
    }
}
