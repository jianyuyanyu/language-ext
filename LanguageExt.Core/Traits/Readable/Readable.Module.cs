﻿using System;

namespace LanguageExt.Traits;

public static class Readable
{
    public static K<M, Env> ask<M, Env>()
        where M : Readable<M, Env> =>
        M.Ask;

    public static K<M, A> asks<M, Env, A>(Func<Env, A> f)
        where M : Readable<M, Env> =>
        M.Asks(f);

    public static K<M, A> asksM<M, Env, A>(Func<Env, K<M, A>> f)
        where M : Readable<M, Env>, Monad<M> =>
        M.Flatten(M.Asks(f));

    public static K<M, A> local<M, Env, A>(Func<Env, Env> f, K<M, A> ma)
        where M : Readable<M, Env> =>
        M.Local(f, ma);
}
