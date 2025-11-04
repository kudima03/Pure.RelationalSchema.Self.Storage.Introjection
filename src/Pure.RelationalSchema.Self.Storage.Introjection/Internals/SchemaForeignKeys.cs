using System.Collections;
using Pure.HashCodes.Abstractions;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record SchemaForeignKeys : IEnumerable<IDeterminedHash>
{
    private readonly IEnumerable<IRow> _rows;

    public SchemaForeignKeys(
        IDeterminedHash schemaHash,
        IStoredSchemaDataSet schemaDataset
    )
    {
        _rows = schemaDataset[new SchemasToForeignKeysTable()]
            .Where(x =>
                new HashFromString(
                    x.Cells[new ReferenceToSchemaColumn()].Value
                ).SequenceEqual(schemaHash)
            );
    }

    public IEnumerator<IDeterminedHash> GetEnumerator()
    {
        return _rows
            .Select(row => new HashFromString(
                row.Cells[new ReferenceToForeignKeyColumn()].Value
            ))
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
