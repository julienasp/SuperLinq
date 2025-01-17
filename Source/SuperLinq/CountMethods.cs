﻿namespace SuperLinq;

public static partial class SuperEnumerable
{
	/// <summary>
	/// Determines whether or not the number of elements in the sequence is greater than
	/// or equal to the given integer.
	/// </summary>
	/// <typeparam name="T">Element type of sequence</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="count">The minimum number of items a sequence must have for this
	/// function to return true</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative</exception>
	/// <returns><see langword="true"/> if the number of elements in the sequence is greater than
	/// or equal to the given integer or <see langword="false"/> otherwise.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 123, 456, 789 };
	/// var result = numbers.AtLeast(2);
	/// ]]></code>
	/// The <c>result</c> variable will contain <see langword="true"/>.
	/// </example>

	public static bool AtLeast<T>(this IEnumerable<T> source, int count)
	{
		Guard.IsGreaterThanOrEqualTo(count, 0);

		return QuantityIterator(source, count, count, int.MaxValue);
	}

	/// <summary>
	/// Determines whether or not the number of elements in the sequence is lesser than
	/// or equal to the given integer.
	/// </summary>
	/// <typeparam name="T">Element type of sequence</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="count">The maximum number of items a sequence must have for this
	/// function to return true</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative</exception>
	/// <returns><see langword="true"/> if the number of elements in the sequence is lesser than
	/// or equal to the given integer or <see langword="false"/> otherwise.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 123, 456, 789 };
	/// var result = numbers.AtMost(2);
	/// ]]></code>
	/// The <c>result</c> variable will contain <see langword="false"/>.
	/// </example>

	public static bool AtMost<T>(this IEnumerable<T> source, int count)
	{
		Guard.IsGreaterThanOrEqualTo(count, 0);

		return QuantityIterator(source, count + 1, 0, count);
	}

	/// <summary>
	/// Determines whether or not the number of elements in the sequence is equals to the given integer.
	/// </summary>
	/// <typeparam name="T">Element type of sequence</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="count">The exactly number of items a sequence must have for this
	/// function to return true</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative</exception>
	/// <returns><see langword="true"/> if the number of elements in the sequence is equals
	/// to the given integer or <see langword="false"/> otherwise.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 123, 456, 789 };
	/// var result = numbers.Exactly(3);
	/// ]]></code>
	/// The <c>result</c> variable will contain <see langword="true"/>.
	/// </example>

	public static bool Exactly<T>(this IEnumerable<T> source, int count)
	{
		Guard.IsGreaterThanOrEqualTo(count, 0);

		return QuantityIterator(source, count + 1, count, count);
	}

	/// <summary>
	/// Determines whether or not the number of elements in the sequence is between
	/// an inclusive range of minimum and maximum integers.
	/// </summary>
	/// <typeparam name="T">Element type of sequence</typeparam>
	/// <param name="source">The source sequence</param>
	/// <param name="min">The minimum number of items a sequence must have for this
	/// function to return true</param>
	/// <param name="max">The maximum number of items a sequence must have for this
	/// function to return true</param>
	/// <exception cref="ArgumentNullException"><paramref name="source"/> is null</exception>
	/// <exception cref="ArgumentOutOfRangeException"><paramref name="min"/> is negative or <paramref name="max"/> is less than min</exception>
	/// <returns><see langword="true"/> if the number of elements in the sequence is between (inclusive)
	/// the min and max given integers or <see langword="false"/> otherwise.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var numbers = new[] { 123, 456, 789 };
	/// var result = numbers.CountBetween(1, 2);
	/// ]]></code>
	/// The <c>result</c> variable will contain <see langword="false"/>.
	/// </example>

	public static bool CountBetween<T>(this IEnumerable<T> source, int min, int max)
	{
		Guard.IsGreaterThanOrEqualTo(min, 0);
		Guard.IsGreaterThanOrEqualTo(max, min);

		return QuantityIterator(source, max + 1, min, max);
	}

	private static bool QuantityIterator<T>(IEnumerable<T> source, int limit, int min, int max)
	{
		Guard.IsNotNull(source);

		var count = source.TryGetCollectionCount() ?? source.CountUpTo(limit);

		return count >= min && count <= max;
	}

	/// <summary>
	/// Compares two sequences and returns an integer that indicates whether the first sequence
	/// has fewer, the same or more elements than the second sequence.
	/// </summary>
	/// <typeparam name="TFirst">Element type of the first sequence</typeparam>
	/// <typeparam name="TSecond">Element type of the second sequence</typeparam>
	/// <param name="first">The first sequence</param>
	/// <param name="second">The second sequence</param>
	/// <exception cref="ArgumentNullException"><paramref name="first"/> is null</exception>
	/// <exception cref="ArgumentNullException"><paramref name="second"/> is null</exception>
	/// <returns><c>-1</c> if the first sequence has the fewest elements, <c>0</c> if the two sequences have the same number of elements
	/// or <c>1</c> if the first sequence has the most elements.</returns>
	/// <example>
	/// <code><![CDATA[
	/// var first = new[] { 123, 456 };
	/// var second = new[] { 789 };
	/// var result = first.CompareCount(second);
	/// ]]></code>
	/// The <c>result</c> variable will contain <c>1</c>.
	/// </example>

	public static int CompareCount<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second)
	{
		Guard.IsNotNull(first);
		Guard.IsNotNull(second);

		if (first.TryGetCollectionCount() is int firstCount)
		{
			return firstCount.CompareTo(second.TryGetCollectionCount() ?? second.CountUpTo(firstCount + 1));
		}
		else if (second.TryGetCollectionCount() is int secondCount)
		{
			return first.CountUpTo(secondCount + 1).CompareTo(secondCount);
		}
		else
		{
			bool firstHasNext;
			bool secondHasNext;

			using var e1 = first.GetEnumerator();
			using (var e2 = second.GetEnumerator())
			{
				do
				{
					firstHasNext = e1.MoveNext();
					secondHasNext = e2.MoveNext();
				}
				while (firstHasNext && secondHasNext);
			}

			return firstHasNext.CompareTo(secondHasNext);
		}
	}

	private static int CountUpTo<T>(this IEnumerable<T> source, int max)
	{
		Guard.IsNotNull(source);
		Guard.IsGreaterThanOrEqualTo(max, 0);

		var count = 0;

		using var e = source.GetEnumerator();
		while (count < max && e.MoveNext())
			count++;

		return count;
	}
}
