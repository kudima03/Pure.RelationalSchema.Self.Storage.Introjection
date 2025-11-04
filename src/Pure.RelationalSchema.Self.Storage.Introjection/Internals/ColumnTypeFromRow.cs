using Pure.HashCodes.Abstractions;
using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Abstractions.ColumnType;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;
using Pure.RelationalSchema.Storage.HashCodes;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record ColumnTypeFromRow : IColumnType
{
    private readonly IRow _row;

    public ColumnTypeFromRow(IDeterminedHash rowHash, IStoredSchemaDataSet schemaDataset)
        : this(
            schemaDataset[new ColumnTypesTable()]
                .Single(x => new RowHash(x).SequenceEqual(rowHash))
        )
    { }

    public ColumnTypeFromRow(IRow row)
    {
        _row = row;
    }

    public IString Name => _row.Cells[new NameColumn()].Value;
}
