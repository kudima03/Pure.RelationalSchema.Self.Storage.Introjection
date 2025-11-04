using System.Collections;
using Pure.HashCodes.Abstractions;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record IndexColumns : IEnumerable<IDeterminedHash>
{
    private readonly IEnumerable<IRow> _rows;

    public IndexColumns(IDeterminedHash indexHash, IStoredSchemaDataSet schemaDataset)
    {
        _rows = schemaDataset[new IndexesToColumnsTable()]
            .Where(x =>
                new HashFromString(
                    x.Cells[new ReferenceToIndexColumn()].Value
                ).SequenceEqual(indexHash)
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
