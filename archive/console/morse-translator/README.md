# Morse Code Translator

A small, dependency-free C# console application recovered from the historical C# project archive and modernized as an independently buildable project.

## Runtime

- .NET 10 LTS
- No third-party runtime packages
- Nullable reference types and compiler warnings-as-errors enabled

.NET 10 is the supported LTS baseline selected for this recovery. Microsoft lists support through November 14, 2028; keep the SDK current with servicing releases.

## Run

From this directory:

```bash
dotnet build --configuration Release
dotnet run
```

The default mode encodes text. To decode Morse code, run:

```bash
dotnet run -- decode
```

The application reads one line at a time from standard input and writes the converted result to standard output. End input with Ctrl+D (macOS/Linux) or Ctrl+Z then Enter (Windows). Use `dotnet run -- --help` for a usage summary.

## Format

- Letters, digits, and the supported punctuation set are separated by spaces.
- A slash separates words.
- Repeated input whitespace is treated as one word boundary.
- Unsupported characters and invalid Morse symbols produce an explicit error rather than a lossy placeholder.

## Tests

Run the dependency-free contract test harness:

```bash
dotnet run --project tests/MorseTranslator.ContractTests/MorseTranslator.ContractTests.csproj --configuration Release
```

GitHub Actions builds the application and runs the same contract tests for changes to this project. The historical compiled executable and IDE/build artifacts were deliberately not restored.
