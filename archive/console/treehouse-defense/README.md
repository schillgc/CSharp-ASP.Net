# Treehouse Defense

A source-only recovery of the original Treehouse Defense console game from the historical C# project archive, rebuilt as a deterministic .NET 10 console application.

## Run

From this directory:

```bash
dotnet run --project TreehouseDefense.csproj
dotnet run --project TreehouseDefense.csproj -- --seed 123
```

The default seed is `42`. Supplying `--seed INTEGER` makes the simulation reproducible.

## Test

No external test packages are required:

```bash
dotnet run --project tests/TreehouseDefense.ContractTests/TreehouseDefense.ContractTests.csproj
```

The contract runner checks map boundaries, path validation, fast-invader movement, deterministic tower damage, win/loss resolution, and repeatability for a fixed random seed.

## Engineering notes

- The game engine is separated into map, path, invader, tower, and level types.
- Randomness is injected through a seeded `GameRandom` instance instead of using a hidden process-wide random generator.
- Invalid dimensions, empty paths, mixed-map paths, invalid tower settings, and non-positive damage are rejected explicitly.
- A level has a maximum-turn guard so malformed simulation states cannot loop forever.
- The old executable and generated IDE/build artifacts were not restored.

This is a learning project and simulation, not a production game service.
