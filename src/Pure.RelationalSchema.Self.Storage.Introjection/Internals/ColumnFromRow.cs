using Pure.HashCodes.Abstractions;
using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Abstractions.Column;
using Pure.RelationalSchema.Abstractions.ColumnType;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;
using Pure.RelationalSchema.Storage.HashCodes;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record ColumnFromRow : IColumn
{
    private readonly IRow _row;

    private readonly IStoredSchemaDataSet _schemaDataset;

    public ColumnFromRow(IDeterminedHash rowHash, IStoredSchemaDataSet schemaDataset)
    {
        _row = schemaDataset[new ColumnsTable()]
            .Single(x => new RowHash(x).SequenceEqual(rowHash));
        _schemaDataset = schemaDataset;
    }

    public IString Name => _row.Cells[new NameColumn()].Value;

    public IColumnType Type =>
        new ColumnTypeFromRow(
            new HashFromString(_row.Cells[new ReferenceToColumnTypeColumn()].Value),
            _schemaDataset
        );
}
