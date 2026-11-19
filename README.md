# C# and ASP.NET Project Archive

This repository was originally a multi-project collection of C# console applications and ASP.NET MVC exercises. It should be treated as a historical project archive, not as a single deployable application.

## Current default-branch status

The current `master` branch does **not** contain the original application source tree. Its current tree contains repository metadata and the CodeQL workflow only. The source snapshot remains reachable in Git history at commit [`da03ed59172336292021fe6cddd07f8007500267`](https://github.com/schillgc/CSharp-ASP.Net/tree/da03ed59172336292021fe6cddd07f8007500267).

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
