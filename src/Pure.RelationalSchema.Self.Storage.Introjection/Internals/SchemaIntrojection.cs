using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Abstractions.ForeignKey;
using Pure.RelationalSchema.Abstractions.Schema;
using Pure.RelationalSchema.Abstractions.Table;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record SchemaIntrojection : ISchema
{
    private readonly IRow _row;

    private readonly IStoredSchemaDataSet _schemaDataset;

    public SchemaIntrojection(IString schemaId, IStoredSchemaDataSet schemaDataset)
    {
        _row = schemaDataset[new SchemasTable()]
            .Single(x => x.Cells[new UuidColumn()].Value.TextValue == schemaId.TextValue);
        _schemaDataset = schemaDataset;
    }

    public IString Name => _row.Cells[new NameColumn()].Value;

    public IEnumerable<ITable> Tables =>
        new SchemaTables(_row.Cells[new UuidColumn()].Value, _schemaDataset).Select(
            x => new TableFromRow(x, _schemaDataset)
        );

    public IEnumerable<IForeignKey> ForeignKeys =>
        new SchemaForeignKeys(_row.Cells[new UuidColumn()].Value, _schemaDataset).Select(
            x => new ForeignKeyFromRow(x, _schemaDataset)
        );
}
