using Pure.HashCodes.Abstractions;
using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.Abstractions.Index;
using Pure.RelationalSchema.Abstractions.Table;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;
using Pure.RelationalSchema.Storage.HashCodes;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record TableFromRow : ITable
{
    private readonly IRow _row;

    private readonly IStoredSchemaDataSet _schemaDataset;

    public TableFromRow(IDeterminedHash rowHash, IStoredSchemaDataSet schemaDataset)
    {
        _row = schemaDataset[new TablesTable()]
            .Single(x => new RowHash(x).SequenceEqual(rowHash));
        _schemaDataset = schemaDataset;
    }

    public IString Name => _row.Cells[new NameColumn()].Value;

    public IEnumerable<IColumn> Columns =>
        new TableColumns(new RowHash(_row), _schemaDataset).Select(x => new ColumnFromRow(
            x,
            _schemaDataset
        ));

    public IEnumerable<IIndex> Indexes =>
        new TableIndexes(new RowHash(_row), _schemaDataset).Select(x => new IndexFromRow(
            x,
            _schemaDataset
        ));
}
