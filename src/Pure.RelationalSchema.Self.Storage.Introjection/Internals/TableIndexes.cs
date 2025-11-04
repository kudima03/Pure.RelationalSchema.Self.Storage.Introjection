using System.Collections;
using Pure.HashCodes.Abstractions;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record TableIndexes : IEnumerable<IDeterminedHash>
{
    private readonly IEnumerable<IRow> _rows;

    public TableIndexes(IDeterminedHash tableHash, IStoredSchemaDataSet schemaDataset)
    {
        _rows = schemaDataset[new TablesToIndexesTable()]
            .Where(x =>
                new HashFromString(
                    x.Cells[new ReferenceToTableColumn()].Value
                ).SequenceEqual(tableHash)
            );
    }

    public IEnumerator<IDeterminedHash> GetEnumerator()
    {
        return _rows
            .Select(row => new HashFromString(
                row.Cells[new ReferenceToIndexColumn()].Value
            ))
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
