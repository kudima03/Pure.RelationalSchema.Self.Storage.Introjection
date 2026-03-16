using System.Collections;
using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record SchemaTables : IEnumerable<IString>
{
    private readonly IEnumerable<IRow> _rows;

    public SchemaTables(IString schemaId, IStoredSchemaDataSet schemaDataset)
    {
        IQueryable<IRow> rows = schemaDataset[new SchemasToTablesTable()];
        _rows = rows.Where(x =>
            x.Cells[new ReferenceToSchemaColumn()].Value.TextValue == schemaId.TextValue
        );
    }

    public IEnumerator<IString> GetEnumerator()
    {
        return _rows
            .Select(row => row.Cells[new ReferenceToTableColumn()].Value)
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
