# Changelog

All notable changes to Pure.RelationalSchema.Self.Storage.Introjection are documented here.

Format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).

---

## [0.1.0-preview.1.1.1] — 2026-08-04

- Maintenance release: dependency and build updates.

## [0.1.0-preview.1.1.0] — 2026-06-07

- Maintenance release: dependency and build updates.

## [0.1.0-preview.1.0.0] — 2026-03-16

### Added

- Target framework support extended to `net7.0`, `net8.0`, and `net10.0`
  (previously `net9.0` only).

### Changed

- **Breaking:** the package is no longer marked AOT-compatible
  (`IsAotCompatible` set to `false`).

## [0.1.0-preview.0.1.0] — 2025-11-04

### Added

- **`SchemasIntrojection`** — the package's single public entry point, an
  `IQueryable<ISchema>` over an `IStoredSchemaDataSet`. Reads raw storage
  rows and lazily projects them into the `Pure.RelationalSchema` domain
  model: schemas, tables, columns, column types, indexes, and foreign
  keys.
