using System.Collections;
using Pure.HashCodes.Abstractions;
using Pure.RelationalSchema.Self.Schema.Columns;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record SchemaTables : IEnumerable<IDeterminedHash>
{
    private readonly IEnumerable<IRow> _rows;

    public SchemaTables(IDeterminedHash schemaHash, IStoredSchemaDataSet schemaDataset)
    {
        _rows = schemaDataset[new SchemasToTablesTable()]
            .Where(x =>
                new HashFromString(
                    x.Cells[new ReferenceToSchemaColumn()].Value
                ).SequenceEqual(schemaHash)
            );
    }

    public IEnumerator<IDeterminedHash> GetEnumerator()
    {
        return _rows
            .Select(row => new HashFromString(
                row.Cells[new ReferenceToTableColumn()].Value
            ))
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
