using Pure.RelationalSchema.Abstractions.Schema;
using Pure.RelationalSchema.HashCodes;
using Pure.RelationalSchema.Self.Schema;
using Pure.RelationalSchema.Self.Storage.Projection;
using Pure.RelationalSchema.Storage.Abstractions;
using Pure.RelationalSchema.Storage.PostgreSQL;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Tests;

public sealed record SchemasFromRowsTests : IAsyncLifetime, IDisposable
{
    private DatabaseFixture? _databaseFixture;

    [Fact]
    public void FetchSchemas()
    {
        ISchema schema = new PostgreSqlCreatedSchema(
            new RelationalSchemaSchema(),
            _databaseFixture!.Connection
        );

        IStoredSchemaDataSet schemaDataSet =
            new PostgreSqlStoredSchemaDataSetWithInsertedRows(
                new PostgreSqlStoredSchemaDataSet(schema, _databaseFixture.Connection),
                new SchemaProjection(new RelationalSchemaSchema())
            );

        _ = schemaDataSet.Values.Sum(x => x.Count());

        Assert.True(
            new SchemaHash(new RelationalSchemaSchema()).SequenceEqual(
                new SchemaHash(new SchemasIntrojection(schemaDataSet).Single())
            )
        );
    }

    public void Dispose()
    {
        _databaseFixture?.Dispose();
    }

    public Task DisposeAsync()
    {
        Dispose();
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        _databaseFixture = new DatabaseFixture();
        return Task.CompletedTask;
    }
}
