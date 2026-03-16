using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.Abstractions.Index;
using Pure.RelationalSchema.Abstractions.Table;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record TableFromRow : ITable
{
    private readonly IRow _row;

    private readonly IStoredSchemaDataSet _schemaDataset;

    public TableFromRow(IString tableId, IStoredSchemaDataSet schemaDataset)
    {
        _row = schemaDataset[new TablesTable()]
            .Single(x => x.Cells[new UuidColumn()].Value.TextValue == tableId.TextValue);
        _schemaDataset = schemaDataset;
    }

    public IString Name => _row.Cells[new NameColumn()].Value;

    public IEnumerable<IColumn> Columns =>
        new TableColumns(_row.Cells[new UuidColumn()].Value, _schemaDataset).Select(
            x => new ColumnFromRow(x, _schemaDataset)
        );

    public IEnumerable<IIndex> Indexes =>
        new TableIndexes(_row.Cells[new UuidColumn()].Value, _schemaDataset).Select(
            x => new IndexFromRow(x, _schemaDataset)
        );
}
