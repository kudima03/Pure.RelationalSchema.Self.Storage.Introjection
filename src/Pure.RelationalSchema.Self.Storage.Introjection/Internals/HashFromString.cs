using System.Collections;
using Pure.HashCodes;
using Pure.Primitives.Abstractions.String;
using Pure.Primitives.Materialized.String;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record HashFromString : IDeterminedHash
{
    private readonly IString _hash;

    public HashFromString(IString hash)
    {
        _hash = hash;
    }

    public IEnumerator<byte> GetEnumerator()
    {
        return Convert
            .FromHexString(new MaterializedString(_hash).Value)
            .AsEnumerable()
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
