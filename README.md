# Pure.RelationalSchema.Self.Storage.Introjection

Converts relational schema storage rows into a LINQ-queryable `IQueryable<ISchema>` object graph — schema introspection over `IStoredSchemaDataSet`.

[![.NET build & test](https://github.com/kudima03/Pure.RelationalSchema.Self.Storage.Introjection/actions/workflows/build-and-test.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.RelationalSchema.Self.Storage.Introjection/actions/workflows/build-and-test.yml)
[![Build and Deploy](https://github.com/kudima03/Pure.RelationalSchema.Self.Storage.Introjection/actions/workflows/publish-nuget.yml/badge.svg?branch=main)](https://github.com/kudima03/Pure.RelationalSchema.Self.Storage.Introjection/actions/workflows/publish-nuget.yml)
[![NuGet](https://img.shields.io/nuget/v/Pure.RelationalSchema.Self.Storage.Introjection)](https://www.nuget.org/packages/Pure.RelationalSchema.Self.Storage.Introjection)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

## Overview

`Pure.RelationalSchema.Self.Storage.Introjection` reads raw rows from an `IStoredSchemaDataSet` and projects them into the Pure.RelationalSchema domain model — schemas, tables, columns, column types, indexes, and foreign keys — all exposed as a navigable `IQueryable<ISchema>`.

The single public entry point is `SchemasIntrojection`. It wraps a dataset and lazily builds the full schema object graph as the sequence is enumerated.

## Public API

| Type | Kind | Description |
|------|------|-------------|
| `SchemasIntrojection` | `sealed record` | `IQueryable<ISchema>` over an `IStoredSchemaDataSet` |

`SchemasIntrojection` implements `IQueryable<ISchema>` and proxies `ElementType`, `Expression`, and `Provider` from the underlying LINQ query. Enumeration materialises each `ISchema` with its complete object graph: tables → columns → types, tables → indexes, and schema → foreign keys.

## Dependencies

- [`Pure.RelationalSchema.Self.Schema`](https://github.com/kudima03/Pure.RelationalSchema.Self.Schema/tree/0.1.0-preview.6.0.0) — self-describing table and column definitions that describe how relational schema metadata is stored
- [`Pure.Primitives.Switches`](https://github.com/kudima03/Pure.Primitives.Switches/tree/1.1.0) — type-safe switch primitives used for `IBool`/`IString` value conversions
- [`Pure.RelationalSchema.Storage.HashCodes`](https://github.com/kudima03/Pure.RelationalSchema.Storage.HashCodes/tree/0.1.0-preview.2.1.0) — hash code utilities for storage row equality and lookup

## Target Frameworks

- .NET 7
- .NET 8
- .NET 9
- .NET 10

## Installation

```bash
dotnet add package Pure.RelationalSchema.Self.Storage.Introjection
```

## Usage

```csharp
// Obtain a dataset from Pure.RelationalSchema.Storage.PostgreSQL or similar
IStoredSchemaDataSet schemaDataSet = ...;

IQueryable<ISchema> schemas = new SchemasIntrojection(schemaDataSet);

foreach (ISchema schema in schemas)
{
    Console.WriteLine(schema.Name.TextValue);
    foreach (ITable table in schema.Tables)
        Console.WriteLine($"  {table.Name.TextValue}");
}
```
