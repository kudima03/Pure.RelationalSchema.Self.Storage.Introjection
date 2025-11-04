using System.Collections;
using Pure.HashCodes.Abstractions;
using Pure.Primitives.Abstractions.String;

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
        return Convert.FromHexString(_hash.TextValue).AsEnumerable().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
