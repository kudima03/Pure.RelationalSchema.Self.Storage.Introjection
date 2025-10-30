using Pure.HashCodes;
using Pure.Primitives.Abstractions.Bool;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.Abstractions.Index;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;
using Pure.RelationalSchema.Storage.HashCodes;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record IndexFromRow : IIndex
{
    private readonly IRow _row;

    private readonly IStoredSchemaDataSet _schemaDataset;

    public IndexFromRow(IDeterminedHash rowHash, IStoredSchemaDataSet schemaDataset)
    {
        _row = schemaDataset[new IndexesTable()]
            .Single(x => new RowHash(x).SequenceEqual(rowHash));
        _schemaDataset = schemaDataset;
    }

    public IBool IsUnique =>
        new BoolFromString(_row.Cells[new IsUniqueColumn()].Value).Value;

    public IEnumerable<IColumn> Columns =>
        new IndexColumns(new RowHash(_row), _schemaDataset).Select(x => new ColumnFromRow(
            x,
            _schemaDataset
        ));
}
