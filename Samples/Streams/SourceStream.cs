#pragma warning disable LX_StreamT

using System.Threading.Channels;
using LanguageExt;
using static Streams.Console;
using static LanguageExt.Prelude;
using LanguageExt.Pipes.Concurrent;

namespace Streams;

public class SourceStream
{
    public static IO<Unit> run =>
        from c in IO.pure(ConduitT.spawn<IO, string>())
        from f in fork(subscribe(c.Source).Iter())
        from _ in writeLine("Type something and press enter (empty-line ends the demo)") >>
                  interaction(c.Sink)
        select unit;

    static IO<Unit> interaction(Sink<string> sink) =>
        repeat(from l in readLine
               from _ in deliver(sink, l)
               select unit) 
      | @catch(unitIO);

    static IO<Unit> deliver(Sink<string> sink, string line) =>
        guardIO(line != "") >>
        sink.Post(line);

    static SourceT<IO, Unit> subscribe(SourceT<IO, string> source) =>
        from v in source
        from _ in writeLine(v)
        where false
        select unit;
}
