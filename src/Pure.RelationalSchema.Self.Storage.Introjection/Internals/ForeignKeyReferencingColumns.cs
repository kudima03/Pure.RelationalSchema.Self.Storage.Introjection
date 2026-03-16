using System.Collections;
using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record ForeignKeyReferencingColumns : IEnumerable<IString>
{
    private readonly IEnumerable<IRow> _rows;

    public ForeignKeyReferencingColumns(
        IString foreignKeyId,
        IStoredSchemaDataSet schemaDataset
    )
    {
        IQueryable<IRow> rows = schemaDataset[new ForeignKeysToReferencingColumnsTable()];
        _rows = rows.Where(x =>
            x.Cells[new ReferenceToForeignKeyColumn()].Value.TextValue
            == foreignKeyId.TextValue
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
