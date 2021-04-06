using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Platform.Disposables;
using Platform.Threading;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Platform.IO
{
    public class ConsoleCancellation : DisposableBase
    {
        /// <summary>
        /// <para>Returns the <see cref="CancellationTokenSource"></see> object.</para>
        /// <para>Возвращает объект <see cref="CancellationTokenSource"></see>.</para>
        /// </summary>
        public CancellationTokenSource Source
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>Returns the <see cref="CancellationToken"></see> object.</para>
        /// <para>Возвращает объект <see cref="CancellationToken"></see>.</para>
        /// </summary>
        public CancellationToken Token
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>Gets whether cancellation has been requested for this <see cref="CancellationTokenSource"></see>.</para>
        /// <para>Получает значение, указывающее, была ли получена отмена для <see cref="CancellationTokenSource"></see>.</para>
        /// </summary>
        public bool IsRequested
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Source.IsCancellationRequested;
        }

        /// <summary>
        /// <para>Gets whether cancellation has not been requested for this <see cref="CancellationTokenSource"></see>.</para>
        /// <para>Получает не была ли получена отмена для <see cref="CancellationTokenSource"></see>.</para>
        /// </summary>
        public bool NotRequested
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => !Source.IsCancellationRequested;
        }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="CancellationTokenSource"></see>.</para>
        /// <para>Инициализирует новый экземпляр <see cref="CancellationTokenSource"></see>.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConsoleCancellation()
        {
            Source = new CancellationTokenSource();
            Token = Source.Token;
            Console.CancelKeyPress += OnCancelKeyPress;
        }

        /// <summary>
        /// <para>Communicates a request for cancellation.</para>
        /// <para>Передает запрос на отмену.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ForceCancellation() => Source.Cancel();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Wait()
        {
            while (NotRequested)
            {
                ThreadHelpers.Sleep();
            }
        }

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
