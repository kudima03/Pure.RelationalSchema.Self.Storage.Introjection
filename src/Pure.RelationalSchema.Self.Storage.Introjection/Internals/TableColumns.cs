using System.Collections;
using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record TableColumns : IEnumerable<IString>
{
    private readonly IEnumerable<IRow> _rows;

    public TableColumns(IString tableId, IStoredSchemaDataSet schemaDataset)
    {
        IQueryable<IRow> rows = schemaDataset[new TablesToColumnsTable()];
        _rows = rows.Where(x =>
            x.Cells[new ReferenceToTableColumn()].Value.TextValue == tableId.TextValue
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
