using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.Abstractions.ForeignKey;
using Pure.RelationalSchema.Abstractions.Table;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record ForeignKeyFromRow : IForeignKey
{
    private readonly IRow _row;

    private readonly IStoredSchemaDataSet _schemaDataset;

    public ForeignKeyFromRow(IString foreignKeyId, IStoredSchemaDataSet schemaDataset)
    {
        _row = schemaDataset[new ForeignKeysTable()]
            .Single(x =>
                x.Cells[new UuidColumn()].Value.TextValue == foreignKeyId.TextValue
            );
        _schemaDataset = schemaDataset;
    }

    public ITable ReferencingTable =>
        new TableFromRow(_row.Cells[new ReferencingTableColumn()].Value, _schemaDataset);

    public IEnumerable<IColumn> ReferencingColumns =>
        new ForeignKeyReferencingColumns(
            _row.Cells[new UuidColumn()].Value,
            _schemaDataset
        ).Select(x => new ColumnFromRow(x, _schemaDataset));

    public ITable ReferencedTable =>
        new TableFromRow(_row.Cells[new ReferencedTableColumn()].Value, _schemaDataset);

    public IEnumerable<IColumn> ReferencedColumns =>
        new ForeignKeyReferencedColumns(
            _row.Cells[new UuidColumn()].Value,
            _schemaDataset
        ).Select(x => new ColumnFromRow(x, _schemaDataset));
}
