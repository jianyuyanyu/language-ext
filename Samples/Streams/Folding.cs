using LanguageExt;
using LanguageExt.Common;
using LanguageExt.Traits;
using static Streams.Console;
using static LanguageExt.Prelude;

namespace Streams;

public static class Folding
{
    public static IO<Unit> run =>
        from x in example(1000).Iter().As()
        select unit;

    static StreamT<IO, Unit> example(int n) =>
        from v in Range(0, n).AsStream<IO>()
                             .FoldUntil(0, (s, x) => s + x, (s, _) => s % 10 == 0)
        from _ in writeLine(v.ToString())
        where false
        select unit;
}