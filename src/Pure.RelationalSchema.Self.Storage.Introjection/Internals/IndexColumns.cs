using System.Collections;
using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record IndexColumns : IEnumerable<IString>
{
    private readonly IEnumerable<IRow> _rows;

    public IndexColumns(IString indexId, IStoredSchemaDataSet schemaDataset)
    {
        IQueryable<IRow> rows = schemaDataset[new IndexesToColumnsTable()];
        _rows = rows.Where(x =>
            x.Cells[new ReferenceToIndexColumn()].Value.TextValue == indexId.TextValue
        );
    }

    public IEnumerator<IString> GetEnumerator()
    {
        return _rows
            .Select(row => row.Cells[new ReferenceToColumnColumn()].Value)
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
