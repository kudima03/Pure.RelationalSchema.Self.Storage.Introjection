using Pure.Primitives.Abstractions.Bool;
using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.Abstractions.Index;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record IndexFromRow : IIndex
{
    private readonly IRow _row;

    private readonly IStoredSchemaDataSet _schemaDataset;

    public IndexFromRow(IString indexId, IStoredSchemaDataSet schemaDataset)
    {
        _row = schemaDataset[new IndexesTable()]
            .Single(x => x.Cells[new UuidColumn()].Value.TextValue == indexId.TextValue);
        _schemaDataset = schemaDataset;
    }

    public IBool IsUnique =>
        new BoolFromString(_row.Cells[new IsUniqueColumn()].Value).Value;

    public IEnumerable<IColumn> Columns =>
        new IndexColumns(_row.Cells[new UuidColumn()].Value, _schemaDataset).Select(
            x => new ColumnFromRow(x, _schemaDataset)
        );
}
