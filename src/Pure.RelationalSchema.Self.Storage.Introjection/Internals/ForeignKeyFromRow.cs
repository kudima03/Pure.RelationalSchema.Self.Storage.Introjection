using Pure.HashCodes.Abstractions;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.Abstractions.ForeignKey;
using Pure.RelationalSchema.Abstractions.Table;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;
using Pure.RelationalSchema.Storage.HashCodes;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record ForeignKeyFromRow : IForeignKey
{
    private readonly IRow _row;

    private readonly IStoredSchemaDataSet _schemaDataset;

    public ForeignKeyFromRow(IDeterminedHash rowHash, IStoredSchemaDataSet schemaDataset)
    {
        _row = schemaDataset[new ForeignKeysTable()]
            .Single(x => new RowHash(x).SequenceEqual(rowHash));
        _schemaDataset = schemaDataset;
    }

    public ITable ReferencingTable =>
        new TableFromRow(
            new HashFromString(_row.Cells[new ReferencingTableColumn()].Value),
            _schemaDataset
        );

    public IEnumerable<IColumn> ReferencingColumns =>
        new ForeignKeyReferencingColumns(new RowHash(_row), _schemaDataset).Select(
            x => new ColumnFromRow(x, _schemaDataset)
        );

    public ITable ReferencedTable =>
        new TableFromRow(
            new HashFromString(_row.Cells[new ReferencedTableColumn()].Value),
            _schemaDataset
        );

    public IEnumerable<IColumn> ReferencedColumns =>
        new ForeignKeyReferencedColumns(new RowHash(_row), _schemaDataset).Select(
            x => new ColumnFromRow(x, _schemaDataset)
        );
}
