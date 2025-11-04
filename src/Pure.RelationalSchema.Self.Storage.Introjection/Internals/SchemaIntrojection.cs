using Pure.HashCodes.Abstractions;
using Pure.Primitives.Abstractions.String;
using Pure.RelationalSchema.Abstractions.ForeignKey;
using Pure.RelationalSchema.Abstractions.Schema;
using Pure.RelationalSchema.Abstractions.Table;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;
using Pure.RelationalSchema.Storage.HashCodes;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record SchemaIntrojection : ISchema
{
    private readonly IRow _row;

    private readonly IStoredSchemaDataSet _schemaDataset;

    public SchemaIntrojection(IDeterminedHash rowHash, IStoredSchemaDataSet schemaDataset)
    {
        _row = schemaDataset[new SchemasTable()]
            .Single(x => new RowHash(x).SequenceEqual(rowHash));
        _schemaDataset = schemaDataset;
    }

    public IString Name => _row.Cells[new NameColumn()].Value;

    public IEnumerable<ITable> Tables =>
        new SchemaTables(new RowHash(_row), _schemaDataset).Select(x => new TableFromRow(
            x,
            _schemaDataset
        ));

    public IEnumerable<IForeignKey> ForeignKeys =>
        new SchemaForeignKeys(new RowHash(_row), _schemaDataset).Select(
            x => new ForeignKeyFromRow(x, _schemaDataset)
        );
}
