﻿namespace SuperLinq.Async;

public static partial class AsyncSuperEnumerable
{
	/// <summary>
	/// Generates an enumerable sequence by repeating a source sequence as long as the given loop condition holds.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="condition">Loop condition.</param>
	/// <param name="source">Sequence to repeat while the condition evaluates true.</param>
	/// <returns>Sequence generated by repeating the given sequence while the condition evaluates to true.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="condition"/> or <paramref name="source"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// <paramref name="condition"/> is evaluated lazily, once at the start of each loop of <paramref name="source"/>.
	/// </para>
	/// <para>
	/// <paramref name="source"/> is cached via <see cref="Memoize{TSource}(IAsyncEnumerable{TSource})"/>, so that it
	/// is only iterated once during the first loop. Successive loops will enumerate the cache instead of <paramref
	/// name="source"/>.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> While<TSource>(Func<bool> condition, IAsyncEnumerable<TSource> source)
	{
		Guard.IsNotNull(condition);
		Guard.IsNotNull(source);

		return While(condition.ToAsync(), source);
	}

	/// <summary>
	/// Generates an enumerable sequence by repeating a source sequence as long as the given loop condition holds.
	/// </summary>
	/// <typeparam name="TSource">Source sequence element type.</typeparam>
	/// <param name="condition">Loop condition.</param>
	/// <param name="source">Sequence to repeat while the condition evaluates true.</param>
	/// <returns>Sequence generated by repeating the given sequence while the condition evaluates to true.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="condition"/> or <paramref name="source"/> is <see
	/// langword="null"/>.</exception>
	/// <remarks>
	/// <para>
	/// <paramref name="condition"/> is evaluated lazily, once at the start of each loop of <paramref name="source"/>.
	/// </para>
	/// <para>
	/// <paramref name="source"/> is cached via <see cref="Memoize{TSource}(IAsyncEnumerable{TSource})"/>, so that it
	/// is only iterated once during the first loop. Successive loops will enumerate the cache instead of <paramref
	/// name="source"/>.
	/// </para>
	/// </remarks>
	public static IAsyncEnumerable<TSource> While<TSource>(Func<ValueTask<bool>> condition, IAsyncEnumerable<TSource> source)
	{
		Guard.IsNotNull(condition);
		Guard.IsNotNull(source);

		return Core(condition, source);

		static async IAsyncEnumerable<TSource> Core(
			Func<ValueTask<bool>> condition,
			IAsyncEnumerable<TSource> source,
			[EnumeratorCancellation] CancellationToken cancellationToken = default)
		{
			await using var memo = source.Memoize();
			while (await condition().ConfigureAwait(false))
			{
				await foreach (var item in memo.WithCancellation(cancellationToken).ConfigureAwait(false))
					yield return item;
			}
		}
	}
}
