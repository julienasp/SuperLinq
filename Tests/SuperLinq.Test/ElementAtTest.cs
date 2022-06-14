﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using NUnit.Framework;

namespace Test;

public class ElementAtTest
{
	// simplified tests - already tested by fx, only need to prove that we don't step on fx toes

	[Test]
	public void SameResultsRepeatCallsIntQuery()
	{
		var q = Enumerable.Repeat(from x in new[] { 9999, 0, 888, -1, 66, -777, 1, 2, -12345 }
								  where x > int.MinValue
								  select x, 3).ToArray();

		Assert.AreEqual(q[1].ElementAt(new Index(3)), q[1].ElementAtOrDefault(new Index(3)));
		Assert.AreEqual(q[2].ElementAt(^6), q[2].ElementAtOrDefault(^6));
	}

	[Test]
	public void SameResultsRepeatCallsStringQuery()
	{
		var q = Enumerable.Repeat(from x in new[] { "!@#$%^", "C", "AAA", "", "Calling Twice", "SoS", string.Empty }
								  where !string.IsNullOrEmpty(x)
								  select x, 3).ToArray();

		Assert.AreEqual(q[1].ElementAt(new Index(4)), q[1].ElementAtOrDefault(new Index(4)));
		Assert.AreEqual(q[2].ElementAt(^2), q[2].ElementAtOrDefault(^2));
	}
}