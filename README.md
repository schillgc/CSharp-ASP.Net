# C# and ASP.NET Project Archive

This repository was originally a multi-project collection of C# console applications and ASP.NET MVC exercises. It should be treated as a historical project archive, not as a single deployable application.

## Current default-branch status

The current `master` branch does **not** contain the original application source tree wholesale. It retains individually reviewed, source-only recoveries under `archive/console/`, each with its own build/test workflow. The remaining historical source snapshot is reachable in Git history at commit [`da03ed59172336292021fe6cddd07f8007500267`](https://github.com/schillgc/CSharp-ASP.Net/tree/da03ed59172336292021fe6cddd07f8007500267).

That historical snapshot contains 494 tree entries. It includes application source, Visual Studio metadata, compiled binaries, intermediate build output, and vendored package files. Do not restore the snapshot wholesale: that would reintroduce generated artifacts and old dependencies without review.

## Historical project inventory

The pre-cleanup source snapshot included these project areas:

### Console applications

- **Comic Book Gallery Model** — a comic-book library manager and a separate Entity Framework model project.
- **Fitness Frog** — a console-based activity-tracking exercise.
- **Treehouse Defense** — a small object-oriented tower-defense game.
- **Morse Code Translator** — a console translator.
- **Soccer Stats** — a console application for soccer results and player statistics.

### ASP.NET MVC applications

- **Comic Book Gallery** — an ASP.NET MVC comic-book gallery prototype.
- **Fitness Frog** — an MVC activity-tracking sample.
- **JBS Properties** — an MVC / Entity Framework property-management application.

Some related projects now have separate repositories:

- [Comic Book Gallery](https://github.com/schillgc/Comic-Book-Gallery)
- [JBS Properties](https://github.com/SchillingEnterprises/JBS-Properties)

Those repositories have their own maintenance and deployment boundaries. This archive should not become a second, conflicting source of truth for their active versions.

## Restored project

### Morse Code Translator

A source-only recovery of the historical console translator is available at [`archive/console/morse-translator/`](archive/console/morse-translator/). It has been modernized as a standalone .NET 10 LTS application with bidirectional encoding/decoding, explicit validation, a dependency-free contract test harness, and a scoped GitHub Actions workflow.

Build and run the application from its directory:

```bash
dotnet build --configuration Release
dotnet run
dotnet run -- decode
```

Run its contract tests:

```bash
dotnet run --project tests/MorseTranslator.ContractTests/MorseTranslator.ContractTests.csproj --configuration Release
```

The recovery intentionally excludes the historical compiled executable and editor/build artifacts.

### Treehouse Defense

A source-only recovery of the historical tower-defense simulation is available at [`archive/console/treehouse-defense/`](archive/console/treehouse-defense/). It is a standalone .NET 10 console application with map/path validation, five invader types, configurable towers, seeded randomness, a turn limit, and a dependency-free contract test harness.

Build and run it:

```bash
dotnet build archive/console/treehouse-defense/TreehouseDefense.csproj --configuration Release
dotnet run --project archive/console/treehouse-defense/TreehouseDefense.csproj
dotnet run --project archive/console/treehouse-defense/TreehouseDefense.csproj -- --seed 123
```

Run its contract tests:

```bash
dotnet run --project archive/console/treehouse-defense/tests/TreehouseDefense.ContractTests/TreehouseDefense.ContractTests.csproj --configuration Release
```

### Fitness Frog

A source-only recovery of the historical Fitness Frog exercise tracker is available at [`archive/console/fitness-frog/`](archive/console/fitness-frog/). It is a standalone .NET 10 console application with invariant-culture decimal parsing, finite/positive duration validation, checked running totals, and a dependency-free contract test harness. It stores totals only in memory for the current session.

Run it:

```bash
dotnet run --project archive/console/fitness-frog/FitnessFrog.csproj
```

Run its contract tests:

```bash
dotnet run --project archive/console/fitness-frog/tests/FitnessFrog.ContractTests/FitnessFrog.ContractTests.csproj --configuration Release
```

The original executable, generated IDE/build artifacts, and vendored packages were not restored. Other historical projects remain under review and have not been wholesale restored.

### Deferred recovery: Soccer Stats

The historical Soccer Stats source is intentionally not restored yet. Its old program embedded third-party news/sentiment service credentials in source control, and its expected `players.json` dataset is absent from the historical snapshot. The old CSV importer also needs input-validation and parsing work. See [the Soccer Stats recovery/security gate](https://github.com/schillgc/CSharp-ASP.Net/issues/13). Do not copy the historical program or configuration directly into an active project.

## Safe recovery and modernization policy

If historical source is restored to this repository, do it as a reviewed, source-only recovery:

1. Inventory each solution, project file, source file, configuration file, and dependency manifest from the historical commit.
2. Review configuration and source for exposed credentials, connection strings, personal data, and private datasets before restoring anything.
3. Exclude `.vs/`, `bin/`, `obj/`, compiled executables and libraries, caches, and vendored package directories.
4. Preserve project boundaries and commit history. Do not combine unrelated console and MVC applications into one runtime.
5. Record the supported .NET Framework / .NET SDK target for each project. Do not perform broad framework or package upgrades without compatibility tests.
6. Add Windows-based build and test workflows only after each project's restore/build requirements are known.

## Security

- Never commit credentials, populated databases, private customer or tenant records, or production connection strings.
- Treat historical configuration values as potentially exposed until reviewed and rotated.
- Keep any recovered application isolated until its dependencies, authentication, authorization, data handling, and deployment model have been assessed.

## Contribution guidance

Prefer small, project-scoped changes with a documented purpose and reproducible validation. The historical projects are valuable learning and engineering artifacts; preservation and accurate provenance take priority over an unreviewed rewrite.

## Security

See [SECURITY.md](SECURITY.md) for this repository's security, privacy, and responsible-use guidance.
