using LanguageExt.Traits;

namespace LanguageExt.Pipes.Concurrent;

record ChooseSourceT<M, A>(SourceT<M, A> SourceTA, SourceT<M, A> SourceTB) : SourceT<M, A>
    where M : MonadIO<M>, Alternative<M>
{
    internal override SourceTIterator<M, A> GetIterator() =>
        new ChooseSourceTIterator<M, A>(SourceTA.GetIterator(), SourceTB.GetIterator());
}
