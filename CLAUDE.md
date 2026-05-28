# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Commands

All `dotnet` commands must be run from the `./src` directory.

```bash
dotnet restore
dotnet build --no-restore -warnaserror
dotnet format --verify-no-changes             # check code style (CI enforces this)
dotnet test --no-build --verbosity normal     # run tests (requires Docker for Testcontainers)
dotnet pack --configuration Release -p:PackageVersion=<version> --output .
```

## Architecture

This is a **projection NuGet library** — it maps storage rows to domain model objects. There are no domain logic implementations; all types are sealed records that wrap an `IStoredSchemaDataSet`.

**Single public type:** `SchemasIntrojection : IQueryable<ISchema>`

It accepts an `IStoredSchemaDataSet` and projects each row from the schemas table into an `ISchema` via internal record types. The object graph is built lazily:

```
SchemasIntrojection → SchemaIntrojection (ISchema)
  SchemaTables → TableFromRow (ITable)
    TableColumns → ColumnFromRow (IColumn) → ColumnTypeFromRow (IColumnType)
    TableIndexes → IndexFromRow (IIndex) → IndexColumns
  SchemaForeignKeys → ForeignKeyFromRow (IForeignKey)
    ForeignKeyReferencingColumns / ForeignKeyReferencedColumns
```

**Table/column descriptors** come from `Pure.RelationalSchema.Self.Schema`. Each internal type keys into the dataset using typed table and column objects (e.g. `new SchemasTable()`, `new UuidColumn()`, `new NameColumn()`).

**Bool-from-string conversion** uses `BoolSwitch<T>` from `Pure.Primitives.Switches` with `DeterminedHash` from `Pure.RelationalSchema.Storage.HashCodes` for key equality.

**Tests** run against a real PostgreSQL instance via Testcontainers — no in-memory or mocked storage. The test creates an actual schema via `Pure.RelationalSchema.Storage.PostgreSQL`, populates it with `Pure.RelationalSchema.Self.Storage.Projection`, introspects it with `SchemasIntrojection`, and verifies structural equality via `SchemaHash`.

**Multi-targeting:** net7.0, net8.0, net9.0, net10.0. `IsAotCompatible` is `false`.

**Package validation:** `EnablePackageValidation = true` with `PackageValidationBaselineVersion = 0.1.0-preview.0.1.0`. Breaking surface changes fail the build.

**Publishing:** triggered by pushing a semver tag (e.g. `1.2.3`). The tag becomes `PackageVersion`. Packages are published to both GitHub Packages and NuGet.org.

## Code Style

Enforced via `.editorconfig` and `dotnet format --verify-no-changes` in CI:

- No `var` — always use explicit types
- No expression-bodied methods or constructors — properties and accessors are expression-bodied
- `using` directives outside the namespace
- Private fields prefixed with `_` (underscore camelCase)
- All `{ }` braces on their own lines (`csharp_new_line_before_open_brace = all`)
- No implicit object creation (`new()`) — always write the type name explicitly
- Max line length: 90 characters

## Commit Messages

Do not mention Claude or AI assistance in commit messages.
