using Pure.HashCodes;
using Pure.HashCodes.Abstractions;
using Pure.Primitives.Abstractions.Bool;
using Pure.Primitives.Abstractions.String;
using Pure.Primitives.Bool;
using Pure.Primitives.Switches.Bool;

namespace Pure.RelationalSchema.Self.Storage.Introjection.Internals;

internal sealed record BoolFromString
{
    private readonly IString _source;

    public BoolFromString(IString source)
    {
        _source = source;
    }

    public IBool Value =>
        new BoolSwitch<IString>(
            _source,
            [
                new KeyValuePair<IString, IBool>(
                    new Primitives.String.String(new True()),
                    new True()
                ),
                new KeyValuePair<IString, IBool>(
                    new Primitives.String.String(new False()),
                    new False()
                ),
            ],
            x => new DeterminedHash(x)
        );
}
