using System.Threading;
using System.Threading.Tasks;

namespace LanguageExt.Pipes.Concurrent;

record TransformSource<A, B>(Source<A> Source, Transducer<A, B> Transducer) : Source<B>
{
    internal override SourceIterator<B> GetIterator() =>
        new TransformSourceIterator<A, B>(Source.GetIterator(), Transducer);
}
