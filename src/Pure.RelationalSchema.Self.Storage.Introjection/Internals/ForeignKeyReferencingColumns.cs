using System.Collections;
using Pure.HashCodes.Abstractions;
using Pure.HashCodes.Abstractions;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record ForeignKeyReferencingColumns : IEnumerable<IDeterminedHash>
{
    private readonly IEnumerable<IRow> _rows;

    public ForeignKeyReferencingColumns(
        IDeterminedHash foreignKeyHash,
        IStoredSchemaDataSet schemaDataset
    )
    {
        _rows = schemaDataset[new ForeignKeysToReferencingColumnsTable()]
            .Where(x =>
                new HashFromString(
                    x.Cells[new ReferenceToForeignKeyColumn()].Value
                ).SequenceEqual(foreignKeyHash)
            );
    }

    public IEnumerator<IDeterminedHash> GetEnumerator()
    {
        return _rows
            .Select(row => new HashFromString(
                row.Cells[new ReferenceToColumnColumn()].Value
            ))
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
