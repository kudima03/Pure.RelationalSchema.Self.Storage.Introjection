using System.Collections;
using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record TableIndexes : IEnumerable<IString>
{
    private readonly IEnumerable<IRow> _rows;

    public TableIndexes(IString tableId, IStoredSchemaDataSet schemaDataset)
    {
        IQueryable<IRow> rows = schemaDataset[new TablesToIndexesTable()];
        _rows = rows.Where(x =>
            x.Cells[new ReferenceToTableColumn()].Value.TextValue == tableId.TextValue
        );
    }

    public IEnumerator<IString> GetEnumerator()
    {
        return _rows
            .Select(row => row.Cells[new ReferenceToIndexColumn()].Value)
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
