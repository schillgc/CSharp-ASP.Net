using FitnessFrog;

var tests = new (string Name, Action Run)[]
{
    ("Quit is case-insensitive and whitespace-tolerant", ParsesQuit),
    ("Invariant decimal values are accepted", ParsesFractionalMinutes),
    ("Zero and negative values are rejected", RejectsNonPositiveMinutes),
    ("Malformed and non-finite values are rejected", RejectsInvalidNumbers),
    ("Session tracks totals and entry counts", TracksSessionTotals),
    ("Feedback thresholds are stable", UsesFeedbackThresholds),
    ("Session rejects overflow without changing totals", RejectsTotalOverflow),
    ("Entry-count overflow leaves session state unchanged", RejectsEntryCountOverflow)
};

var failures = new List<string>();
foreach (var (name, run) in tests)
{
    try
    {
        run();
        Console.WriteLine($"PASS {name}");
    }
    catch (Exception exception)
    {
        failures.Add($"{name}: {exception.Message}");
        Console.WriteLine($"FAIL {name}: {exception.Message}");
    }
}

Console.WriteLine($"{tests.Length - failures.Count}/{tests.Length} contract tests passed.");
return failures.Count == 0 ? 0 : 1;

static void ParsesQuit()
{
    AssertEqual(EntryKind.Quit, ExerciseEntryParser.Parse(" QUIT ").Kind);
    AssertEqual(EntryKind.Quit, ExerciseEntryParser.Parse("quit").Kind);
    AssertEqual(EntryKind.Quit, ExerciseEntryParser.Parse(null).Kind);
}

static void ParsesFractionalMinutes()
{
    var result = ExerciseEntryParser.Parse(" 12.5 ");
    AssertEqual(EntryKind.Valid, result.Kind);
    AssertEqual(12.5, result.Minutes);
}

static void RejectsNonPositiveMinutes()
{
    AssertEqual(EntryKind.Invalid, ExerciseEntryParser.Parse("0").Kind);
    AssertEqual(EntryKind.Invalid, ExerciseEntryParser.Parse("-3").Kind);
}

static void RejectsInvalidNumbers()
{
    foreach (var input in new[] { "", "ten", "NaN", "Infinity", "-Infinity" })
    {
        AssertEqual(EntryKind.Invalid, ExerciseEntryParser.Parse(input).Kind);
    }
}

static void TracksSessionTotals()
{
    var session = new FitnessSession();
    session.Add(10.25);
    session.Add(20.5);
    AssertEqual(30.75, session.TotalMinutes);
    AssertEqual(2, session.EntryCount);
}

static void UsesFeedbackThresholds()
{
    AssertEqual("Better than nothing, am I right?", ActivityFeedback.For(10));
    AssertEqual("Way to go!", ActivityFeedback.For(10.01));
    AssertEqual("Way to go!", ActivityFeedback.For(30));
    AssertEqual("You must be a ninja warrior in training!", ActivityFeedback.For(30.01));
    AssertEqual("You must be a ninja warrior in training!", ActivityFeedback.For(60));
    AssertEqual("Okay, now you're just showing off!", ActivityFeedback.For(60.01));
}

static void RejectsTotalOverflow()
{
    var session = new FitnessSession();
    session.Add(double.MaxValue);
    AssertThrows<OverflowException>(() => session.Add(double.MaxValue));
    AssertEqual(double.MaxValue, session.TotalMinutes);
    AssertEqual(1, session.EntryCount);
}

static void RejectsEntryCountOverflow()
{
    var session = new FitnessSession();
    typeof(FitnessSession).GetProperty(nameof(FitnessSession.EntryCount))!
        .SetValue(session, int.MaxValue);

    AssertThrows<OverflowException>(() => session.Add(1));
    AssertEqual(0d, session.TotalMinutes);
    AssertEqual(int.MaxValue, session.EntryCount);
}

static void AssertEqual<T>(T expected, T actual)
{
    if (!EqualityComparer<T>.Default.Equals(expected, actual))
    {
        throw new InvalidOperationException($"Expected '{expected}', received '{actual}'.");
    }
}

static void AssertThrows<TException>(Action action) where TException : Exception
{
    try
    {
        action();
    }
    catch (TException)
    {
        return;
    }

    throw new InvalidOperationException($"Expected {typeof(TException).Name}.");
}
