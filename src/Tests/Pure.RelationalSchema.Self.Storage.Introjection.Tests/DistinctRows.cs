using System.Collections;
using Pure.RelationalSchema.Abstractions.Table;
using Pure.RelationalSchema.Storage.Abstractions;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Tests;

internal sealed record DistinctRows : IEnumerable<IGrouping<ITable, IRow>>
{
    private readonly IEnumerable<IGrouping<ITable, IRow>> _rows;

    public DistinctRows(IEnumerable<IGrouping<ITable, IRow>> rows)
    {
        _rows = rows;
    }

    public IEnumerator<IGrouping<ITable, IRow>> GetEnumerator()
    {
        return _rows
            .SelectMany(group =>
                group
                    .DistinctBy(row =>
                        string.Join(
                            '\u001f',
                            row.Cells.Select(cell => cell.Value.Value.TextValue)
                        )
                    )
                    .Select(row => new KeyValuePair<ITable, IRow>(group.Key, row))
            )
            .GroupBy(pair => pair.Key, pair => pair.Value)
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
