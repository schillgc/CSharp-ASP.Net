# Security Policy

## Scope

This repository is a historical multi-project C# / ASP.NET archive. The current default branch contains selected source-only recoveries and is not one deployable application. The preserved historical snapshot contains older project source, build output, binaries, and vendored dependencies; do not restore it wholesale.

## Safe recovery

- Review each project independently before building or running it.
- Never commit credentials, connection strings, populated databases, private datasets, or real user records.
- Treat credentials present in historical commits as exposed. Revoke or rotate them with the issuing provider; removing a value from the current tree does not remove it from Git history.
- Exclude compiled output, IDE metadata, caches, and vendored package directories from new recoveries.
- Use supported SDKs and dependencies where compatible, with project-scoped build and contract tests.
- Do not restore the deferred Soccer Stats application until its historical credentials, missing dataset, and CSV input-validation concerns have been resolved as documented in issue #13.

## Reporting

Report vulnerabilities privately through GitHub's private vulnerability reporting feature when available. Do not disclose secrets or private datasets in public issues or pull requests.
