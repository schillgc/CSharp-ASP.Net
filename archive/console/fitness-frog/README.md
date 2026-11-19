# Fitness Frog

A source-only recovery of the original Fitness Frog console exercise tracker, modernized as a .NET 10 application.

## Run

From this directory:

```bash
dotnet run --project FitnessFrog.csproj
```

Enter exercise minutes as positive decimal values. Type `quit` to finish. The tracker keeps totals in memory for the current run only; it does not collect identities or persist health/activity records.

## Test

No external test packages are required:

```bash
dotnet run --project tests/FitnessFrog.ContractTests/FitnessFrog.ContractTests.csproj
```

The contract runner covers quit handling, decimal parsing, invalid/non-finite inputs, total aggregation, feedback boundaries, and numeric overflow.

## Engineering notes

- Input parsing is separate from console I/O.
- Numeric parsing uses invariant culture for reproducible decimal input.
- Empty, malformed, non-finite, zero, and negative durations are rejected.
- Session totals are checked for overflow before being committed.
- The old executable and generated IDE/build artifacts were not restored.

This is an educational exercise tracker, not a medical or clinical activity application.
