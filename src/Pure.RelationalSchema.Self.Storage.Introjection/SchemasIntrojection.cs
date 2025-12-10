using System.Collections;
using System.Linq.Expressions;
using Pure.RelationalSchema.Abstractions.Schema;
using Pure.RelationalSchema.Self.Schema.Tables;
using Pure.RelationalSchema.Self.Storage.Introjection.Internals;
using Pure.RelationalSchema.Storage.Abstractions;
using Pure.RelationalSchema.Storage.HashCodes;

namespace Pure.RelationalSchema.Self.Storage.Introjection;

public sealed record SchemasIntrojection : IQueryable<ISchema>
{
    private readonly IQueryable<ISchema> _rows;

    public SchemasIntrojection(IStoredSchemaDataSet dataset)
    {
        IQueryable<IRow> rows = dataset[new SchemasTable()];
        _rows = rows.Select(x => new SchemaIntrojection(new RowHash(x), dataset));
    }

    public Type ElementType => _rows.ElementType;

    public Expression Expression => _rows.Expression;

    public IQueryProvider Provider => _rows.Provider;

    public IEnumerator<ISchema> GetEnumerator()
    {
        return _rows.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
