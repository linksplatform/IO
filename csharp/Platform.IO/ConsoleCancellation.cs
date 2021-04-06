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
        /// <para>Returns the <see cref="CancellationTokenSource"/> object.</para>
        /// <para>Возвращает объект <see cref="CancellationTokenSource"/>.</para>
        /// </summary>
        public CancellationTokenSource Source
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>Returns the <see cref="CancellationToken"/> object.</para>
        /// <para>Возвращает объект <see cref="CancellationToken"/>.</para>
        /// </summary>
        public CancellationToken Token
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get;
        }

        /// <summary>
        /// <para>Gets whether cancellation has been requested for this <see cref="CancellationTokenSource"/>.</para>
        /// <para>Получает значение, указывающее, была ли получена отмена для <see cref="CancellationTokenSource"/>.</para>
        /// </summary>
        public bool IsRequested
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Source.IsCancellationRequested;
        }

        /// <summary>
        /// <para>Gets whether cancellation has not been requested for this <see cref="CancellationTokenSource"/>.</para>
        /// <para>Получает не была ли получена отмена для <see cref="CancellationTokenSource"/>.</para>
        /// </summary>
        public bool NotRequested
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => !Source.IsCancellationRequested;
        }

        /// <summary>
        /// <para>Initializes the <see cref="CancellationTokenSource"/> instance, assigns token to <see cref="Token"/>, adds the <see cref="OnCancelKeyPress(object, ConsoleCancelEventArgs)"/> handler for the <see cref="Console.CancelKeyPress"/>.</para>
        /// <para>Инициализирует экземпляр <see cref="CancellationTokenSource"/>, присваивает <see cref="Token"/> токен, добавляет обработчик <see cref="OnCancelKeyPress(object, ConsoleCancelEventArgs)"/> для события <see cref="Console.CancelKeyPress"/>.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ConsoleCancellation()
        {
            Source = new CancellationTokenSource();
            Token = Source.Token;
            Console.CancelKeyPress += OnCancelKeyPress;
        }

        /// <summary>
        /// <para>Requests cancellation.</para>
        /// <para>Запрашивает отмену.</para>
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ForceCancellation() => Source.Cancel();

        /// <summary>
        /// <para>Suspends the current thread until a cancellation request.</para>
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
        /// <para>Removes <see cref="Console.CancelKeyPress"/> handler</para>
        /// <para></para>
        /// </summary>
        /// <param name="manual"></param>
        /// <param name="wasDisposed">
        /// <para>Specifies whether the <see cref="Source"/> was disposed.</para>
        /// <para>Указывает были ли освобождены ресурсы, используемые <see cref="Source"/>.</para>
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

        /// <summary>
        /// <para>Requests cancellation if cancellation was not requested.</para>
        /// <para>Запрашивает отмену если отмена не запрашивалась.</para>
        /// </summary>
        /// <param name="sender">
        /// <para>The event source.</para>
        /// <para>Источник события.</para>
        /// </param>
        /// <param name="e">
        /// <para>The <see cref="ConsoleCancelEventArgs"/> instance that provides data for the CancelKeyPress event.</para>
        /// <para>Экземпляр <see cref="ConsoleCancelEventArgs"/> предоставляющий данные для события CancelKeyPress.</para>
        /// </param>
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
