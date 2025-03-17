using LanguageExt.Traits;

namespace LanguageExt.Pipes.Concurrent;

record ForeverSourceT<M, A>(K<M, A> Value) : SourceT<M, A>
    where M : MonadIO<M>, Alternative<M>
{
    internal override SourceTIterator<M, A> GetIterator() =>
        new ForeverSourceTIterator<M, A>(Value);
}
